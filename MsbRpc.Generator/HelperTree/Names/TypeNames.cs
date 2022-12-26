using MsbRpc.Generator.Info;

namespace MsbRpc.Generator.HelperTree.Names;

public class TypeNames
{
    public readonly string Name;

    public TypeNames(TypeInfo info, SerializationKind serializationKind)
    {
        Name = $"{info.Namespace}.{info.LocalName}";

        if (serializationKind.GetKeyword(out string? primitiveKeyword) && primitiveKeyword != null)
        {
            Name = primitiveKeyword;
        }
    }
}