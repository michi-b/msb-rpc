#region

using Microsoft.CodeAnalysis;

#endregion

namespace MsbRpc.Generator.Extensions;

internal static class NamespaceSymbolExtensions
{
    public static bool GetIsSystem(this INamespaceSymbol target) => target.Name == "System" && target.ContainingNamespace.IsGlobalNamespace;

    public static bool GetIsContracts(this INamespaceSymbol target) => target.Name == "Contracts" && target.ContainingNamespace.GetIsMsbRpc();

    public static bool GetIsAttributesNamespace(this INamespaceSymbol target) => target.Name == "Attributes" && target.ContainingNamespace.GetIsGenerator();

    public static bool GetIsSerializerAttributes(this INamespaceSymbol target) => target.Name == "Serialization" && target.ContainingNamespace.GetIsAttributesNamespace();

    public static bool GetIsBuffers(this INamespaceSymbol target) => target.Name == "Buffers" && target.ContainingNamespace.GetIsSerialization();

    private static bool GetIsMsbRpc(this INamespaceSymbol target) => target.Name == "MsbRpc" && target.ContainingNamespace.IsGlobalNamespace;

    private static bool GetIsGenerator(this INamespaceSymbol target) => target.Name == "Generator" && target.ContainingNamespace.GetIsMsbRpc();

    private static bool GetIsSerialization(this INamespaceSymbol target) => target.Name == "Serialization" && target.ContainingNamespace.GetIsMsbRpc();
}