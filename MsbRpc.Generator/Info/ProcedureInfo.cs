using System.Collections.Immutable;
using Microsoft.CodeAnalysis;

namespace MsbRpc.Generator.Info;

public readonly struct ProcedureInfo
{
    public string Name { get; }
    public ImmutableArray<ParameterInfo> Parameters { get; }

    public ProcedureInfo(IMethodSymbol method)
    {
        Name = method.Name;
        ImmutableArray<IParameterSymbol> parameters = method.Parameters;
        Parameters = parameters.Select(parameter => new ParameterInfo(parameter)).ToImmutableArray();
    }
}