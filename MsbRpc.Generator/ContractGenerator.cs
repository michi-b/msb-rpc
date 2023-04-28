using System;
using System.Linq;
using Microsoft.CodeAnalysis;
using MsbRpc.Generator.CodeWriters.Files;
using MsbRpc.Generator.Extensions;
using MsbRpc.Generator.GenerationTree;
using MsbRpc.Generator.Info;
using MsbRpc.Generator.Info.Comparers;
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

        IncrementalValuesProvider<ConstantSizeSerializerInfo> constantSizeSerializers = context.SyntaxProvider.CreateSyntaxProvider
            (
                GeneratorUtility.GetIsAttributedNonInterfaceTypeDeclarationSyntax,
                GeneratorUtility.GetConstantSizeSerializerInfo
            )
            .Where(item => item != null)
            .Select((item, _) => (ConstantSizeSerializerInfo)item!);

        context.RegisterSourceOutput(rpcContracts, Generate);
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
                    TypeNode parameterType = parameter.Type;
                    if (!parameterType.IsValidParameter)
                    {
                        context.ReportTypeIsNotAValidRpcParameter(contract, procedure, parameter);
                    }
                }
            }

            TypeNode returnType = procedure.ReturnType;
            if (!returnType.IsValidReturnType)
            {
                context.ReportTypeIsNotAValidRpcReturnType(contract, procedure);
            }
        }
    }
}