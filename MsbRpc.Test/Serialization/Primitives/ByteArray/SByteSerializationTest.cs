using System.Diagnostics.CodeAnalysis;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MsbRpc.Serialization.Primitives;

namespace MsbRpc.Test.Serialization.Primitives.ByteArray;

[TestClass]
[SuppressMessage("ReSharper", "BuiltInTypeReferenceStyle")]
public class SByteSerializationTest : PrimitiveByteArraySerializationTest<SByte>
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
        byte[] buffer = GetBuffer(4);
        buffer.WriteSbyte(value, offset);
        SByte result = buffer.ReadSbyte(offset);
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

        byte[] buffer = GetBuffer(5);

        buffer.WriteSbyte(value0);
        buffer.WriteSbyte(value1, GetOffset(1));
        buffer.WriteSbyte(value2, GetOffset(2));
        buffer.WriteSbyte(value3, GetOffset(3));
        buffer.WriteSbyte(value4, GetOffset(4));

        SByte result0 = buffer.ReadSbyte(GetOffset(0));
        SByte result1 = buffer.ReadSbyte(GetOffset(1));
        SByte result2 = buffer.ReadSbyte(GetOffset(2));
        SByte result3 = buffer.ReadSbyte(GetOffset(3));
        SByte result4 = buffer.ReadSbyte(GetOffset(4));

        Assert.AreEqual(value0, result0);
        Assert.AreEqual(value1, result1);
        Assert.AreEqual(value2, result2);
        Assert.AreEqual(value3, result3);
        Assert.AreEqual(value4, result4);
    }

    protected override void WriteSingleElement(SByte value)
    {
        SingleElementBuffer.WriteSbyte(value);
    }

    protected override SByte ReadSingleElement() => SingleElementBuffer.ReadSbyte();
}