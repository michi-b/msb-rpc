namespace MsbRpc.Generator.Info;

public readonly struct CustomSerializerInfo
{
    public readonly CustomSerializerKind Kind;

    /// <summary>
    ///     fully qualified type name
    /// </summary>
    public readonly string Name;

    public readonly string SerializationMethodName;
    public readonly string DeserializationMethodName;
    public readonly string SizeFieldName;
}