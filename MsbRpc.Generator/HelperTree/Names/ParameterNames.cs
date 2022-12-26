using static MsbRpc.Generator.IndependentNames;

namespace MsbRpc.Generator.HelperTree.Names;

public readonly struct ParameterNames
{
    public readonly string Name;
    public readonly string SizeVariable;
    public readonly string ReceivedArgument;

    public ParameterNames(string name)
    {
        Name = name.ToCamelCase();
        SizeVariable = Name + Variables.SizeVariablePostFix;
        ReceivedArgument = Name + Variables.ReceivedArgumentPostFix;
    }
}