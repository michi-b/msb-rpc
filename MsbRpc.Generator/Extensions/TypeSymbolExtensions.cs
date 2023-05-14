using System.Text;
using Microsoft.CodeAnalysis;

namespace MsbRpc.Generator.Extensions;

public static class TypeSymbolExtensions
{
    private const int DefaultStringBuilderCapacity = 100;

    /// <summary>internal method for symbol referencing name computation</summary>
    /// <returns>the fully qualified reference name of the symbol</returns>
    /// <remarks>
    ///     this is different from ISymbol.ToDisplayString(SymbolDisplayFormat.FullyQualifiedFormat)
    ///     in that it results e.g. in "System.Int32" instead of just "int" for C# type keywords
    /// </remarks>
    /// <seealso cref="ISymbol.ToDisplayString" />
    public static string GetFullName(this ITypeSymbol symbol)
    {
        StringBuilder stringBuilder = new(DefaultStringBuilderCapacity);

        ISymbol? containingSymbol = symbol.ContainingSymbol;
        while (containingSymbol is not null && !containingSymbol.GetIsGlobalNamespace())
        {
            stringBuilder.Insert(0, '.');
            stringBuilder.Insert(0, containingSymbol.Name);
            containingSymbol = containingSymbol.ContainingSymbol;
        }

        stringBuilder.Append(symbol.Name);

        return stringBuilder.ToString();
    }
}