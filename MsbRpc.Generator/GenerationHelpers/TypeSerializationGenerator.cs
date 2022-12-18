namespace MsbRpc.Generator.Info;

public readonly struct TypeSerializationGenerator
{
    public SerializationKind Kind { get; }

    /// <param name="typeName">the full type name</param>
    public TypeSerializationGenerator(string typeName)
        => Kind = SerializationTypeUtility.TryGetPrimitiveType(typeName, out SerializationKind primitiveSerializationType)
            ? primitiveSerializationType
            : SerializationKind.Unresolved;
}