using MsbRpc.Generator.Info;

namespace MsbRpc.Generator.HelperTree;

public class TypeNode
{
    public TypeNames Names;
    public SerializationKind SerializationKind;
    public bool IsPrimitive;

    public TypeNode(ref TypeInfo info)
    {
        Names = new TypeNames(info);
        IsPrimitive = SerializationTypeUtility.TryGetPrimitiveType(Names.FullName, out SerializationKind primitiveSerializationType);
        SerializationKind = IsPrimitive ? primitiveSerializationType : SerializationKind.Unresolved;
    }
}