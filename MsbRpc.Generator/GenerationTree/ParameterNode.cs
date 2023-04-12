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
        Name = name.PascalToCamelCase();
        ArgumentVariableName = Name + ArgumentPostfix;
        SizeVariableName = ArgumentVariableName + SizePostfix;
        Index = index;
        Type = type;
        WriteToRequestWriterStatement = type.GetBufferWriterWriteStatement(Variables.RequestWriter, Name);
    }

    public string GetDeclarationStatement() => $"{Type.DeclarationSyntax} {ArgumentVariableName};";

    public string GetRequestReadStatement() => $"{ArgumentVariableName} = {Type.GetBufferReadExpression(Variables.RequestReader)};";

    public override string ToString() => $"{Name} ({Type})";

    public void WriteSizeVariableInitialization(TextWriter writer)
    {
        Type.WriteTargetSizeVariableInitialization(writer, SizeVariableName, Name);
    }
}