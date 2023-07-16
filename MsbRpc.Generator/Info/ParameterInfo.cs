#region

using System;
using Microsoft.CodeAnalysis;
using MsbRpc.Generator.Utility;

#endregion

namespace MsbRpc.Generator.Info;

public readonly struct ParameterInfo : IEquatable<ParameterInfo>
{
    public string Name { get; }
    public TypeReferenceInfo TypeReference { get; }

    public ParameterInfo(IParameterSymbol parameter)
    {
        Name = parameter.Name.PascalToCamelCase();
        TypeReference = new TypeReferenceInfo(parameter.Type);
    }

    public bool Equals(ParameterInfo other) => Name == other.Name && TypeReference.Equals(other.TypeReference);

    public override bool Equals(object? obj) => obj is ParameterInfo other && Equals(other);

    public override int GetHashCode()
    {
        unchecked
        {
            return (Name.GetHashCode() * 397) ^ TypeReference.GetHashCode();
        }
    }
}