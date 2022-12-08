using Microsoft.CodeAnalysis;

namespace MsbRpc.Generator.Info;

public readonly struct ParameterInfo : IEquatable<ParameterInfo>
{
    public string Name { get; }
    public string Type { get; }

    public ParameterInfo(IParameterSymbol parameter)
    {
        Name = parameter.Name;
        Type = parameter.Type.OriginalDefinition.Name;
    }

    public bool Equals(ParameterInfo other) => Name == other.Name && Type == other.Type;

    public override bool Equals(object? obj) => obj is ParameterInfo other && Equals(other);

    public override int GetHashCode()
    {
        unchecked
        {
            return (Name.GetHashCode() * 397) ^ Type.GetHashCode();
        }
    }
}