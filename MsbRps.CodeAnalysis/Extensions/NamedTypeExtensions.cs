using Microsoft.CodeAnalysis;

namespace MsbRps.CodeAnalysis.Extensions;

public static class NamedTypeExtensions
{
    public static bool HasName(this INamedTypeSymbol target, string name) => target.ToDisplayString().Equals(name, StringComparison.Ordinal);
}