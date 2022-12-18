using MsbRpc.Generator.Info;

namespace MsbRpc.Generator.GenerationHelpers;

public readonly struct ParameterGenerator
{
    public readonly string ParameterCode { get; }
    private readonly TypeGenerator Type { get; }
    private readonly string Name { get; }

    public ParameterGenerator(ParameterInfo info)
    {
        Name = info.Name;
        Type = new TypeGenerator(info.Type);
        
        ParameterCode = $"{Type.FullName} {Name}";
    }
}