using MsbRpc.Generator.GenerationTree.Names;
using MsbRpc.Generator.Info;

namespace MsbRpc.Generator.GenerationTree;

internal class TypeNode
{
    public readonly string? ConstantSizeExpression;
    public readonly bool IsConstantSize;
    public readonly TypeNames Names;
    public readonly SerializationKind SerializationKind;

    public TypeNode(ref TypeInfo info)
    {
        SerializationKindUtility.TryGetPrimitiveSerializationKind($"{info.Namespace}.{info.LocalName}", out SerializationKind);
        Names = new TypeNames(info, SerializationKind);
        ConstantSizeExpression = SerializationKind.GetConstantSizeExpression();
        IsConstantSize = ConstantSizeExpression != null;
    }
}