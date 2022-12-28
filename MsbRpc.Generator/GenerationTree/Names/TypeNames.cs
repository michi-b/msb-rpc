using MsbRpc.Generator.Info;

namespace MsbRpc.Generator.GenerationTree.Names;

internal struct TypeNames
{
    public readonly string FullName;
    public readonly string Name;

    public TypeNames(string fullName, SerializationKind serializationKind)
    {
        FullName = fullName;
        string? keyword = serializationKind.GetKeyword();
        Name = keyword ?? FullName;
    }

    private TypeNames(string fullName)
    {
        Name = fullName;
        FullName = fullName;
    }

    public static TypeNames CreateInvalid(string fullName) => new(fullName);
}