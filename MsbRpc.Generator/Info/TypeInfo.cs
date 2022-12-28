using System;
using Microsoft.CodeAnalysis;

namespace MsbRpc.Generator.Info;

internal readonly struct TypeInfo : IEquatable<TypeInfo>
{
    public string LocalName { get; }
    public string Namespace { get; }

    public TypeInfo(INamedTypeSymbol typeSymbol)
    {
        INamedTypeSymbol originalDefinition = typeSymbol.OriginalDefinition;
        LocalName = originalDefinition.Name;
        Namespace = originalDefinition.ContainingNamespace.ToDisplayString();
    }

    public bool Equals(TypeInfo other)
        => LocalName == other.LocalName
           && Namespace == other.Namespace;

    public override bool Equals(object? obj) => obj is TypeInfo other && Equals(other);

    public override int GetHashCode()
    {
        unchecked
        {
            return (LocalName.GetHashCode() * 397) ^ Namespace.GetHashCode();
        }
    }
}