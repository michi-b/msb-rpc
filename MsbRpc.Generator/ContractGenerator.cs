using System;
using System.Collections.Immutable;
using System.Linq;
using System.Threading;
using Microsoft.CodeAnalysis;
using MsbRpc.Generator.CodeWriters.Files;
using MsbRpc.Generator.CodeWriters.Files.Base;
using MsbRpc.Generator.Extensions;
using MsbRpc.Generator.GenerationTree;
using MsbRpc.Generator.Info;
using MsbRpc.Generator.Info.Comparers;
using MsbRpc.Generator.Serialization;
using MsbRpc.Generator.Utility;

namespace MsbRpc.Generator;

[Generator("C#")]
public class ContractGenerator : IIncrementalGenerator
{
    public void Initialize(IncrementalGeneratorInitializationContext context)
    {
        IncrementalValuesProvider<ContractInfo> rpcContracts = context.SyntaxProvider.CreateSyntaxProvider
            (
                GeneratorUtility.GetIsAttributedInterfaceDeclarationSyntax,
                GeneratorUtility.GetContractInfo
            )
            .Where(item => item != null)
            .Select((item, _) => (ContractInfo)item!);

        rpcContracts = rpcContracts
            .Collect()
            .SelectMany((infos, _) => infos.Distinct(TargetComparer.Instance));

        IncrementalValueProvider<ImmutableDictionary<TypeReferenceInfo, CustomSerializationInfo>> customSerializations = context.SyntaxProvider.CreateSyntaxProvider
            (
                GeneratorUtility.GetIsAttributedNonInterfaceTypeDeclarationSyntax,
                GeneratorUtility.GetCustomSerializerInfos
            )
            .SelectMany((infos, _) => infos)
            .Collect()
            .Select((infos, _) => CreateCustomSerializationsDictionary(infos));

        rpcContracts = rpcContracts.Combine(customSerializations).Select(GetContractWithUsedCustomSerializations);

        context.RegisterSourceOutput(rpcContracts, Generate);
    }

    private static ImmutableDictionary<TypeReferenceInfo, CustomSerializationInfo> CreateCustomSerializationsDictionary
        (ImmutableArray<CustomSerializationInfoWithTargetType> infos)
    {
        ImmutableDictionary<TypeReferenceInfo, CustomSerializationInfo>.Builder builder = ImmutableDictionary.CreateBuilder<TypeReferenceInfo, CustomSerializationInfo>();
        foreach (CustomSerializationInfoWithTargetType info in infos)
        {
            builder.Add(info.TargetTypeReference, new CustomSerializationInfo(info));
        }

        return builder.ToImmutable();
    }

    private static ContractInfo GetContractWithUsedCustomSerializations
        ((ContractInfo contract, ImmutableDictionary<TypeReferenceInfo, CustomSerializationInfo> customSerializations) tuple, CancellationToken _)
    {
        ContractInfo contract = tuple.contract;
        ImmutableDictionary<TypeReferenceInfo, CustomSerializationInfo> customSerializations = tuple.customSerializations;
        ImmutableDictionary<TypeReferenceInfo, CustomSerializationInfo>.Builder builder = ImmutableDictionary.CreateBuilder<TypeReferenceInfo, CustomSerializationInfo>();
        foreach (ProcedureInfo procedure in contract.Procedures)
        {
            foreach (ParameterInfo parameter in procedure.Parameters)
            {
                builder.MirrorDistinct(customSerializations, parameter.TypeReference);
            }

            builder.MirrorDistinct(customSerializations, procedure.ResultType);
        }

        ImmutableDictionary<TypeReferenceInfo, CustomSerializationInfo> usedSerializations = builder.ToImmutable();

        return tuple.contract.WithCustomSerializations(usedSerializations);
    }

    private static void Generate(SourceProductionContext context, ContractInfo contractInfo)
    {
        try
        {
            ContractNode contract = new(ref contractInfo);
            ReportDiagnostics(context, contract);
            ProcedureCollectionNode procedures = contract.Procedures;
            context.GenerateFile(new ProcedureEnumFileWriter(procedures));
            context.GenerateFile(new ProcedureEnumExtensionsWriter(procedures));
            context.GenerateFile(EndPointWriter.Get(contract.ServerEndPoint));
            context.GenerateFile(EndPointWriter.Get(contract.ClientEndPoint));
            context.GenerateFile(EndPointConfigurationBuilderWriter.Get(contract.ServerEndPoint));
            context.GenerateFile(EndPointConfigurationBuilderWriter.Get(contract.ClientEndPoint));
        }
        catch (Exception exception)
        {
            context.ReportContractGenerationException(ref contractInfo, exception);
        }
    }

    private static void ReportDiagnostics(SourceProductionContext context, ContractNode contract)
    {
        foreach (ProcedureNode procedure in contract.Procedures)
        {
            ParameterCollectionNode? parameters = procedure.Parameters;
            if (parameters != null)
            {
                foreach (ParameterNode parameter in parameters)
                {
                    ISerialization serialization = parameter.Serialization;
                    if (!serialization.GetCanHandleRpcArguments())
                    {
                        context.ReportTypeIsNotAValidRpcParameter(contract, procedure, parameter);
                    }
                }
            }

            ISerialization resultSerialization = procedure.ResultSerialization;

            if (!resultSerialization.GetCanHandleRpcResults())
            {
                context.ReportTypeIsNotAValidRpcReturnType(contract, procedure);
            }
        }
    }
}