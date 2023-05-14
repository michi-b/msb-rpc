using System;
using Microsoft.CodeAnalysis;
using MsbRpc.Generator.Extensions;
using MsbRpc.Generator.Serialization.Default;

namespace MsbRpc.Generator.Info;

public readonly struct TypeDeclarationInfo : IEquatable<TypeDeclarationInfo>
{
    /// <summary>
    ///     the fully qualified reference name of the type, without any type arguments
    /// </summary>
    public string Name { get; } = string.Empty;

    public readonly int TypeParameterCount;

    public EnumSerializationKind? EnumSerializationKind { get; } = null;

    public TypeDeclarationInfo
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

    public TypeDeclarationInfo(INamedTypeSymbol typeSymbol)
        : this
        (
            typeSymbol.GetFullName(),
            typeSymbol.TypeParameters.Length,
            typeSymbol.EnumUnderlyingType is { } enumUnderlyingType
                ? enumUnderlyingType.GetFullName() switch
                {
                    "System.Byte" => Serialization.Default.EnumSerializationKind.Byte,
                    "System.SByte" => Serialization.Default.EnumSerializationKind.Sbyte,
                    "System.Int32" => Serialization.Default.EnumSerializationKind.Int,
                    "System.Int64" => Serialization.Default.EnumSerializationKind.Long,
                    "System.Int16" => Serialization.Default.EnumSerializationKind.Short,
                    "System.UInt32" => Serialization.Default.EnumSerializationKind.Uint,
                    "System.UInt64" => Serialization.Default.EnumSerializationKind.Ulong,
                    "System.UInt16" => Serialization.Default.EnumSerializationKind.Ushort,
                    _ => throw new ArgumentOutOfRangeException()
                }
                : null
        ) { }

    public bool Equals(TypeDeclarationInfo other)
        => Name == other.Name
           && TypeParameterCount == other.TypeParameterCount
           && Equals(EnumSerializationKind, other.EnumSerializationKind);

    public override bool Equals(object? obj) => obj is TypeDeclarationInfo other && Equals(other);

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