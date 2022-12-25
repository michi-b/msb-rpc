using MsbRpc.Generator.HelperTree.Names;

namespace MsbRpc.Generator.HelperTree;

public readonly struct Parameter
{
    public Parameter(string name, TypeNode type)
    {
        Names = new ParameterNames(name);
        Type = type;
    }

    public ParameterNames Names { get; }

    public TypeNode Type { get; }
}