using static MsbRpc.Generator.CodeWriters.Utility.IndependentNames;

namespace MsbRpc.Generator.GenerationTree;

internal class ParameterNode
{
    public readonly string ArgumentVariableName;
    public readonly int Index;
    public readonly string Name;
    public readonly string SizeVariableName;
    public readonly TypeNode Type;

    public ParameterNode(string name, int index, TypeNode type)
    {
        Name = name.PascalToCamelCase();
        ArgumentVariableName = Name + ArgumentPostfix;
        SizeVariableName = ArgumentVariableName + SizePostfix;
        Index = index;
        Type = type;
    }

    public string GetDeclarationStatement() => $"{Type.DeclarationSyntax} {ArgumentVariableName};";

    public override string ToString() => $"{Name} ({Type})";
}