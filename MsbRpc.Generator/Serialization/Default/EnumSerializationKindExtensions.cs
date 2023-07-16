#region

using System;

#endregion

namespace MsbRpc.Generator.Serialization.Default;

public static class EnumSerializationKindExtensions
{
    public static string GetKeyword(this EnumSerializationKind target)
        => target switch
        {
            EnumSerializationKind.Byte => "byte",
            EnumSerializationKind.Sbyte => "sbyte",
            EnumSerializationKind.Int => "int",
            EnumSerializationKind.Long => "long",
            EnumSerializationKind.Short => "short",
            EnumSerializationKind.Uint => "uint",
            EnumSerializationKind.Ulong => "ulong",
            EnumSerializationKind.Ushort => "ushort",
            _ => throw new ArgumentOutOfRangeException(nameof(target), target, null)
        };

    public static SimpleDefaultSerializationKind GetSimpleDefaultSerializationKind(this EnumSerializationKind target)
        => target switch
        {
            EnumSerializationKind.Byte => SimpleDefaultSerializationKind.Byte,
            EnumSerializationKind.Sbyte => SimpleDefaultSerializationKind.Sbyte,
            EnumSerializationKind.Int => SimpleDefaultSerializationKind.Int,
            EnumSerializationKind.Long => SimpleDefaultSerializationKind.Long,
            EnumSerializationKind.Short => SimpleDefaultSerializationKind.Short,
            EnumSerializationKind.Uint => SimpleDefaultSerializationKind.Uint,
            EnumSerializationKind.Ulong => SimpleDefaultSerializationKind.Ulong,
            EnumSerializationKind.Ushort => SimpleDefaultSerializationKind.Ushort,
            _ => throw new ArgumentOutOfRangeException(nameof(target), target, null)
        };
}