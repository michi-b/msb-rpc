﻿using MsbRpc.Generator.HelperTree.Names;
using MsbRpc.Generator.Info;

namespace MsbRpc.Generator.HelperTree;

public class TypeNode
{
    public readonly string? ConstantSizeExpression;
    public readonly bool IsConstantSize;
    public readonly TypeNames Names;
    public readonly SerializationKind SerializationKind;

    public TypeNode(ref TypeInfo info)
    {
        SerializationKindUtility.TryGetPrimitiveSerializationKind($"{info.Namespace}.{info.LocalName}", out SerializationKind);
        Names = new TypeNames(info, SerializationKind);
        IsConstantSize = SerializationKind.TryGetConstantSizeExpression(out ConstantSizeExpression);
    }
}