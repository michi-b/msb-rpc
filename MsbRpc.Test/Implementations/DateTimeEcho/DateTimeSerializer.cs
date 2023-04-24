using MsbRpc.Generator.Attributes.Serialization;
using MsbRpc.Serialization.Buffers;
using MsbRpc.Serialization.Primitives;

namespace MsbRpc.Test.Implementations.DateTimeEcho;

[ConstantSizeSerializer(typeof(DateTime))]
public static class DateTimeSerializer
{
    [ConstantSerializedSize] public const int Size = PrimitiveSerializer.LongSize;

    [SerializationMethod]
    public static void Write(BufferWriter writer, DateTime value)
    {
        writer.Write(value.Ticks);
    }

    [DeserializationMethod]
    public static DateTime Read(BufferReader reader) => new(reader.ReadLong());
}