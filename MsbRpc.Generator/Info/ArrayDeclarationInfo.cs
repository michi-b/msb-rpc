#region

using System;
using System.Diagnostics;
using Microsoft.CodeAnalysis;

#endregion

namespace MsbRpc.Generator.Info;

[DebuggerDisplay("{DebuggerDisplay,nq}")]
public class ArrayDeclarationInfo : IEquatable<ArrayDeclarationInfo>
{
    public TypeReferenceInfo ElementType { get; }
    public int Rank { get; }
    public string DebuggerDisplay => ElementType.DebuggerDisplay + $"[{new string(',', Rank - 1)}]";

    public ArrayDeclarationInfo(IArrayTypeSymbol arrayTypeSymbol)
        : this(new TypeReferenceInfo(arrayTypeSymbol.ElementType), arrayTypeSymbol.Rank) { }

    public ArrayDeclarationInfo(TypeReferenceInfo elementType, int rank)
    {
        ElementType = elementType;
        Rank = rank;
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