using Microsoft.CodeAnalysis;

namespace MsbRpc.Generator.Utility;

internal static class TypeCheck
{
    public static bool IsRpcContractAttribute(this ISymbol attributeClass)
        => attributeClass is { Name: "RpcContractAttribute" } && IsAttributesNamespace(attributeClass.ContainingNamespace);

    public static bool IsGenerateServerAttribute(this INamedTypeSymbol attributeClass)
        => attributeClass is { Name: "GenerateServerAttribute" } && IsAttributesNamespace(attributeClass.ContainingNamespace);

    public static bool IsRpcContractInterface(this INamedTypeSymbol interfaceSymbol)
        => interfaceSymbol is
        {
            Name: "IRpcContract",
            ContainingNamespace: { Name: "Contracts", ContainingNamespace: { Name: "MsbRpc", ContainingNamespace: { IsGlobalNamespace: true } } }
        };

    public static bool IsNullable(this INamedTypeSymbol typeSymbol)
        => typeSymbol is { Name: "Nullable", ContainingNamespace: { Name: "System", ContainingNamespace: { IsGlobalNamespace: true } } };

    private static bool IsAttributesNamespace(INamespaceSymbol namespaceSymbol)
        => namespaceSymbol is
        {
            Name: "Attributes", ContainingNamespace: { Name: "Generator", ContainingNamespace: { Name: "MsbRpc", ContainingNamespace: { IsGlobalNamespace: true } } }
        };
}