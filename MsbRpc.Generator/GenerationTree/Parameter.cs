using MsbRpc.Generator.GenerationTree.Names;

namespace MsbRpc.Generator.GenerationTree;

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