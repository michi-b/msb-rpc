using static MsbRpc.Generator.IndependentNames;

namespace MsbRpc.Generator.GenerationTree;

internal readonly struct ParameterNode
{
    public readonly TypeNode Type;
    public readonly string Name;
    public readonly string SizeVariableName;
    public readonly string ReceivedArgument;

    public ParameterNode(string name, TypeNode type)
    {
        Name = name.ToCamelCase();
        SizeVariableName = Name + Variables.SizePostfix;
        ReceivedArgument = Name + Variables.ReceivedPostfix;
        Type = type;
    }

    public override string ToString() => $"{Name} ({Type})";
}