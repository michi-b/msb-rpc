using System;
using System.Collections.Immutable;
using System.Diagnostics;
using System.Linq;
using Microsoft.CodeAnalysis;

namespace MsbRpc.Generator.Info;

public readonly struct TypeReferenceInfo : IEquatable<TypeReferenceInfo>
{
    /// <summary>
    ///     the named type declaration, without any type arguments, if it is a named type and not e.g. an array type
    /// </summary>
    /// <remarks>exactly one of this and <see cref="ArrayDeclaration" /> must be noll</remarks>
    public NamedTypeDeclarationInfo? NamedDeclaration { get; }

    /// <summary>
    ///     the array declaration, if it is an array type and not e.g. a named type
    /// </summary>
    /// <remarks>exactly one of this and <see cref="NamedDeclaration" /> must be noll</remarks>
    public ArrayDeclarationInfo? ArrayDeclaration { get; }

    public bool IsNullableReference { get; } = false;

    public ImmutableList<TypeReferenceInfo> TypeArguments { get; } = ImmutableList<TypeReferenceInfo>.Empty;

    public TypeReferenceInfo(NamedTypeDeclarationInfo namedDeclaration, bool isNullableReference = false)
        : this(namedDeclaration, null, ImmutableList<TypeReferenceInfo>.Empty, isNullableReference) { }

    public TypeReferenceInfo
    (
        NamedTypeDeclarationInfo namedDeclaration,
        ImmutableList<TypeReferenceInfo> typeArguments,
        bool isNullableReference = false
    )
        : this(namedDeclaration, null, typeArguments, isNullableReference) { }

    private TypeReferenceInfo
    (
        NamedTypeDeclarationInfo? namedDeclaration,
        ArrayDeclarationInfo? arrayDeclaration,
        ImmutableList<TypeReferenceInfo> typeArguments,
        bool isNullableReference = false
    )
    {
        Debug.Assert(namedDeclaration is not null || arrayDeclaration is not null);
        NamedDeclaration = namedDeclaration;
        ArrayDeclaration = arrayDeclaration;
        IsNullableReference = isNullableReference;
        TypeArguments = typeArguments;
    }

    /// <summary>
    ///     create a simple type info, e.g. int, string, etc.;
    ///     not to be used with generic and/or non-default types, or arrays
    /// </summary>
    public static TypeReferenceInfo CreateSimple(string fullyQualifiedSimpleTypeName, bool isNullableReference = false)
        => new
        (
            new NamedTypeDeclarationInfo(fullyQualifiedSimpleTypeName),
            null,
            ImmutableList<TypeReferenceInfo>.Empty,
            isNullableReference
        );

    public TypeReferenceInfo(ITypeSymbol typeSymbol)
    {
        IsNullableReference = typeSymbol is { IsReferenceType: true, NullableAnnotation: NullableAnnotation.Annotated };

        TypeArguments = ImmutableList<TypeReferenceInfo>.Empty;

        switch (typeSymbol)
        {
            case INamedTypeSymbol namedTypeSymbol:
                ArrayDeclaration = null;
                NamedDeclaration = new NamedTypeDeclarationInfo(namedTypeSymbol);
                TypeArguments = TypeArguments.AddRange(namedTypeSymbol.TypeArguments.Select(ta => new TypeReferenceInfo(ta)));
                break;
            case IArrayTypeSymbol arrayTypeSymbol:
                NamedDeclaration = null;
                ArrayDeclaration = new ArrayDeclarationInfo(arrayTypeSymbol);
                break;
        }
    }

    public bool Equals(TypeReferenceInfo other)
        => NamedDeclaration.Equals(other.NamedDeclaration)
           && IsNullableReference == other.IsNullableReference
           && TypeArguments.SequenceEqual(other.TypeArguments)
           && ArrayDeclaration != null
            ? ArrayDeclaration.Equals(other.ArrayDeclaration)
            : other.ArrayDeclaration == null;

    public override bool Equals(object? obj) => obj is TypeReferenceInfo other && Equals(other);

    public override int GetHashCode()
    {
        unchecked
        {
            int hashCode = NamedDeclaration.GetHashCode();
            hashCode = (hashCode * 397) ^ IsNullableReference.GetHashCode();
            hashCode = (hashCode * 397) ^ TypeArguments.GetHashCode();
            hashCode = (hashCode * 397) ^ (ArrayDeclaration?.GetHashCode() ?? 0);
            return hashCode;
        }
    }

    public string GetDeclarationSyntax()
    {
        string declarationSyntax;

        if (NamedDeclaration != null)
        {
            declarationSyntax = GetIsGeneric()
                ? NamedDeclaration.Value.Name + "<" + string.Join(", ", TypeArguments.Select(x => x.GetDeclarationSyntax())) + ">"
                : NamedDeclaration.Value.Name;
        }
        else if (ArrayDeclaration != null)
        {
            declarationSyntax = ArrayDeclaration.GetDeclarationSyntax();
        }
        else
        {
            throw new InvalidOperationException("TypeReferenceInfo is neither a named type nor an array type");
        }

        return IsNullableReference ? declarationSyntax + '?' : declarationSyntax;
    }

    private bool GetIsGeneric() => NamedDeclaration is { TypeParameterCount: > 0 };

    public TypeReferenceInfo MakeNullable(bool nullable) => new(NamedDeclaration, ArrayDeclaration, TypeArguments, nullable);
}