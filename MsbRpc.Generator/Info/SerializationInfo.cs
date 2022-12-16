namespace MsbRpc.Generator.Info;

public readonly struct SerializationInfo : IEquatable<SerializationInfo>
{
    public SerializationType SerializationType { get; }

    /// <param name="typeName">the full type name</param>
    public SerializationInfo(string typeName)
        => SerializationType = SerializationTypeUtility.TryGetPrimitiveType(typeName, out SerializationType primitiveSerializationType)
            ? primitiveSerializationType
            : SerializationType.Unresolved;

    public bool Equals(SerializationInfo other) => SerializationType == other.SerializationType;

    public override bool Equals(object? obj) => obj is SerializationInfo other && Equals(other);

    public override int GetHashCode() => (int)SerializationType;
}