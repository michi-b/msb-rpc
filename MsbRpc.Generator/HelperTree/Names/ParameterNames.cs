using static MsbRpc.Generator.IndependentNames;

namespace MsbRpc.Generator.HelperTree.Names;

public readonly struct ParameterNames
{
    public readonly string PascalCaseName;
    public readonly string Name;
    public readonly string SizeVariable;
    
    public ParameterNames(string name)
    {
        Name = name.ToCamelCase();
        PascalCaseName = name.ToPascalCase();
        SizeVariable = Name + Variables.SizeVariablePostFix;
    }

}