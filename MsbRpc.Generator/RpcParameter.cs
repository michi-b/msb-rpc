using Microsoft.CodeAnalysis;

namespace MsbRpc.Generator;

public readonly record struct RpcParameter
{
    public string Name { get; }
    public string Type { get; }

    public RpcParameter(IParameterSymbol parameter)
    {
        Name = parameter.Name;
        Type = parameter.Type.OriginalDefinition.Name;
    }
}