using System;
using MsbRpc.Attributes;
using MsbRpc.Serialization.Buffers;
using MsbRpc.Serialization.Primitives;
using static MsbRpc.Serialization.Buffers.BufferReader;
using static MsbRpc.Serialization.Buffers.BufferWriter;

namespace MsbRpc.Serialization;

public static class NullableSerializer<TValue> where TValue : struct
{
    [MayBeUsedByGeneratedCode]
    public static int GetSize(TValue? value, int constantValueSize) => value is null ? PrimitiveSerializer.BoolSize : PrimitiveSerializer.BoolSize + constantValueSize;

    [MayBeUsedByGeneratedCode]
    public static int GetSize(TValue? value, Func<TValue?, int> getValueSize)
        => value is null ? PrimitiveSerializer.BoolSize : PrimitiveSerializer.BoolSize + getValueSize(value);

    [MayBeUsedByGeneratedCode]
    public static void Write(ref BufferWriter writer, TValue? nullable, WriteDelegate<TValue> writeValue)
    {
        if (nullable is null)
        {
            writer.Write(false);
        }
        else
        {
            writer.Write(true);
            writer.WriteCustom(nullable.Value, writeValue);
        }
    }

    [MayBeUsedByGeneratedCode]
    public static TValue? Read<TValue>(ref BufferReader reader, ReadDelegate<TValue> readValue) where TValue : struct
    {
        bool isNull = reader.ReadBool();
        return isNull ? null : reader.ReadCustom(readValue);
    }
}