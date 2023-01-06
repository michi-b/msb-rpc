using Microsoft.CodeAnalysis;

namespace MsbRpc.Generator.Utility;

internal static class TypeCheck
{
    public static bool IsRpcContractAttribute(ISymbol attributeClass)
        => attributeClass is
        {
            Name: "RpcContractAttribute",
            ContainingNamespace:
            {
                Name: "Attributes",
                ContainingNamespace:
                {
                    Name: "Generator", ContainingNamespace: { Name: "MsbRpc", ContainingNamespace: { IsGlobalNamespace: true } }
                }
            }
        };

    public static bool IsRpcContractInterface(INamedTypeSymbol interfaceSymbol)
        => interfaceSymbol is
        {
            Name: "IRpcContract",
            ContainingNamespace: { Name: "Contracts", ContainingNamespace: { Name: "MsbRpc", ContainingNamespace: { IsGlobalNamespace: true } } }
        };
}