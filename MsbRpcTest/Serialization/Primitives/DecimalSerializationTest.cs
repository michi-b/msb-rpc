using System.Diagnostics.CodeAnalysis;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MsbRpcTest.Serialization.Primitives;

[TestClass]
#pragma warning disable IDE0079 // Remove unnecessary suppression
[SuppressMessage("ReSharper", "BuiltInTypeReferenceStyle")]
[SuppressMessage("ReSharper", "BuiltInTypeReferenceStyleForMemberAccess")]
#pragma warning restore IDE0079 // Remove unnecessary suppression
public class DecimalSerializationTest : PrimitiveSerializationTest<Decimal>
{
    protected override int ElementSize => sizeof(Decimal);

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
        byte[] buffer = GetBuffer(5);
        Serializer.WriteDecimal(value, buffer, offset);
        Decimal result = Serializer.ReadDecimal(buffer, offset);
        Assert.AreEqual(value, result);
    }

        [TestMethod]
    public void PreservesSeries()
    {
        const Decimal value0 = 1234;
        const Decimal value1 = -981;
        const Decimal value2 = 1324981234;

        byte[] buffer = GetBuffer(3);

        Serializer.WriteDecimal(value0, buffer, GetOffset(0));
        Serializer.WriteDecimal(value1, buffer, GetOffset(1));
        Serializer.WriteDecimal(value2, buffer, GetOffset(2));

        Decimal result0 = Serializer.ReadDecimal(buffer, GetOffset(0));
        Decimal result1 = Serializer.ReadDecimal(buffer, GetOffset(1));
        Decimal result2 = Serializer.ReadDecimal(buffer, GetOffset(2));

        Assert.AreEqual(value0, result0);
        Assert.AreEqual(value1, result1);
        Assert.AreEqual(value2, result2);
    }

    protected override void WriteSingleElement(Decimal value)
    {
        Serializer.WriteDecimal(value, SingleElementBuffer);
    }

    protected override Decimal ReadSingleElement() => Serializer.ReadDecimal(SingleElementBuffer);
}