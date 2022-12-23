using MsbRpc.Generator.Info;

namespace MsbRpc.Generator.GeneratorHelperTree;

public class TypeNode
{
    public string Name;
    public SerializationKind SerializationKind;
    public bool IsPrimitive;
    public string? BufferReadMethodName;
    public string? ConstantSizeCode;

    public TypeNode(ref TypeInfo info)
    {
        Name = $"{info.Namespace}.{info.LocalName}";
        
        IsPrimitive = SerializationTypeUtility.TryGetPrimitiveType(Name, out SerializationKind primitiveSerializationType);
        SerializationKind = IsPrimitive ? primitiveSerializationType : SerializationKind.Unresolved;
        
        if (IsPrimitive)
        {
            BufferReadMethodName = SerializationKind.GetBufferReadMethodName();
            ConstantSizeCode = SerializationKind.GetConstantSizeCode();
        }
    }
}