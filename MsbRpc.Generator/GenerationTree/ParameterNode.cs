#region

using MsbRpc.Generator.Serialization;
using static MsbRpc.Generator.Utility.Names;

#endregion

namespace MsbRpc.Generator.GenerationTree;

public class ParameterNode
{
    public readonly string ArgumentVariableName;
    public readonly int Index;
    public readonly string Name;
    public readonly ISerialization Serialization;
    public readonly string SizeVariableName;

    public ParameterNode(string name, int index, ISerialization serialization)
    {
        Name = name.PascalToCamelCase();
        ArgumentVariableName = Name + ArgumentPostfix;
        SizeVariableName = ArgumentVariableName + SizePostfix;
        Index = index;
        Serialization = serialization;
    }

    public string GetDeclarationStatement() => $"{Serialization.DeclarationSyntax} {ArgumentVariableName};";

    public override string ToString() => $"{Name} ({Serialization.DeclarationSyntax})";
}