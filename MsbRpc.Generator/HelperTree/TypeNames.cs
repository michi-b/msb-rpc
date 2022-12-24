using MsbRpc.Generator.Info;

namespace MsbRpc.Generator.HelperTree;

public class TypeNames
{
    public string FullName { get; }
    public string UpperCaseShortName { get; }
    public string LowerCaseShortName { get; }

    public TypeNames(TypeInfo info)
    {
        FullName = $"{info.Namespace}.{info.LocalName}";
        UpperCaseShortName = info.LocalName.ToUpperFirstChar();
        LowerCaseShortName = info.LocalName.ToLowerFirstChar();
    }
}