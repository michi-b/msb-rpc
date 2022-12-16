using System.Diagnostics.CodeAnalysis;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MsbRpc.Serialization.Buffers;

namespace MsbRpcTest.Serialization.Primitives.ByteArraySegmentExtensions;

[TestClass]
#pragma warning disable IDE0079 // Remove unnecessary suppression
[SuppressMessage("ReSharper", "BuiltInTypeReferenceStyle")]
[SuppressMessage("ReSharper", "BuiltInTypeReferenceStyleForMemberAccess")]
#pragma warning restore IDE0079 // Remove unnecessary suppression
public class Int64SerializationTest : PrimitiveBufferSerializationTest<Int64>
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
        TestPreserves(Int64.MinValue);
    }

    [TestMethod]
    public void PreservesMaximum()
    {
        TestPreserves(Int64.MaxValue);
    }

    [TestMethod]
    public void PreservesWithOffset()
    {
        const Int64 value = -3;
        const int offset = 13;
        ArraySegment<byte> buffer = GetBuffer(5);
        buffer.WriteLong(value, offset);
        Int64 result = buffer.ReadLong(offset);
        Assert.AreEqual(value, result);
    }

    [TestMethod]
    public void PreservesSeries()
    {
        const Int64 value0 = 1234;
        const Int64 value1 = -981;
        const Int64 value2 = 1324981234;

        ArraySegment<byte> buffer = GetBuffer(3);

        buffer.WriteLong(value0, GetOffset(0));
        buffer.WriteLong(value1, GetOffset(1));
        buffer.WriteLong(value2, GetOffset(2));

        Int64 result0 = buffer.ReadLong(GetOffset(0));
        Int64 result1 = buffer.ReadLong(GetOffset(1));
        Int64 result2 = buffer.ReadLong(GetOffset(2));

        Assert.AreEqual(value0, result0);
        Assert.AreEqual(value1, result1);
        Assert.AreEqual(value2, result2);
    }

    protected override void WriteSingleElement(Int64 value)
    {
        SingleElementBuffer.WriteLong(value);
    }

    protected override Int64 ReadSingleElement() => SingleElementBuffer.ReadLong();
}