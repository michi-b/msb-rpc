using System;
using Microsoft.CodeAnalysis;
using MsbRpc.Generator.Extensions;
using MsbRpc.Generator.Serialization.Default;

namespace MsbRpc.Generator.Info;

public readonly struct NamedTypeDeclarationInfo : IEquatable<NamedTypeDeclarationInfo>
{
    /// <summary>
    ///     the fully qualified reference name of the type, without any type arguments
    /// </summary>
    public string Name { get; } = string.Empty;

    public readonly int TypeParameterCount;

    public EnumSerializationKind? EnumSerializationKind { get; } = null;

    public NamedTypeDeclarationInfo
    (
        string fullyQualifiedName,
        int typeParameterCount = 0,
        EnumSerializationKind? enumSerializationKindKind = null
    )
    {
        Name = fullyQualifiedName;
        TypeParameterCount = typeParameterCount;
        EnumSerializationKind = enumSerializationKindKind;
    }

    public NamedTypeDeclarationInfo(INamedTypeSymbol namedTypeSymbol)
        : this
        (
            namedTypeSymbol.GetFullName(),
            namedTypeSymbol.TypeParameters.Length,
            namedTypeSymbol.EnumUnderlyingType is { } enumUnderlyingType ? EnumSerializationKindUtility.Get(enumUnderlyingType.GetFullName()) : null
        ) { }

    public bool Equals(NamedTypeDeclarationInfo other)
        => Name == other.Name
           && TypeParameterCount == other.TypeParameterCount
           && Equals(EnumSerializationKind, other.EnumSerializationKind);

    public override bool Equals(object? obj) => obj is NamedTypeDeclarationInfo other && Equals(other);

    public override int GetHashCode()
    {
        unchecked
        {
            int hashCode = Name.GetHashCode();
            hashCode = (hashCode * 397) ^ TypeParameterCount.GetHashCode();
            hashCode = (hashCode * 397) ^ (EnumSerializationKind != null ? EnumSerializationKind.GetHashCode() : 0);
            return hashCode;
        }
    }
}