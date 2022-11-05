using System.Diagnostics.CodeAnalysis;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MsbRpc.Serialization;
using MsbRpc.Serialization.ByteArraySegment;
using MsbRpc.Serialization.Primitives;
using MsbRpcTest.Serialization.Primitives.ByteArray;

namespace MsbRpcTest.Serialization.Primitives.ByteArraySegmentExtensions;

[TestClass]
#pragma warning disable IDE0079 // Remove unnecessary suppression
[SuppressMessage("ReSharper", "BuiltInTypeReferenceStyle")]
[SuppressMessage("ReSharper", "BuiltInTypeReferenceStyleForMemberAccess")]
#pragma warning restore IDE0079 // Remove unnecessary suppression
public class SingleSerializationTest : PrimitiveByteArraySegmentSerializationTest<Single>
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
        TestPreserves(Single.Epsilon);
    }

    [TestMethod]
    public void PreservesMinusZero()
    {
        TestPreserves(-0);
    }

    [TestMethod]
    public void PreservesMinimum()
    {
        TestPreserves(Single.MinValue);
    }

    [TestMethod]
    public void PreservesMaximum()
    {
        TestPreserves(Single.MaxValue);
    }

    [TestMethod]
    public void PreservesEpsilon()
    {
        TestPreserves(Single.Epsilon);
    }

    [TestMethod]
    public void PreservesNaN()
    {
        TestPreserves(Single.NaN);
    }

    [TestMethod]
    public void PreservesPositiveInfinity()
    {
        TestPreserves(Single.PositiveInfinity);
    }

    [TestMethod]
    public void PreservesNegativeInfinity()
    {
        TestPreserves(Single.NegativeInfinity);
    }

    [TestMethod]
    public void PreservesWithOffset()
    {
        const Single value = -3.1234f;
        const int offset = 13;
        ArraySegment<byte> buffer = GetBuffer(5);
        buffer.WriteSingle(value, offset);
        Single result = buffer.ReadSingle(offset);
        Assert.AreEqual(value, result);
    }

    [TestMethod]
    public void PreservesSeries()
    {
        const Single value0 = 1.234f;
        const Single value1 = -98.1f;
        const Single value2 = 1324.981234f;

        ArraySegment<byte> buffer = GetBuffer(3);

        buffer.WriteSingle(value0, GetOffset(0));
        buffer.WriteSingle(value1, GetOffset(1));
        buffer.WriteSingle(value2, GetOffset(2));

        Single result0 = buffer.ReadSingle(GetOffset(0));
        Single result1 = buffer.ReadSingle(GetOffset(1));
        Single result2 = buffer.ReadSingle(GetOffset(2));

        Assert.AreEqual(value0, result0);
        Assert.AreEqual(value1, result1);
        Assert.AreEqual(value2, result2);
    }

    protected override void WriteSingleElement(Single value)
    {
        SingleElementBuffer.WriteSingle(value);
    }

    protected override Single ReadSingleElement() => SingleElementBuffer.ReadSingle();
}