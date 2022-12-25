using MsbRpc.Generator.Info;

namespace MsbRpc.Generator.HelperTree.Names;

public class TypeNames
{
    public readonly string FullName;
    public readonly string LowerCaseShortName;
    public readonly string UpperCaseShortName;

    public TypeNames(TypeInfo info)
    {
        FullName = $"{info.Namespace}.{info.LocalName}";

        if (SerializationKindUtility.TryGetPrimitiveSerialization(FullName, out SerializationKind primitiveSerialization))
        {
            if (primitiveSerialization.GetKeyword(out string? primitiveKeyword) && primitiveKeyword != null)
            {
                FullName = primitiveKeyword;
                UpperCaseShortName = primitiveKeyword.ToPascalCase();
                LowerCaseShortName = primitiveKeyword;
            }
            else
            {
                UpperCaseShortName = info.LocalName.ToPascalCase();
                LowerCaseShortName = info.LocalName.ToCamelCase();
            }
        }
    }
}