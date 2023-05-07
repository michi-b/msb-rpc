using System;
using System.Collections.Immutable;
using System.Linq;
using Microsoft.CodeAnalysis;
using MsbRpc.Generator.Utility;

namespace MsbRpc.Generator.Info;

internal readonly struct ProcedureInfo : IEquatable<ProcedureInfo>
{
    public string Name { get; }
    public ImmutableArray<ParameterInfo> Parameters { get; }

    public TypeReferenceInfo ResultType { get; }

    public ProcedureInfo(IMethodSymbol method)
    {
        Name = method.Name.CamelToPascalCase();
        ImmutableArray<IParameterSymbol> parameters = method.Parameters;
        Parameters = parameters.Select(parameter => new ParameterInfo(parameter)).ToImmutableArray();
        ResultType = new TypeReferenceInfo((INamedTypeSymbol)method.ReturnType);
    }

    public bool Equals(ProcedureInfo other)
        => Name == other.Name
           && Parameters.SequenceEqual(other.Parameters)
           && ResultType.Equals(other.ResultType);

    public override bool Equals(object? obj) => obj is ProcedureInfo other && Equals(other);

    public override int GetHashCode()
    {
        unchecked
        {
            return (Name.GetHashCode() * 397) ^ Parameters.GetHashCode() ^ ResultType.GetHashCode();
        }
    }
}