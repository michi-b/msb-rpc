namespace MsbRpc.Generator.HelperTree.Names;

public readonly struct ParameterNames
{
    public ParameterNames(string name)
    {
        CamelCaseName = name.ToCamelCase();
        PascalCaseName = name.ToPascalCase();
    }

    public readonly string PascalCaseName;
    public readonly string CamelCaseName;
}