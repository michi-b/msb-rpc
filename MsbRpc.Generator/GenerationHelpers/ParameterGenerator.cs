using MsbRpc.Generator.Info;

namespace MsbRpc.Generator.GenerationHelpers;

public readonly struct ParameterGenerator
{
    public readonly string ParameterCode { get; }
    public readonly TypeGenerator Type { get; }
    public readonly string Name { get; }

    public readonly string ConstantSizeVariableName;
    public readonly string? ConstantSizeVariableInitializationLine;
    public readonly bool IsConstantSize;

    public ParameterGenerator(ParameterInfo info)
    {
        Name = info.Name;
        Type = new TypeGenerator(info.Type);
        ParameterCode = $"{Type.Name} {Name}";

        ConstantSizeVariableName = $"{Name}Size";
        IsConstantSize = Type.TryGetConstantSizeInitializationLine(ConstantSizeVariableName, out ConstantSizeVariableInitializationLine);
    }
}