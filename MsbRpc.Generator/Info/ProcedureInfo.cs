using System.Collections.Immutable;
using Microsoft.CodeAnalysis;

namespace MsbRpc.Generator.Info;

public readonly struct ProcedureInfo : IEquatable<ProcedureInfo>
{
    public string Name { get; }
    public ImmutableArray<ParameterInfo> Parameters { get; }

    public TypeInfo ReturnType { get; }

    public bool InvertsDirection { get; }

    public ProcedureInfo(IMethodSymbol method)
    {
        //todo: use correct direction inversion
        InvertsDirection = false;
        Name = method.Name;
        ImmutableArray<IParameterSymbol> parameters = method.Parameters;
        Parameters = parameters.Select(parameter => new ParameterInfo(parameter)).ToImmutableArray();
        ReturnType = new TypeInfo((INamedTypeSymbol)method.ReturnType);
    }

    public bool Equals(ProcedureInfo other)
        => Name == other.Name
           && Parameters.SequenceEqual(other.Parameters)
           && ReturnType.Equals(other.ReturnType);

    public override bool Equals(object? obj) => obj is ProcedureInfo other && Equals(other);

    public override int GetHashCode()
    {
        unchecked
        {
            return (Name.GetHashCode() * 397) ^ Parameters.GetHashCode() ^ ReturnType.GetHashCode();
        }
    }
}