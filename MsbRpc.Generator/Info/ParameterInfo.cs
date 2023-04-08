using System;
using Microsoft.CodeAnalysis;
using MsbRpc.Generator.CodeWriters.Utility;

namespace MsbRpc.Generator.Info;

internal readonly struct ParameterInfo : IEquatable<ParameterInfo>
{
    public string Name { get; }
    public TypeInfo Type { get; }

    public ParameterInfo(IParameterSymbol parameter)
    {
        Name = parameter.Name.PascalToCamelCase();
        Type = new TypeInfo((INamedTypeSymbol)parameter.Type);
    }

    public bool Equals(ParameterInfo other) => Name == other.Name && Type.Equals(other.Type);

    public override bool Equals(object? obj) => obj is ParameterInfo other && Equals(other);

    public override int GetHashCode()
    {
        unchecked
        {
            return (Name.GetHashCode() * 397) ^ Type.GetHashCode();
        }
    }
}