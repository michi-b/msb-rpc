using System.IO;
using static MsbRpc.Generator.CodeWriters.Utility.IndependentNames;

namespace MsbRpc.Generator.GenerationTree;

internal class ParameterNode
{
    public readonly string ArgumentVariableName;
    public readonly int Index;
    public readonly string Name;
    public readonly string SizeVariableName;
    public readonly TypeNode Type;
    public readonly string WriteToRequestWriterStatement;

    public ParameterNode(string name, int index, TypeNode type)
    {
        Name = name.ToCamelCase();
        ArgumentVariableName = Name + ArgumentPostfix;
        SizeVariableName = ArgumentVariableName + SizePostfix;
        Index = index;
        Type = type;
        WriteToRequestWriterStatement = $"{Variables.RequestWriter}.{Methods.BufferWrite}({Name});";
    }

    public string GetRequestReadStatement() => $"{Type.DeclarationSyntax} {ArgumentVariableName} = {Type.GetBufferReadExpression(Variables.RequestReader)};";

    public override string ToString() => $"{Name} ({Type})";

    public void WriteSizeVariableInitialization(TextWriter writer)
    {
        Type.WriteSizeVariableInitialization(writer, SizeVariableName, Name);
    }
}