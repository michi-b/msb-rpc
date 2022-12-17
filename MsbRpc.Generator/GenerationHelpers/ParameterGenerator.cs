using MsbRpc.Generator.Info;

namespace MsbRpc.Generator.GenerationHelpers;

public readonly struct ParameterGenerator
{
    public readonly string Code { get; }
    
    public ParameterGenerator(ParameterInfo info)
    {
        Code = $"{info.Type.FullName} {info.Name}";
    }
}