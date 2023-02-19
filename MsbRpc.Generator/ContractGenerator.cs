using System;
using System.Linq;
using System.Threading;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
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
        IncrementalValuesProvider<ContractInfo> rpcContractDeclarationSyntaxNodes = context.SyntaxProvider.CreateSyntaxProvider
            (
                GetIsAttributedInterfaceDeclarationSyntax,
                GetContractInfo
            )
            .Where(contract => contract != null)
            .Select((contractInfo, _) => (ContractInfo)contractInfo!);

        IncrementalValuesProvider<ContractInfo> rpcContracts = rpcContractDeclarationSyntaxNodes
            .Collect()
            .SelectMany((infos, _) => infos.Distinct(TargetComparer.Instance));

        context.RegisterSourceOutput(rpcContracts, Generate);
    }

    private static bool GetIsAttributedInterfaceDeclarationSyntax(SyntaxNode syntaxNode, CancellationToken cancellationToken)
    {
        var interfaceDeclarationSyntax = syntaxNode as InterfaceDeclarationSyntax;
        return interfaceDeclarationSyntax != null && interfaceDeclarationSyntax.AttributeLists.Any();
    }

    private static ContractInfo? GetContractInfo(GeneratorSyntaxContext context, CancellationToken cancellationToken)
    {
        SemanticModel semanticModel = context.SemanticModel;

        // get interface symbol
        if (semanticModel.GetDeclaredSymbol(context.Node, cancellationToken) is not INamedTypeSymbol contractInterface)
        {
            return null;
        }

        // check that interfaceSymbol is actually an interface and derives from IRpcContract
        if (contractInterface.TypeKind != TypeKind.Interface
            || !contractInterface.Interfaces.Any(TypeCheck.IsRpcContractInterface))
        {
            return null;
        }

        return ContractInfoParser.Parse(contractInterface);
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
            if (contract.Server != null)
            {
                context.GenerateFile(new ServerWriter(contract.Server));
            }
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