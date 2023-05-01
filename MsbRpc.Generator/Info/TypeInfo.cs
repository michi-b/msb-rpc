using System;
using System.Collections.Immutable;
using System.Diagnostics;
using System.Linq;
using Microsoft.CodeAnalysis;
using MsbRpc.Generator.Extensions;

namespace MsbRpc.Generator.Info;

public readonly struct TypeInfo : IEquatable<TypeInfo>
{
    /// <summary>
    ///     the fully qualified reference name of the type, without any type arguments
    /// </summary>
    public string Name { get; }

    public bool IsNullableReferenceType { get; }

    public ImmutableList<TypeInfo> TypeArguments { get; }

    private static TypeInfo CreateTypeInfo(ITypeSymbol typeSymbol)
        => typeSymbol is INamedTypeSymbol namedTypeSymbol
            ? new TypeInfo(namedTypeSymbol)
            : new TypeInfo(typeSymbol);

    /// <summary>
    ///     ONLY to be used for non-named types
    /// </summary>
    private TypeInfo(ITypeSymbol typeSymbol)
    {
        Debug.Assert(typeSymbol is not INamedTypeSymbol);
        TypeArguments = ImmutableList<TypeInfo>.Empty;
        Name = string.Empty;
        IsNullableReferenceType = false;
    }

    public TypeInfo(INamedTypeSymbol typeSymbol)
    {
        INamedTypeSymbol originalDefinition = typeSymbol.OriginalDefinition;

        var typeArguments = ImmutableList<TypeInfo>.Empty;

        // ReSharper disable once ForeachCanBeConvertedToQueryUsingAnotherGetEnumerator
        // I find the non-Linq loop more readable
        foreach (ITypeSymbol typeArgument in originalDefinition.TypeArguments)
        {
            typeArguments = typeArguments.Add(CreateTypeInfo(typeArgument));
        }

        TypeArguments = typeArguments;
        Name = typeSymbol.GetFullName();
        IsNullableReferenceType = typeSymbol is { IsReferenceType: true, NullableAnnotation: NullableAnnotation.Annotated };
    }

    public bool Equals(TypeInfo other)
        => Name == other.Name
           && IsNullableReferenceType == other.IsNullableReferenceType
           && TypeArguments.SequenceEqual(other.TypeArguments);

    public override bool Equals(object? obj) => obj is TypeInfo other && Equals(other);

    public override int GetHashCode()
    {
        unchecked
        {
            int hashCode = Name.GetHashCode();
            hashCode = (hashCode * 397) ^ IsNullableReferenceType.GetHashCode();
            hashCode = (hashCode * 397) ^ TypeArguments.GetHashCode();
            return hashCode;
        }
    }
}