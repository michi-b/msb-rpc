using static MsbRpc.Generator.CodeWriters.Utility.IndependentNames;

namespace MsbRpc.Generator.GenerationTree;

internal class ParameterNode
{
    public readonly string ArgumentVariableName;
    public readonly string Name;
    public readonly string SizeVariableName;
    public readonly TypeNode Type;
    public readonly string WriteToRequestWriterStatement;

    public ParameterNode(string name, TypeNode type)
    {
        Name = name.ToCamelCase();
        ArgumentVariableName = Name + ArgumentPostfix;
        SizeVariableName = ArgumentVariableName + SizePostfix;
        Type = type;
        WriteToRequestWriterStatement = $"{Variables.RequestWriter}.{Methods.BufferWrite}({Name});";
    }

    public string GetRequestReadStatement() => $"{Type.Name} {ArgumentVariableName} = {Type.GetBufferReadExpression(Variables.RequestReader)};";

    public override string ToString() => $"{Name} ({Type})";
}