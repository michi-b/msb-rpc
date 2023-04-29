using Microsoft.CodeAnalysis;

namespace MsbRpc.Generator.Extensions;

internal static class NamedTypeSymbolExtensions
{
    public static string GetFullName(this INamedTypeSymbol symbol)
    {
        string name = symbol.Name;
        INamespaceSymbol? containingNamespace = symbol.ContainingNamespace;
        while (containingNamespace is not null && !containingNamespace.IsGlobalNamespace)
        {
            name = $"{containingNamespace.Name}.{name}";
            containingNamespace = containingNamespace.ContainingNamespace;
        }

        return name;
    }
}