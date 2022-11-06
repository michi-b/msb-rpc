using System.Diagnostics.CodeAnalysis;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MsbRpc.Serialization.ByteArraySegment;

namespace MsbRpcTest.Serialization.Primitives.ByteArraySegmentExtensions;

[TestClass]
#pragma warning disable IDE0079 // Remove unnecessary suppression
[SuppressMessage("ReSharper", "BuiltInTypeReferenceStyle")]
[SuppressMessage("ReSharper", "BuiltInTypeReferenceStyleForMemberAccess")]
#pragma warning restore IDE0079 // Remove unnecessary suppression
public class DoubleSerializationTest : PrimitiveByteArraySegmentSerializationTest<Double>
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
        TestPreserves(Double.Epsilon);
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
    public void PreservesEpsilon()
    {
        TestPreserves(Double.Epsilon);
    }

    [TestMethod]
    public void PreservesNaN()
    {
        TestPreserves(Double.NaN);
    }

    [TestMethod]
    public void PreservesPositiveInfinity()
    {
        TestPreserves(Double.PositiveInfinity);
    }

    [TestMethod]
    public void PreservesNegativeInfinity()
    {
        TestPreserves(Double.NegativeInfinity);
    }

    [TestMethod]
    public void PreservesWithOffset()
    {
        const Double value = -3.1234;
        const int offset = 13;
        ArraySegment<byte> buffer = GetBuffer(5);
        buffer.WriteDouble(value, offset);
        Double result = buffer.ReadDouble(offset);
        Assert.AreEqual(value, result);
    }

    [TestMethod]
    public void PreservesSeries()
    {
        const Double value0 = 1.234;
        const Double value1 = -98.1;
        const Double value2 = 1324.981234;

        ArraySegment<byte> buffer = GetBuffer(3);

        buffer.WriteDouble(value0, GetOffset(0));
        buffer.WriteDouble(value1, GetOffset(1));
        buffer.WriteDouble(value2, GetOffset(2));

        Double result0 = buffer.ReadDouble(GetOffset(0));
        Double result1 = buffer.ReadDouble(GetOffset(1));
        Double result2 = buffer.ReadDouble(GetOffset(2));

        Assert.AreEqual(value0, result0);
        Assert.AreEqual(value1, result1);
        Assert.AreEqual(value2, result2);
    }

    protected override void WriteSingleElement(Double value)
    {
        SingleElementBuffer.WriteDouble(value);
    }

    protected override Double ReadSingleElement() => SingleElementBuffer.ReadDouble();
}