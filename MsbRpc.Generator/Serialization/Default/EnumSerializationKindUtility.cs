namespace MsbRpc.Generator.Serialization.Default;

public static class EnumSerializationKindUtility
{
    public static readonly EnumSerializationKind[] All =
    {
        EnumSerializationKind.Byte,
        EnumSerializationKind.Sbyte,
        EnumSerializationKind.Int,
        EnumSerializationKind.Long,
        EnumSerializationKind.Short,
        EnumSerializationKind.Uint,
        EnumSerializationKind.Ulong,
        EnumSerializationKind.Ushort
    };

    /// <summary>
    ///     Number of enum serialization kinds, which is 8.
    /// </summary>
    public static readonly int Count = All.Length;
}