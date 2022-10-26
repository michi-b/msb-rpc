using System.Diagnostics.CodeAnalysis;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MsbRpc.Serialization;
using MsbRpc.Serialization.Primitives;
using MsbRpcTest.Serialization.Primitives.ByteArray;

namespace MsbRpcTest.Serialization.Primitives.ByteArraySegmentExtensions;

[TestClass]
#pragma warning disable IDE0079 // Remove unnecessary suppression
[SuppressMessage("ReSharper", "BuiltInTypeReferenceStyle")]
[SuppressMessage("ReSharper", "BuiltInTypeReferenceStyleForMemberAccess")]
#pragma warning restore IDE0079 // Remove unnecessary suppression
public class DecimalSerializationTest : PrimitiveByteArraySegmentSerializationTest<Decimal>
{
    [TestMethod]
    public void PreservesZero()
    {
        TestPreserves(0);
    }

    [TestMethod]
    public void PreservesOne()
    {
        TestPreserves(1);
    }

    [TestMethod]
    public void PreservesMinusOne()
    {
        TestPreserves(-1);
    }

    [TestMethod]
    public void PreservesMinusZero()
    {
        TestPreserves(-0);
    }

    [TestMethod]
    public void PreservesMinimum()
    {
        TestPreserves(Decimal.MinValue);
    }

    [TestMethod]
    public void PreservesMaximum()
    {
        TestPreserves(Decimal.MaxValue);
    }

    [TestMethod]
    public void PreservesWithOffset()
    {
        const Decimal value = -3;
        const int offset = 13;
        ArraySegment<byte> buffer = GetBuffer(5);
        buffer.WriteDecimal(value, offset);
        Decimal result = buffer.ReadDecimal(offset);
        Assert.AreEqual(value, result);
    }

    [TestMethod]
    public void PreservesSeries()
    {
        const Decimal value0 = 1234;
        const Decimal value1 = -981;
        const Decimal value2 = 1324981234;

        ArraySegment<byte> buffer = GetBuffer(3);

        buffer.WriteDecimal(value0, GetOffset(0));
        buffer.WriteDecimal(value1, GetOffset(1));
        buffer.WriteDecimal(value2, GetOffset(2));

        Decimal result0 = buffer.ReadDecimal(GetOffset(0));
        Decimal result1 = buffer.ReadDecimal(GetOffset(1));
        Decimal result2 = buffer.ReadDecimal(GetOffset(2));

        Assert.AreEqual(value0, result0);
        Assert.AreEqual(value1, result1);
        Assert.AreEqual(value2, result2);
    }

    protected override void WriteSingleElement(Decimal value)
    {
        SingleElementBuffer.WriteDecimal(value);
    }

    protected override Decimal ReadSingleElement() => SingleElementBuffer.ReadDecimal();
}