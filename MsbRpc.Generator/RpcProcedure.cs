using System.Collections.Immutable;
using Microsoft.CodeAnalysis;

namespace MsbRpc.Generator;

public readonly record struct RpcProcedure
{
    public string Name { get; }
    public ImmutableArray<RpcParameter> Parameters { get; }

    public RpcProcedure(IMethodSymbol method)
    {
        Name = method.Name;
        ImmutableArray<IParameterSymbol> parameters = method.Parameters;
        Parameters = parameters.Select(parameter => new RpcParameter(parameter)).ToImmutableArray();
    }
}