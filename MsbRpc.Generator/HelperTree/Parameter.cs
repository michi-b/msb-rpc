using MsbRpc.Generator.HelperTree.Names;

namespace MsbRpc.Generator.HelperTree;

public readonly struct Parameter
{
    public readonly ParameterNames Names;
    public readonly TypeNode Type;

    public Parameter(string name, TypeNode type)
    {
        Names = new ParameterNames(name);
        Type = type;
    }
}