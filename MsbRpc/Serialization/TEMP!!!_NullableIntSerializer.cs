using MsbRpc.Serialization.Buffers;
using MsbRpc.Serialization.Primitives;

namespace MsbRpc.Serialization;

public class NullableIntSerializer
{
    public static int GetSize(int? target)
    {
        return NullableSerializer<int>.GetSize(target, _ => PrimitiveSerializer.IntSize);
    }

    public static void Write(ref BufferWriter writer, int? target)
    {
        NullableSerializer<int>.Write(ref writer, target, (writer, value) => writer.Write(value));
    }

    public static int? Read(ref BufferReader reader)
    {
        return NullableSerializer<int>.Read(ref reader, reader => reader.ReadInt());
    }
}