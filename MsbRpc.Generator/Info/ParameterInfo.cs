using System.CodeDom.Compiler;
using Microsoft.CodeAnalysis;

namespace MsbRpc.Generator.Info;

public readonly struct ParameterInfo : IEquatable<ParameterInfo>
{
    private string Name { get; }
    private TypeInfo Type { get; }

    public ParameterInfo(IParameterSymbol parameter)
    {
        Name = parameter.Name;
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

    public void GenerateInterface(IndentedTextWriter writer)
    {
        writer.Write(Type.FullName);
        writer.Write(" ");
        writer.Write(Name);
    }
}