using System;
using Microsoft.CodeAnalysis;
using MsbRpc.Generator.Utility;

namespace MsbRpc.Generator.Info;

internal readonly struct TypeInfo : IEquatable<TypeInfo>
{
    public string LocalName { get; }
    public string Namespace { get; }
    public bool IsNullable { get; }

    public TypeInfo(INamedTypeSymbol typeSymbol)
    {
        INamedTypeSymbol originalDefinition = typeSymbol.OriginalDefinition;

        if (originalDefinition.IsGenericType)
        {
            if (typeSymbol.OriginalDefinition.IsNullable())
            {
                ITypeSymbol innerType = typeSymbol.TypeArguments[0];
                if (innerType is INamedTypeSymbol targetType)
                {
                    INamedTypeSymbol targetTypeDefinition = targetType.OriginalDefinition;
                    LocalName = targetTypeDefinition.Name;
                    Namespace = targetTypeDefinition.ContainingNamespace.ToDisplayString();
                    IsNullable = true;
                }
                else
                {
                    throw new NotSupportedException("Nullable types without named inner type are not supported");
                }
            }
            else
            {
                throw new NotSupportedException("Generic types except nullable are not supported");
            }
        }
        else
        {
            LocalName = originalDefinition.Name;
            Namespace = originalDefinition.ContainingNamespace.ToDisplayString();
            IsNullable = typeSymbol.NullableAnnotation != NullableAnnotation.NotAnnotated;
        }
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