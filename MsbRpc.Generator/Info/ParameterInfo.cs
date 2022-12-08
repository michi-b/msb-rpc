using Microsoft.CodeAnalysis;

namespace MsbRpc.Generator.Info;

public readonly struct ParameterInfo
{
    public string Name { get; }
    public string Type { get; }

    public ParameterInfo(IParameterSymbol parameter)
    {
        Name = parameter.Name;
        Type = parameter.Type.OriginalDefinition.Name;
    }
}