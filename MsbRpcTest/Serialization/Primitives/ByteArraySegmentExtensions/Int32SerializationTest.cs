using System.Diagnostics.CodeAnalysis;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MsbRpc.Serialization.Buffers;

namespace MsbRpcTest.Serialization.Primitives.ByteArraySegmentExtensions;

[TestClass]
#pragma warning disable IDE0079 // Remove unnecessary suppression
[SuppressMessage("ReSharper", "BuiltInTypeReferenceStyle")]
[SuppressMessage("ReSharper", "BuiltInTypeReferenceStyleForMemberAccess")]
#pragma warning restore IDE0079 // Remove unnecessary suppression
public class Int32SerializationTest : PrimitiveByteArraySegmentSerializationTest<Int32>
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
    public void PreservesMinimum()
    {
        TestPreserves(Int32.MinValue);
    }

    [TestMethod]
    public void PreservesMaximum()
    {
        TestPreserves(Int32.MaxValue);
    }

    [TestMethod]
    public void PreservesWithOffset()
    {
        const Int32 value = -3;
        const int offset = 13;
        ArraySegment<byte> buffer = GetBuffer(5);
        buffer.WriteInt32(value, offset);
        Int32 result = buffer.ReadInt32(offset);
        Assert.AreEqual(value, result);
    }

    [TestMethod]
    public void PreservesSeries()
    {
        const Int32 value0 = 1234;
        const Int32 value1 = -981;
        const Int32 value2 = 1324981234;

        ArraySegment<byte> buffer = GetBuffer(3);

        buffer.WriteInt32(value0, GetOffset(0));
        buffer.WriteInt32(value1, GetOffset(1));
        buffer.WriteInt32(value2, GetOffset(2));

        Int32 result0 = buffer.ReadInt32(GetOffset(0));
        Int32 result1 = buffer.ReadInt32(GetOffset(1));
        Int32 result2 = buffer.ReadInt32(GetOffset(2));

        Assert.AreEqual(value0, result0);
        Assert.AreEqual(value1, result1);
        Assert.AreEqual(value2, result2);
    }

    protected override void WriteSingleElement(Int32 value)
    {
        SingleElementBuffer.WriteInt32(value);
    }

    protected override Int32 ReadSingleElement() => SingleElementBuffer.ReadInt32();
}