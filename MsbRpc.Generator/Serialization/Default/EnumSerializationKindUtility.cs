using System;

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

    public static EnumSerializationKind Get(string fullName)
        => fullName switch
        {
            "System.Byte" => EnumSerializationKind.Byte,
            "System.SByte" => EnumSerializationKind.Sbyte,
            "System.Int32" => EnumSerializationKind.Int,
            "System.Int64" => EnumSerializationKind.Long,
            "System.Int16" => EnumSerializationKind.Short,
            "System.UInt32" => EnumSerializationKind.Uint,
            "System.UInt64" => EnumSerializationKind.Ulong,
            "System.UInt16" => EnumSerializationKind.Ushort,
            _ => throw new ArgumentOutOfRangeException()
        };
}