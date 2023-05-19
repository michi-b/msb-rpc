using System;
using MsbRpc.Attributes;
using MsbRpc.Serialization.Buffers;
using MsbRpc.Serialization.Primitives;

namespace MsbRpc.Serialization;

public static class NullableSerializer<TValue> where TValue : struct
{
    [MayBeUsedByGeneratedCode]
    public static int GetSize(TValue? value, int constantValueSize) => value is null ? PrimitiveSerializer.BoolSize : PrimitiveSerializer.BoolSize + constantValueSize;

    [MayBeUsedByGeneratedCode]
    public static int GetSize(TValue? value, Func<TValue?, int> getValueSize)
        => value is null ? PrimitiveSerializer.BoolSize : PrimitiveSerializer.BoolSize + getValueSize(value);

    [MayBeUsedByGeneratedCode]
    public static void Write(ref BufferWriter writer, TValue? nullable, Action<BufferWriter, TValue> writeValue)
    {
        if (nullable is null)
        {
            writer.Write(false);
        }
        else
        {
            writer.Write(true);
            writeValue(writer, nullable.Value);
        }
    }

    [MayBeUsedByGeneratedCode]
    public static TValue? Read(ref BufferReader reader, Func<BufferReader, TValue> readValue)
    {
        bool isNull = reader.ReadBool();
        return isNull ? null : readValue(reader);
    }
}