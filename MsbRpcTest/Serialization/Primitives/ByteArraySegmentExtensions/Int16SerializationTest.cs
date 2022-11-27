using System.Diagnostics.CodeAnalysis;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MsbRpc.Serialization.Buffers;

namespace MsbRpcTest.Serialization.Primitives.ByteArraySegmentExtensions;

[TestClass]
#pragma warning disable IDE0079 // Remove unnecessary suppression
[SuppressMessage("ReSharper", "BuiltInTypeReferenceStyle")]
[SuppressMessage("ReSharper", "BuiltInTypeReferenceStyleForMemberAccess")]
#pragma warning restore IDE0079 // Remove unnecessary suppression
public class Int16SerializationTest : PrimitiveByteArraySegmentSerializationTest<Int16>
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
        TestPreserves(Int16.MinValue);
    }

    [TestMethod]
    public void PreservesMaximum()
    {
        TestPreserves(Int16.MaxValue);
    }

    [TestMethod]
    public void PreservesWithOffset()
    {
        const Int16 value = -3;
        const int offset = 7;
        ArraySegment<byte> buffer = GetBuffer(5);
        buffer.WriteInt16(value, offset);
        Int16 result = buffer.ReadInt16(offset);
        Assert.AreEqual(value, result);
    }

    [TestMethod]
    public void PreservesSeries()
    {
        const Int16 value0 = 1234;
        const Int16 value1 = -981;
        const Int16 value2 = -3324;

        ArraySegment<byte> buffer = GetBuffer(3);

        buffer.WriteInt16(value0, GetOffset(0));
        buffer.WriteInt16(value1, GetOffset(1));
        buffer.WriteInt16(value2, GetOffset(2));

        Int16 result0 = buffer.ReadInt16(GetOffset(0));
        Int16 result1 = buffer.ReadInt16(GetOffset(1));
        Int16 result2 = buffer.ReadInt16(GetOffset(2));

        Assert.AreEqual(value0, result0);
        Assert.AreEqual(value1, result1);
        Assert.AreEqual(value2, result2);
    }

    protected override void WriteSingleElement(Int16 value)
    {
        SingleElementBuffer.WriteInt16(value);
    }

    protected override Int16 ReadSingleElement() => SingleElementBuffer.ReadInt16();
}