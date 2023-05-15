using System;
using Microsoft.CodeAnalysis;

namespace MsbRpc.Generator.Info;

public class ArrayDeclarationInfo : IEquatable<ArrayDeclarationInfo>
{
    public TypeReferenceInfo ElementType { get; }
    public int Rank { get; }

    public ArrayDeclarationInfo(IArrayTypeSymbol arrayTypeSymbol)
    {
        Rank = arrayTypeSymbol.Rank;
        ElementType = new TypeReferenceInfo(arrayTypeSymbol.ElementType);
    }

    public string GetDeclarationSyntax()
    {
        //depending on rank, e.g. [], [,], [,,] etc.
        string arrayIdentifier = $"[{new string(',', Rank - 1)}]";

        return $"{ElementType.GetDeclarationSyntax()}{arrayIdentifier}";
    }

    public bool Equals(ArrayDeclarationInfo? other)
    {
        if (ReferenceEquals(null, other))
        {
            return false;
        }

        if (ReferenceEquals(this, other))
        {
            return true;
        }

        return ElementType.Equals(other.ElementType) && Rank == other.Rank;
    }

    public override bool Equals(object? other)
    {
        if (ReferenceEquals(null, other))
        {
            return false;
        }

        if (ReferenceEquals(this, other))
        {
            return true;
        }

        return other.GetType() == GetType() && Equals((ArrayDeclarationInfo)other);
    }

    public override int GetHashCode()
    {
        unchecked
        {
            return (ElementType.GetHashCode() * 397) ^ Rank;
        }
    }
}