namespace MsbRpc.Generator.Serialization.Default;

public static class SimpleDefaultSerializationKindUtility
{
    public const int DictionaryCapacity = 100;

    /// <summary>
    ///     the number of simple default serialization kinds, which is 14.
    /// </summary>
    public const int Count = 14;

    public static readonly SimpleDefaultSerializationKind[] All =
    {
        SimpleDefaultSerializationKind.Byte,
        SimpleDefaultSerializationKind.Sbyte,
        SimpleDefaultSerializationKind.Bool,
        SimpleDefaultSerializationKind.Char,
        SimpleDefaultSerializationKind.Int,
        SimpleDefaultSerializationKind.Long,
        SimpleDefaultSerializationKind.Short,
        SimpleDefaultSerializationKind.Uint,
        SimpleDefaultSerializationKind.Ulong,
        SimpleDefaultSerializationKind.Ushort,
        SimpleDefaultSerializationKind.Float,
        SimpleDefaultSerializationKind.Double,
        SimpleDefaultSerializationKind.Decimal,
        SimpleDefaultSerializationKind.String
    };
}