using MsbRpc.Generator.Info;

namespace MsbRpc.Generator.GenerationHelpers;

public readonly struct TypeGenerator
{
    public readonly string FullName;
    public readonly TypeSerializationGenerator Serialization;

    public TypeGenerator(TypeInfo typeInfo)
    {
        string name = typeInfo.Name;
        string ns = typeInfo.Namespace;
        FullName = $"{ns}.{name}";
        Serialization = new TypeSerializationGenerator(FullName);
    }
}