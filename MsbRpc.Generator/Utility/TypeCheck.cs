﻿using Microsoft.CodeAnalysis;

namespace MsbRpc.Generator.Utility;

internal static class TypeCheck
{
    public static bool IsRpcContractAttribute(ISymbol attributeClass)
        => attributeClass is { Name: "RpcContractAttribute" } && IsAttributesNamespace(attributeClass.ContainingNamespace);

    public static bool IsGenerateServerAttribute(INamedTypeSymbol attributeClass)
        => attributeClass is { Name: "GenerateServerAttribute" } && IsAttributesNamespace(attributeClass.ContainingNamespace);

    public static bool IsRpcContractInterface(INamedTypeSymbol interfaceSymbol)
        => interfaceSymbol is
        {
            Name: "IRpcContract",
            ContainingNamespace: { Name: "Contracts", ContainingNamespace: { Name: "MsbRpc", ContainingNamespace: { IsGlobalNamespace: true } } }
        };

    private static bool IsAttributesNamespace(INamespaceSymbol namespaceSymbol)
        => namespaceSymbol is
        {
            Name: "Attributes", ContainingNamespace: { Name: "Generator", ContainingNamespace: { Name: "MsbRpc", ContainingNamespace: { IsGlobalNamespace: true } } }
        };
}