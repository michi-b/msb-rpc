using System;
using MsbRpc.Serialization.Buffers;
using MsbRpc.Serialization.Primitives;

namespace MsbRpc.Serialization;

public static class NullableSerializer<TValue> where TValue : struct
{
    public static int GetSize(TValue? value, Func<TValue?, int> getValueSize) 
        => value is null ? PrimitiveSerializer.BoolSize : PrimitiveSerializer.BoolSize + getValueSize(value);

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

    public static TValue? Read(ref BufferReader reader, Func<BufferReader, TValue> readValue)
    {
        bool isNull = reader.ReadBool();
        return isNull ? null : readValue(reader);
    }
}