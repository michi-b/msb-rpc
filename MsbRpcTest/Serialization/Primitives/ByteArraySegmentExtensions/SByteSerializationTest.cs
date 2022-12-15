using System.Diagnostics.CodeAnalysis;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MsbRpc.Serialization.Buffers;

namespace MsbRpcTest.Serialization.Primitives.ByteArraySegmentExtensions;

[TestClass]
[SuppressMessage("ReSharper", "BuiltInTypeReferenceStyle")]
public class SByteSerializationTest : PrimitiveBufferSerializationTest<SByte>
{
    [TestMethod]
    public void PreservesMinValue()
    {
        TestPreserves(SByte.MinValue);
    }

    [TestMethod]
    public void PreservesMaxValue()
    {
        TestPreserves(SByte.MaxValue);
    }

    [TestMethod]
    public void PreservesMinus2WithOffset()
    {
        const SByte value = -2;
        const int offset = 2;
        ArraySegment<byte> buffer = GetBuffer(4);
        buffer.WriteSByte(value, offset);
        SByte result = buffer.ReadSByte(offset);
        Assert.AreEqual(value, result);
    }

    [TestMethod]
    public void PreservesSeries()
    {
        const SByte value0 = 5;
        const SByte value1 = -4;
        const SByte value2 = 1;
        const SByte value3 = -3;
        const SByte value4 = 2;

        ArraySegment<byte> buffer = GetBuffer(5);

        buffer.WriteSByte(value0);
        buffer.WriteSByte(value1, GetOffset(1));
        buffer.WriteSByte(value2, GetOffset(2));
        buffer.WriteSByte(value3, GetOffset(3));
        buffer.WriteSByte(value4, GetOffset(4));

        SByte result0 = buffer.ReadSByte(GetOffset(0));
        SByte result1 = buffer.ReadSByte(GetOffset(1));
        SByte result2 = buffer.ReadSByte(GetOffset(2));
        SByte result3 = buffer.ReadSByte(GetOffset(3));
        SByte result4 = buffer.ReadSByte(GetOffset(4));

        Assert.AreEqual(value0, result0);
        Assert.AreEqual(value1, result1);
        Assert.AreEqual(value2, result2);
        Assert.AreEqual(value3, result3);
        Assert.AreEqual(value4, result4);
    }

    protected override void WriteSingleElement(SByte value)
    {
        SingleElementBuffer.WriteSByte(value);
    }

    protected override SByte ReadSingleElement() => SingleElementBuffer.ReadSByte();
}