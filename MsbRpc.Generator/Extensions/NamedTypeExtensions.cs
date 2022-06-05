using Microsoft.CodeAnalysis;

namespace MsbRpc.Generator.Extensions;

public static class NamedTypeExtensions
{
    public static bool HasName(this INamedTypeSymbol target, string name) => target.ToDisplayString().Equals(name, StringComparison.Ordinal);

    public static bool HasBaseTransitive(this INamedTypeSymbol target, INamedTypeSymbol baseType)
    {
        return target.BaseType.IsOrHasBaseTransitive(baseType);
    }

    public static bool IsOrHasBaseTransitive(this INamedTypeSymbol? target, INamedTypeSymbol other)
    {
        INamedTypeSymbol? current = target;
        while (current != null)
        {
            if (current.DefaultEquals(other))
            {
                return true;
            }

            current = current.BaseType;
        }

        return false;
    }

    public static bool DefaultEquals(this INamedTypeSymbol? target, INamedTypeSymbol? other)
    {
        if (target == null || other == null)
        {
            return false;
        }
        return SymbolEqualityComparer.Default.Equals(target, other);
    }
}