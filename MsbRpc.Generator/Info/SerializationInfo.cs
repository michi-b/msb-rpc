namespace MsbRpc.Generator.Info;

public readonly struct SerializationInfo : IEquatable<SerializationInfo>
{
    public SerializationType SerializationType { get; }

    /// <param name="typeName">the full type name</param>
    public SerializationInfo(string typeName)
        => SerializationType = GetIsPrimitive(typeName)
            ? SerializationType.Primitive
            : SerializationType.Unresolved;

    private static bool GetIsPrimitive(string name)
    {
        return name switch
        {
            "System.Boolean" => true,
            "System.Byte" => true,
            "System.Char" => true,
            // "System.DateTime" => true,
            "System.Decimal" => true,
            "System.Double" => true,
            // "System.Guid" => true,
            "System.Int16" => true,
            "System.Int32" => true,
            "System.Int64" => true,
            "System.SByte" => true,
            "System.Single" => true,
            // "System.String" => true,
            // "System.TimeSpan" => true,
            "System.UInt16" => true,
            "System.UInt32" => true,
            "System.UInt64" => true,
            _ => false
        };
    }

    public bool Equals(SerializationInfo other) => SerializationType == other.SerializationType;

    public override bool Equals(object? obj) => obj is SerializationInfo other && Equals(other);

    public override int GetHashCode() => (int)SerializationType;
}