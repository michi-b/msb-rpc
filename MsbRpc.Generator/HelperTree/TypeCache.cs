using MsbRpc.Generator.Info;

namespace MsbRpc.Generator.HelperTree;

public readonly struct TypeCache
{
    private readonly Dictionary<TypeInfo, TypeNode> _cache = new();

    public TypeCache()
    { }

    public TypeNode GetOrAdd(TypeInfo typeInfo)
    {
        if (_cache.TryGetValue(typeInfo, out TypeNode? node))
        {
            return node;
        }

        node = new TypeNode(ref typeInfo);

        _cache.Add(typeInfo, node);

        return node;
    }
}