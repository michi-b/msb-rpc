using System;
using Microsoft.CodeAnalysis;
using MsbRpc.Generator.Extensions;

namespace MsbRpc.Generator.Info;

public readonly struct TypeDeclarationInfo : IEquatable<TypeDeclarationInfo>
{
    /// <summary>
    ///     the fully qualified reference name of the type, without any type arguments
    /// </summary>
    public string Name { get; } = string.Empty;

    public readonly int TypeParameterCount;

    public TypeDeclarationInfo(string fullyQualifiedName, int typeParameterCount)
    {
        Name = fullyQualifiedName;
        TypeParameterCount = typeParameterCount;
    }

    public TypeDeclarationInfo(INamedTypeSymbol typeSymbol)
        : this(typeSymbol.GetFullName(), typeSymbol.TypeParameters.Length) { }

    public bool Equals(TypeDeclarationInfo other)
        => Name == other.Name
           && TypeParameterCount == other.TypeParameterCount;

    public override bool Equals(object? obj) => obj is TypeDeclarationInfo other && Equals(other);

    public override int GetHashCode()
    {
        unchecked
        {
            int hashCode = Name.GetHashCode();
            hashCode = (hashCode * 397) ^ TypeParameterCount.GetHashCode();
            return hashCode;
        }
    }
}