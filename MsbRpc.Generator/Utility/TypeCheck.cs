using Microsoft.CodeAnalysis;

namespace MsbRpc.Generator.Utility;

internal static class TypeCheck
{
    public static bool IsRpcContractAttribute(this ISymbol attributeClass)
        => attributeClass is { Name: "RpcContractAttribute" } && attributeClass.ContainingNamespace.IsAttributesNamespace();

    public static bool IsConstantSizeSerializerAttribute(this ISymbol attributeClass)
        => attributeClass is { Name: "ConstantSizeSerializerAttribute" } && attributeClass.ContainingNamespace.IsSerializerAttributesNamespace();

    public static bool IsRpcContractInterface(this INamedTypeSymbol interfaceSymbol)
        => interfaceSymbol is
        {
            Name: "IRpcContract",
            ContainingNamespace: { Name: "Contracts", ContainingNamespace: { Name: "MsbRpc", ContainingNamespace: { IsGlobalNamespace: true } } }
        };

    public static bool IsNullable(this INamedTypeSymbol typeSymbol)
        => typeSymbol is { Name: "Nullable", ContainingNamespace: { Name: "System", ContainingNamespace: { IsGlobalNamespace: true } } };

    private static bool IsAttributesNamespace(this INamespaceSymbol namespaceSymbol)
        => namespaceSymbol is
        {
            Name: "Attributes", ContainingNamespace: { Name: "Generator", ContainingNamespace: { Name: "MsbRpc", ContainingNamespace: { IsGlobalNamespace: true } } }
        };

    private static bool IsSerializerAttributesNamespace(this INamespaceSymbol namespaceSymbol)
        => namespaceSymbol is { Name: "Serialization" } && namespaceSymbol.ContainingNamespace.IsAttributesNamespace();
}