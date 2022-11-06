using System.Diagnostics.CodeAnalysis;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MsbRpc.Serialization.ByteArraySegment;

namespace MsbRpcTest.Serialization.Primitives.ByteArraySegmentExtensions;

[TestClass]
#pragma warning disable IDE0079 // Remove unnecessary suppression
[SuppressMessage("ReSharper", "BuiltInTypeReferenceStyle")]
[SuppressMessage("ReSharper", "BuiltInTypeReferenceStyleForMemberAccess")]
#pragma warning restore IDE0079 // Remove unnecessary suppression
public class UInt32SerializationTest : PrimitiveByteArraySegmentSerializationTest<UInt32>
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
    public void PreservesMinimum()
    {
        TestPreserves(UInt32.MinValue);
    }

    [TestMethod]
    public void PreservesMaximum()
    {
        TestPreserves(UInt32.MaxValue);
    }

    [TestMethod]
    public void PreservesWithOffset()
    {
        const UInt32 value = 5142;
        const int offset = 13;
        ArraySegment<byte> buffer = GetBuffer(5);
        buffer.WriteUInt32(value, offset);
        UInt32 result = buffer.ReadUInt32(offset);
        Assert.AreEqual(value, result);
    }

    [TestMethod]
    public void PreservesSeries()
    {
        const UInt32 value0 = 1234;
        const UInt32 value1 = 21543;
        const UInt32 value2 = 1324981234;

        ArraySegment<byte> buffer = GetBuffer(3);

        buffer.WriteUInt32(value0, GetOffset(0));
        buffer.WriteUInt32(value1, GetOffset(1));
        buffer.WriteUInt32(value2, GetOffset(2));

        UInt32 result0 = buffer.ReadUInt32(GetOffset(0));
        UInt32 result1 = buffer.ReadUInt32(GetOffset(1));
        UInt32 result2 = buffer.ReadUInt32(GetOffset(2));

        Assert.AreEqual(value0, result0);
        Assert.AreEqual(value1, result1);
        Assert.AreEqual(value2, result2);
    }

    protected override void WriteSingleElement(UInt32 value)
    {
        SingleElementBuffer.WriteUInt32(value);
    }

    protected override UInt32 ReadSingleElement() => SingleElementBuffer.ReadUInt32();
}