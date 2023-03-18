using System;
using Microsoft.CodeAnalysis;

namespace MsbRpc.Generator.Info;

internal readonly struct TypeInfo : IEquatable<TypeInfo>
{
    public string LocalName { get; }
    public string Namespace { get; }
    public bool IsNullable { get; }

    public TypeInfo(INamedTypeSymbol typeSymbol)
    {
        INamedTypeSymbol originalDefinition = typeSymbol.OriginalDefinition;
        LocalName = originalDefinition.Name;
        Namespace = originalDefinition.ContainingNamespace.ToDisplayString();
        IsNullable = typeSymbol.NullableAnnotation != NullableAnnotation.NotAnnotated;
    }

    public bool Equals(TypeInfo other) => LocalName == other.LocalName && Namespace == other.Namespace && IsNullable == other.IsNullable;

    public override bool Equals(object? obj) => obj is TypeInfo other && Equals(other);

    public override int GetHashCode()
    {
        unchecked
        {
            int hashCode = LocalName.GetHashCode();
            hashCode = (hashCode * 397) ^ Namespace.GetHashCode();
            hashCode = (hashCode * 397) ^ IsNullable.GetHashCode();
            return hashCode;
        }
    }
}