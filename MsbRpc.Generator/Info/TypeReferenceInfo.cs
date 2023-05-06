﻿using System;
using System.Collections.Immutable;
using System.Linq;
using Microsoft.CodeAnalysis;

namespace MsbRpc.Generator.Info;

public readonly struct TypeReferenceInfo : IEquatable<TypeReferenceInfo>
{
    /// <summary>
    ///     the fully qualified reference name of the type, without any type arguments
    /// </summary>
    public TypeDeclarationInfo Declaration { get; }

    public bool IsNullableReference { get; } = false;

    public ImmutableList<TypeReferenceInfo> TypeArguments { get; } = ImmutableList<TypeReferenceInfo>.Empty;

    private static TypeReferenceInfo Create(ITypeSymbol typeSymbol)
        => typeSymbol is INamedTypeSymbol namedTypeSymbol
            ? new TypeReferenceInfo(namedTypeSymbol)
            : new TypeReferenceInfo();

    private TypeReferenceInfo(TypeDeclarationInfo declaration, bool isNullableReference, ImmutableList<TypeReferenceInfo> typeArguments)
    {
        Declaration = declaration;
        IsNullableReference = isNullableReference;
        TypeArguments = typeArguments;
    }

    //create a simple type info, e.g. int, string, etc.

    /// <remarks>never use this with generic or non-default types</remarks>
    public static TypeReferenceInfo CreateSimple(string fullyQualifiedSimpleTypeName)
        => new
        (
            new TypeDeclarationInfo(fullyQualifiedSimpleTypeName, 0),
            false,
            ImmutableList<TypeReferenceInfo>.Empty
        );

    public TypeReferenceInfo(INamedTypeSymbol typeSymbol)
    {
        var typeArguments = ImmutableList<TypeReferenceInfo>.Empty;

        // ReSharper disable once ForeachCanBeConvertedToQueryUsingAnotherGetEnumerator
        // I find the non-Linq loop more readable
        foreach (ITypeSymbol typeArgument in typeSymbol.TypeArguments)
        {
            typeArguments = typeArguments.Add(Create(typeArgument));
        }

        TypeArguments = typeArguments;
        Declaration = new TypeDeclarationInfo(typeSymbol.OriginalDefinition);
        IsNullableReference = typeSymbol is { IsReferenceType: true, NullableAnnotation: NullableAnnotation.Annotated };
    }

    public bool Equals(TypeReferenceInfo other)
        => Declaration.Equals(other.Declaration)
           && IsNullableReference == other.IsNullableReference
           && TypeArguments.SequenceEqual(other.TypeArguments);

    public override bool Equals(object? obj) => obj is TypeReferenceInfo other && Equals(other);

    public override int GetHashCode()
    {
        unchecked
        {
            int hashCode = Declaration.GetHashCode();
            hashCode = (hashCode * 397) ^ IsNullableReference.GetHashCode();
            hashCode = (hashCode * 397) ^ TypeArguments.GetHashCode();
            return hashCode;
        }
    }

    public string GetDeclarationSyntax()
    {
        string GetNullableAnnotated(string target, bool isNullableReference) => isNullableReference ? target + '?' : target;
        return GetNullableAnnotated
        (
            GetIsGeneric()
                ? Declaration.Name + "<" + string.Join(", ", TypeArguments.Select(x => x.GetDeclarationSyntax())) + ">"
                : Declaration.Name,
            IsNullableReference
        );
    }

    private bool GetIsGeneric() => Declaration.TypeParameterCount > 0;
}