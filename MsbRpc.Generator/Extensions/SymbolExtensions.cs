#region

using System;
using Microsoft.CodeAnalysis;

#endregion

namespace MsbRpc.Generator.Extensions;

internal static class SymbolExtensions
{
    public static bool HasAttribute(this ISymbol symbol, Predicate<INamedTypeSymbol> predicate)
    {
        foreach (AttributeData? attribute in symbol.GetAttributes())
        {
            if (attribute is { AttributeClass: { } namedAttributeTypeSymbol })
            {
                if (predicate(namedAttributeTypeSymbol))
                {
                    return true;
                }
            }
        }

        return false;
    }

    public static bool GetIsGlobalNamespace(this ISymbol target) => target is INamespaceSymbol { IsGlobalNamespace: true };
}