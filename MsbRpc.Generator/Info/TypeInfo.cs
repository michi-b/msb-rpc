using System;
using Microsoft.CodeAnalysis;
using MsbRpc.Generator.Extensions;
using MsbRpc.Generator.Utility;

namespace MsbRpc.Generator.Info;

internal readonly struct TypeInfo : IEquatable<TypeInfo>
{
    public string Name { get; }
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
                    Name = targetTypeDefinition.GetFullName();
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
            Name = typeSymbol.GetFullName();
            IsNullable = typeSymbol.NullableAnnotation != NullableAnnotation.NotAnnotated;
        }
    }

    public bool Equals(TypeInfo other) => Name == other.Name && IsNullable == other.IsNullable;

    public override bool Equals(object? obj) => obj is TypeInfo other && Equals(other);

    public override int GetHashCode()
    {
        unchecked
        {
            int hashCode = Name.GetHashCode();
            hashCode = (hashCode * 397) ^ IsNullable.GetHashCode();
            return hashCode;
        }
    }
}