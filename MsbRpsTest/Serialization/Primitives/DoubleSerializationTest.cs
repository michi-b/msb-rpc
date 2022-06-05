using System.Diagnostics.CodeAnalysis;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MsbRps.Serialization.Primitives;

namespace MsbRpsTest.Serialization.Primitives;

[TestClass]
#pragma warning disable IDE0079 // Remove unnecessary suppression
[SuppressMessage("ReSharper", "BuiltInTypeReferenceStyle")]
[SuppressMessage("ReSharper", "BuiltInTypeReferenceStyleForMemberAccess")]
#pragma warning restore IDE0079 // Remove unnecessary suppression
public class DoubleSerializationTest : PrimitiveSerializationTest<Double>
{
    protected override int ElementSize => sizeof(Double);

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
        TestPreserves(Double.MinValue);
    }

    [TestMethod]
    public void PreservesMaximum()
    {
        TestPreserves(Double.MaxValue);
    }

    [TestMethod]
    public void PreservesWithOffset()
    {
        const Double value = -3.1234;
        const int offset = 13;
        byte[] buffer = GetBuffer(5);
        Serializer.WriteDouble(value, buffer, offset);
        Double result = PrimitiveSerializer.ReadDouble(buffer, offset);
        Assert.AreEqual(value, result);
    }

        [TestMethod]
    public void PreservesSeries()
    {
        const Double value0 = 1.234;
        const Double value1 = -98.1;
        const Double value2 = 1324.981234;

        byte[] buffer = GetBuffer(3);

        Serializer.WriteDouble(value0, buffer, GetOffset(0));
        Serializer.WriteDouble(value1, buffer, GetOffset(1));
        Serializer.WriteDouble(value2, buffer, GetOffset(2));

        Double result0 = PrimitiveSerializer.ReadDouble(buffer, GetOffset(0));
        Double result1 = PrimitiveSerializer.ReadDouble(buffer, GetOffset(1));
        Double result2 = PrimitiveSerializer.ReadDouble(buffer, GetOffset(2));

        Assert.AreEqual(value0, result0);
        Assert.AreEqual(value1, result1);
        Assert.AreEqual(value2, result2);
    }

    protected override void WriteSingleElement(Double value)
    {
        Serializer.WriteDouble(value, SingleElementBuffer);
    }

    protected override Double ReadSingleElement() => PrimitiveSerializer.ReadDouble(SingleElementBuffer);
}