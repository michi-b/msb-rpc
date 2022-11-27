using System.Diagnostics.CodeAnalysis;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MsbRpc.Serialization.Buffers;

namespace MsbRpcTest.Serialization.Primitives.ByteArraySegmentExtensions;

[TestClass]
[SuppressMessage("ReSharper", "BuiltInTypeReferenceStyle")]
public class ByteSerializationTest : PrimitiveByteArraySegmentSerializationTest<Byte>
{
    [TestMethod]
    public void PreservesMinValue()
    {
        TestPreserves(Byte.MinValue);
    }

    [TestMethod]
    public void PreservesMaxValue()
    {
        TestPreserves(Byte.MaxValue);
    }

    [TestMethod]
    public void Preserves2WithOffset()
    {
        const Byte value = 2;
        const int offset = 4;
        ArraySegment<byte> buffer = GetBuffer(7);
        buffer.WriteByte(value, offset);
        Byte result = buffer.ReadByte(offset);
        Assert.AreEqual(value, result);
    }

    [TestMethod]
    public void PreservesSeries()
    {
        const Byte value0 = 5;
        const Byte value1 = 4;
        const Byte value2 = 1;
        const Byte value3 = 3;
        const Byte value4 = 2;

        ArraySegment<byte> buffer = GetBuffer(5);

        buffer.WriteByte(value0);
        buffer.WriteByte(value1, GetOffset(1));
        buffer.WriteByte(value2, GetOffset(2));
        buffer.WriteByte(value3, GetOffset(3));
        buffer.WriteByte(value4, GetOffset(4));

        Byte result0 = buffer.ReadByte(GetOffset(0));
        Byte result1 = buffer.ReadByte(GetOffset(1));
        Byte result2 = buffer.ReadByte(GetOffset(2));
        Byte result3 = buffer.ReadByte(GetOffset(3));
        Byte result4 = buffer.ReadByte(GetOffset(4));

        Assert.AreEqual(value0, result0);
        Assert.AreEqual(value1, result1);
        Assert.AreEqual(value2, result2);
        Assert.AreEqual(value3, result3);
        Assert.AreEqual(value4, result4);
    }

    protected override void WriteSingleElement(byte value)
    {
        SingleElementBuffer.WriteByte(value);
    }

    protected override Byte ReadSingleElement() => SingleElementBuffer.ReadByte();
}