using System;
using MsbRpc.Generator.Serialization;
using static MsbRpc.Generator.CodeWriters.Utility.IndependentNames;

namespace MsbRpc.Generator.GenerationTree;

internal class ParameterNode
{
    public readonly string ArgumentVariableName;
    public readonly int Index;
    public readonly string Name;
    public readonly string SizeVariableName;
    public readonly ISerialization Serialization;

    public ParameterNode(string name, int index, ISerialization serialization)
    {
        Name = name.PascalToCamelCase();
        ArgumentVariableName = Name + ArgumentPostfix;
        SizeVariableName = ArgumentVariableName + SizePostfix;
        Index = index;
        Serialization = serialization;
    }

    public string GetDeclarationStatement() => $"{Serialization.GetDeclarationSyntax()} {ArgumentVariableName};";

    public override string ToString() => $"{Name} ({Serialization.GetDeclarationSyntax()})";
}