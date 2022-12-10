﻿//todo: implement

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using MsbRpc.Generator.Info;
using MsbRpc.Generator.Utility;

namespace MsbRpc.Generator;

[Generator]
public class Generator : IIncrementalGenerator
{
    public void Initialize(IncrementalGeneratorInitializationContext context)
    {
        IncrementalValuesProvider<ContractInfo> rpcContractDeclarationSyntaxNodes = context.SyntaxProvider.CreateSyntaxProvider
            (
                GetIsAttributedInterfaceDeclarationSyntax,
                GetContractInfo
            )
            .Where(contract => contract != null)
            .Select((c, _) => c!);

        IncrementalValuesProvider<ContractInfo> rpcContracts = rpcContractDeclarationSyntaxNodes
            .Collect()
            .SelectMany((infos, _) => infos.Distinct(ContractInfo.TargetComparer.Instance));

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

        // check that interfaceSymbol is actually an interface
        if (contractInterface.TypeKind != TypeKind.Interface)
        {
            return null;
        }

        // check that interfaceSymbol has the RpcContract attribute
        return contractInterface.GetAttributes()
            .Select(attributeData => attributeData.AttributeClass!)
            .Any(WellKnownAttributes.IsRpcContractAttribute)
            ? new ContractInfo(contractInterface)
            : null;
    }

    private static void Generate(SourceProductionContext context, ContractInfo contract)
    {
        ContractNames names = contract.CreateNames();
        context.AddSource(names.ServerInterfaceFileName, contract.GenerateServerInterface(names));
        // context.AddSource(nam);
    }
}