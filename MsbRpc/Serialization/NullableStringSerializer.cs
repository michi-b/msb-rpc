using MsbRpc.Serialization.Buffers;
using MsbRpc.Serialization.Primitives;

namespace MsbRpc.Serialization;

public static class NullableStringSerializer
{
    public static int GetSize(string? value) => value == null ? PrimitiveSerializer.IntSize : StringSerializer.GetSize(value);

    public static void WriteNullable(ref this BufferWriter writer, string? value)
    {
        //null case
        if (value == null)
        {
            writer.Write(-1);
            return;
        }

        writer.Write(value);
    }

    public static string? ReadStringNullable(ref this BufferReader reader)
    {
        int count = reader.ReadInt();
        return count == -1 ? null : reader.ReadString(count);
    }
}