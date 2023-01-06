﻿using static MsbRpc.Generator.CodeWriters.Utility.IndependentNames;

namespace MsbRpc.Generator.GenerationTree;

internal class ParameterNode
{
    public readonly string Name;
    public readonly string ReceivedArgument;
    public readonly string SizeVariableName;
    public readonly TypeNode Type;

    public ParameterNode(string name, TypeNode type)
    {
        Name = name.ToCamelCase();
        SizeVariableName = Name + Variables.SizePostfix;
        ReceivedArgument = Name + Variables.ReceivedPostfix;
        Type = type;
    }

    public override string ToString() => $"{Name} ({Type})";
}