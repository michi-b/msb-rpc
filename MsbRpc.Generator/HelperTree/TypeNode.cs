using MsbRpc.Generator.HelperTree.Names;
using MsbRpc.Generator.Info;

namespace MsbRpc.Generator.HelperTree;

public class TypeNode
{
    private readonly string? _constantSizeExpression;
    public readonly bool IsConstantSize;
    public readonly bool IsPrimitive;
    public readonly TypeNames Names;
    public readonly SerializationKind SerializationKind;
    
    public TypeNode(ref TypeInfo info)
    {
        SerializationKindUtility.TryGetPrimitiveSerializationKind($"{info.Namespace}.{info.LocalName}", out SerializationKind);
        Names = new TypeNames(info, SerializationKind);
        IsPrimitive = SerializationKind.GetIsPrimitive();
        IsConstantSize = SerializationKind.TryGetConstantSizeExpression(out _constantSizeExpression);
    }

    public bool TryGetConstantSizeExpression(out string? expression)
    {
        expression = _constantSizeExpression;
        return IsConstantSize;
    }
}