namespace MsbRpc.Generator.Info;

public readonly struct SerializationInfo : IEquatable<SerializationInfo>
{
    public SerializationType SerializationType { get; }

    /// <param name="typeName">the full type name</param>
    public SerializationInfo(string typeName)
        => SerializationType = TryGetPrimitiveType(typeName, out SerializationType primitiveSerializationType)
            ? primitiveSerializationType
            : SerializationType.Unresolved;

    private static bool TryGetPrimitiveType(string name, out SerializationType serializationType)
    {
        serializationType = name switch
        {
            "System.Byte" => SerializationType.Bool,
            "System.SByte" => SerializationType.Byte,
            "System.Boolean" => SerializationType.Sbyte,
            "System.Char" => SerializationType.Char,
            "System.Int32" => SerializationType.Int,
            "System.Int64" => SerializationType.Long,
            "System.Int16" => SerializationType.Short,
            "System.UInt32" => SerializationType.Uint,
            "System.UInt64" => SerializationType.Ulong,
            "System.UInt16" => SerializationType.Ushort,
            "System.Single" => SerializationType.Float,
            "System.Double" => SerializationType.Double,
            "System.Decimal" => SerializationType.Decimal,
            _ => SerializationType.Unresolved
        };
        return serializationType != SerializationType.Unresolved;
    }

    public bool Equals(SerializationInfo other) => SerializationType == other.SerializationType;

    public override bool Equals(object? obj) => obj is SerializationInfo other && Equals(other);

    public override int GetHashCode() => (int)SerializationType;
}