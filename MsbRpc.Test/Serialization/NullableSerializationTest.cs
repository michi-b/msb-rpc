using Microsoft.VisualStudio.TestTools.UnitTesting;
using MsbRpc.Serialization;
using MsbRpc.Serialization.Buffers;
using MsbRpc.Serialization.Primitives;

namespace MsbRpc.Test.Serialization;

[TestClass]
public class NullableSerializationTest : Base.Test
{
    [TestMethod]
    public void PreservesNullableNonNullInt()
    {
        int? value = 4;

        int size = NullableSerializer<int>.GetSize(value, PrimitiveSerializer.IntSize);

        ArraySegment<byte> buffer = BufferUtility.Create(size);

        var writer = new BufferWriter(buffer);

        NullableSerializer<int>.Write
        (
            ref writer,
            value,
            (ref BufferWriter writer, int innerValue) => { writer.Write(innerValue); }
        );

        var reader = new BufferReader(buffer);

        int? result = NullableSerializer<int>.Read
        (
            ref reader,
            (ref BufferReader reader) => reader.ReadInt()
        );

        Assert.AreEqual(value, result);

        WriteNullableInt(result);
    }

    [TestMethod]
    public void PreservesNullableNullInt()
    {
        int? value = null;

        int size = NullableSerializer<int>.GetSize(value, PrimitiveSerializer.IntSize);

        ArraySegment<byte> buffer = BufferUtility.Create(size);

        var writer = new BufferWriter(buffer);

        NullableSerializer<int>.Write
        (
            ref writer,
            value,
            (ref BufferWriter writer, int innerValue) => { writer.Write(innerValue); }
        );

        var reader = new BufferReader(buffer);

        int? result = NullableSerializer<int>.Read
        (
            ref reader,
            (ref BufferReader reader) => reader.ReadInt()
        );

        Assert.AreEqual(value, result);

        WriteNullableInt(result);
    }

    private void WriteNullableInt(int? result)
    {
        TestContext.Write(result == null ? "null" : result.ToString());
    }
}