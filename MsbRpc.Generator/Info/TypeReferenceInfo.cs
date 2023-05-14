using System;
using System.Collections.Immutable;
using System.Linq;
using Microsoft.CodeAnalysis;

namespace MsbRpc.Generator.Info;

public readonly struct TypeReferenceInfo : IEquatable<TypeReferenceInfo>
{
    /// <summary>
    ///     the fully qualified reference name of the type, without any type arguments;
    ///     for arrays, this is the element type
    /// </summary>
    public NamedTypeDeclarationInfo? NamedDeclaration { get; }

    public bool IsNullableReference { get; } = false;

    public ImmutableList<TypeReferenceInfo> TypeArguments { get; } = ImmutableList<TypeReferenceInfo>.Empty;

    private static TypeReferenceInfo Create(ITypeSymbol typeSymbol)
        => typeSymbol is INamedTypeSymbol namedTypeSymbol
            ? new TypeReferenceInfo(namedTypeSymbol)
            : new TypeReferenceInfo();

    public TypeReferenceInfo(NamedTypeDeclarationInfo namedDeclaration, bool isNullableReference = false)
        : this(namedDeclaration, ImmutableList<TypeReferenceInfo>.Empty, isNullableReference) { }

    public TypeReferenceInfo
    (
        NamedTypeDeclarationInfo? namedDeclaration,
        ImmutableList<TypeReferenceInfo> typeArguments,
        bool isNullableReference = false
    )
    {
        NamedDeclaration = namedDeclaration;
        IsNullableReference = isNullableReference;
        TypeArguments = typeArguments;
    }

    /// <summary>
    ///     create a simple type info, e.g. int, string, etc.;
    ///     not to be used with generic and/or non-default types
    /// </summary>
    public static TypeReferenceInfo CreateSimple(string fullyQualifiedSimpleTypeName, bool isNullableReference = false)
        => new
        (
            new NamedTypeDeclarationInfo(fullyQualifiedSimpleTypeName),
            ImmutableList<TypeReferenceInfo>.Empty,
            isNullableReference
        );

    public TypeReferenceInfo(ITypeSymbol typeSymbol)
    {
        IsNullableReference = typeSymbol is { IsReferenceType: true, NullableAnnotation: NullableAnnotation.Annotated };

        TypeArguments = ImmutableList<TypeReferenceInfo>.Empty;

        if (typeSymbol is INamedTypeSymbol namedTypeSymbol)
        {
            NamedDeclaration = new NamedTypeDeclarationInfo(namedTypeSymbol);
            TypeArguments = TypeArguments.AddRange(namedTypeSymbol.TypeArguments.Select(Create));
        }
    }

    public bool Equals(TypeReferenceInfo other)
        => NamedDeclaration.Equals(other.NamedDeclaration)
           && IsNullableReference == other.IsNullableReference
           && TypeArguments.SequenceEqual(other.TypeArguments);

    public override bool Equals(object? obj) => obj is TypeReferenceInfo other && Equals(other);

    public override int GetHashCode()
    {
        unchecked
        {
            int hashCode = NamedDeclaration.GetHashCode();
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
            NamedDeclaration != null
                ? GetIsGeneric()
                    ? NamedDeclaration.Value.Name + "<" + string.Join(", ", TypeArguments.Select(x => x.GetDeclarationSyntax())) + ">"
                    : NamedDeclaration.Value.Name
                //todo: implement non-name-declared type declaration syntax
                : throw new ArgumentOutOfRangeException(),
            IsNullableReference
        );
    }

    private bool GetIsGeneric() => NamedDeclaration is { TypeParameterCount: > 0 };

    public TypeReferenceInfo MakeNullable(bool nullable) => new(NamedDeclaration, TypeArguments, nullable);
}