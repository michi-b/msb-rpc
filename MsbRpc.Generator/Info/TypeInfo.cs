using Microsoft.CodeAnalysis;

namespace MsbRpc.Generator.Info;

public readonly struct TypeInfo : IEquatable<TypeInfo>
{
    public string Name { get; }
    public string Namespace { get; }

    public TypeInfo(INamedTypeSymbol typeSymbol)
    {
        INamedTypeSymbol originalDefinition = typeSymbol.OriginalDefinition;
        Name = originalDefinition.Name;
        Namespace = originalDefinition.ContainingNamespace.ToDisplayString();
    }

    public bool Equals(TypeInfo other)
        => Name == other.Name
           && Namespace == other.Namespace;

    public override bool Equals(object? obj) => obj is TypeInfo other && Equals(other);

    public override int GetHashCode()
    {
        unchecked
        {
            return (Name.GetHashCode() * 397) ^ Namespace.GetHashCode();
        }
    }
}