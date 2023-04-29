using System.Collections.Generic;
using System.Text;
using Microsoft.CodeAnalysis;

namespace MsbRpc.Generator.Extensions;

internal static class NamedTypeSymbolExtensions
{
    private static readonly List<string> ReusableStringList = new(10);
    private static readonly StringBuilder ReusableStringBuilder = new(100);

    public static string GetFullName(this INamedTypeSymbol symbol)
    {
        ReusableStringBuilder.Clear();
        ReusableStringList.Clear();

        INamespaceSymbol? nameSpace = symbol.ContainingNamespace;
        while (nameSpace is not null && !nameSpace.IsGlobalNamespace)
        {
            ReusableStringList.Add(nameSpace.Name);
            nameSpace = nameSpace.ContainingNamespace;
        }

        for (int i = ReusableStringList.Count - 1; i >= 0; i--)
        {
            ReusableStringBuilder.Append(ReusableStringList[i]);
            ReusableStringBuilder.Append('.');
        }

        ReusableStringBuilder.Append(symbol.Name);

        return ReusableStringBuilder.ToString();
    }
}