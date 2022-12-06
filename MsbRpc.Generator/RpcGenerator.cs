//todo: implement

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace MsbRpc.Generator;

[Generator]
public class RpcGenerator : IIncrementalGenerator
{
    public void Initialize(IncrementalGeneratorInitializationContext context)
    {
        IncrementalValuesProvider<RpcContract> rpcContracts = context.SyntaxProvider.CreateSyntaxProvider
            (
                GetIsAttributedInterfaceDeclarationSyntax,
                GetRpcContract
            )
            .Where(contract => contract != null)
            .Select((contract, _) => (RpcContract)contract!);

        context.RegisterSourceOutput(rpcContracts, (sourceProductionContext, contract) => contract.Generate(sourceProductionContext));
    }

    private static bool GetIsAttributedInterfaceDeclarationSyntax(SyntaxNode syntaxNode, CancellationToken cancellationToken)
    {
        var interfaceDeclarationSyntax = syntaxNode as InterfaceDeclarationSyntax;
        return interfaceDeclarationSyntax != null && interfaceDeclarationSyntax.AttributeLists.Any();
    }

    private static RpcContract? GetRpcContract(GeneratorSyntaxContext context, CancellationToken cancellationToken)
    {
        SemanticModel semanticModel = context.SemanticModel;

        // get interface symbol
        ISymbol? declaredSymbol = semanticModel.GetDeclaredSymbol(context.Node, cancellationToken);

        // check that interface is an INamedTypeSymbol
        if (declaredSymbol is not INamedTypeSymbol interfaceSymbol)
        {
            return null;
        }

        // check that interfaceSymbol is actually an interface
        if (interfaceSymbol.TypeKind != TypeKind.Interface)
        {
            return null;
        }

        // check that interfaceSymbol has the RpcContract attribute
        // ReSharper disable twice ForeachCanBeConvertedToQueryUsingAnotherGetEnumerator
        // converting to LINQ requires boxing
        foreach (AttributeListSyntax attributeList in (context.Node as InterfaceDeclarationSyntax)!.AttributeLists)
        {
            foreach (AttributeSyntax attributeSyntax in attributeList.Attributes)
            {
                var attributeConstructor = (semanticModel.GetSymbolInfo(attributeSyntax).Symbol as IMethodSymbol)!;
                INamedTypeSymbol attribute = attributeConstructor.ContainingType;
                if (attribute.ToDisplayString() == "MsbRpc.Generator.Attributes.RpcContractAttribute")
                {
                    return new RpcContract(interfaceSymbol);
                }
            }
        }

        return null;
    }
}