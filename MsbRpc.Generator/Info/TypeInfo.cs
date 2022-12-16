using Microsoft.CodeAnalysis;

namespace MsbRpc.Generator.Info;

public readonly struct TypeInfo : IEquatable<TypeInfo>
{
    private string Name { get; }
    private string Namespace { get; }
    public string FullName { get; }
    public SerializationInfo SerializationInfo { get; }

    public TypeInfo(INamedTypeSymbol typeSymbol)
    {
        INamedTypeSymbol originalDefinition = typeSymbol.OriginalDefinition;
        Name = originalDefinition.Name;
        Namespace = originalDefinition.ContainingNamespace.ToDisplayString();
        FullName = $"{Namespace}.{Name}";
        SerializationInfo = new SerializationInfo(FullName);
    }

    public bool Equals(TypeInfo other) => Name == other.Name && Namespace == other.Namespace;

    public override bool Equals(object? obj) => obj is TypeInfo other && Equals(other);

    public override int GetHashCode()
    {
        unchecked
        {
            return (Name.GetHashCode() * 397) ^ Namespace.GetHashCode();
        }
    }
}