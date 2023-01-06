using System;
using System.Collections.Immutable;
using System.Linq;
using System.Threading;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using MsbRpc.Generator.AttributeData;
using MsbRpc.Generator.AttributeDataUtility;
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
        
        return ContractInfo.Parse(contractInterface);
    }

    private static void Generate(SourceProductionContext context, ContractInfo contractInfo)
    {
        try
        {
            ContractNode contract = new(ref contractInfo, context);
            if (contract.IsValid)
            {
                // GenerateEndPoint(context, contract, contract.Server);
                // GenerateEndPoint(context, contract, contract.Client);
            }
            else
            {
                context.ReportContractGenerationError(contract);
            }
        }
        catch (Exception exception)
        {
            context.ReportContractGenerationException(ref contractInfo, exception);
        }
    }

    private static void GenerateEndPoint(SourceProductionContext context, ContractNode contract, EndPoint endPoint)
    {
        //generate inbound procedure enum and extensions and interface
        ProcedureCollection? inboundProcedures = endPoint.InboundProcedures;
        if (inboundProcedures != null)
        {
            // context.GenerateFile(new ProcedureEnumFileWriter(contract, inboundProcedures));
            // context.GenerateFile(new ProcedureEnumExtensionsFileWriter(contract, inboundProcedures));
            // context.GenerateFile(new InterfaceFileWriter(contract, endPoint, inboundProcedures));
        }

        //generate endpoint
        ProcedureCollection? outboundProcedures = endPoint.OutboundProcedures;
        if (inboundProcedures != null || outboundProcedures != null)
        {
            // context.GenerateFile(new EndPointFileWriter(contract, endPoint, inboundProcedures, outboundProcedures));
        }
    }
}