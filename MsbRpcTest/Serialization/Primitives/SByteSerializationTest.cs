using System.Diagnostics.CodeAnalysis;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MsbRpc.Serialization.Primitives;

namespace MsbRpcTest.Serialization.Primitives;

[TestClass]
[SuppressMessage("ReSharper", "BuiltInTypeReferenceStyle")]
public class SByteSerializationTest : PrimitiveSerializationTest<SByte>
{
    protected override int ElementSize { get; } = sizeof(Boolean);

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
        PrimitiveSerializer.WriteSByte(value, buffer, offset);
        SByte result = PrimitiveSerializer.ReadSByte(buffer, offset);
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

        PrimitiveSerializer.WriteSByte(value0, buffer);
        PrimitiveSerializer.WriteSByte(value1, buffer, GetOffset(1));
        PrimitiveSerializer.WriteSByte(value2, buffer, GetOffset(2));
        PrimitiveSerializer.WriteSByte(value3, buffer, GetOffset(3));
        PrimitiveSerializer.WriteSByte(value4, buffer, GetOffset(4));

        SByte result0 = PrimitiveSerializer.ReadSByte(buffer, GetOffset(0));
        SByte result1 = PrimitiveSerializer.ReadSByte(buffer, GetOffset(1));
        SByte result2 = PrimitiveSerializer.ReadSByte(buffer, GetOffset(2));
        SByte result3 = PrimitiveSerializer.ReadSByte(buffer, GetOffset(3));
        SByte result4 = PrimitiveSerializer.ReadSByte(buffer, GetOffset(4));

        Assert.AreEqual(value0, result0);
        Assert.AreEqual(value1, result1);
        Assert.AreEqual(value2, result2);
        Assert.AreEqual(value3, result3);
        Assert.AreEqual(value4, result4);
    }

    protected override void WriteSingleElement(SByte value)
    {
        PrimitiveSerializer.WriteSByte(value, SingleElementBuffer);
    }

    protected override SByte ReadSingleElement() => PrimitiveSerializer.ReadSByte(SingleElementBuffer);
}