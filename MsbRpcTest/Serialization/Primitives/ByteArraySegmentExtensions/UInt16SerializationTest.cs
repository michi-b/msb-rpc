using System.Diagnostics.CodeAnalysis;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MsbRpc.Serialization.ByteArraySegment;

namespace MsbRpcTest.Serialization.Primitives.ByteArraySegmentExtensions;

[TestClass]
#pragma warning disable IDE0079 // Remove unnecessary suppression
[SuppressMessage("ReSharper", "BuiltInTypeReferenceStyle")]
[SuppressMessage("ReSharper", "BuiltInTypeReferenceStyleForMemberAccess")]
#pragma warning restore IDE0079 // Remove unnecessary suppression
public class UInt16SerializationTest : PrimitiveByteArraySegmentSerializationTest<UInt16>
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
        TestPreserves(UInt16.MinValue);
    }

    [TestMethod]
    public void PreservesMaximum()
    {
        TestPreserves(UInt16.MaxValue);
    }

    [TestMethod]
    public void PreservesWithOffset()
    {
        const UInt16 value = 5142;
        const int offset = 3;
        ArraySegment<byte> buffer = GetBuffer(5);
        buffer.WriteUInt16(value, offset);
        UInt16 result = buffer.ReadUInt16(offset);
        Assert.AreEqual(value, result);
    }

    [TestMethod]
    public void PreservesSeries()
    {
        const UInt16 value0 = 1234;
        const UInt16 value1 = 21543;
        const UInt16 value2 = 53249;

        ArraySegment<byte> buffer = GetBuffer(3);

        buffer.WriteUInt16(value0, GetOffset(0));
        buffer.WriteUInt16(value1, GetOffset(1));
        buffer.WriteUInt16(value2, GetOffset(2));

        UInt16 result0 = buffer.ReadUInt16(GetOffset(0));
        UInt16 result1 = buffer.ReadUInt16(GetOffset(1));
        UInt16 result2 = buffer.ReadUInt16(GetOffset(2));

        Assert.AreEqual(value0, result0);
        Assert.AreEqual(value1, result1);
        Assert.AreEqual(value2, result2);
    }

    protected override void WriteSingleElement(UInt16 value)
    {
        SingleElementBuffer.WriteUInt16(value);
    }

    protected override UInt16 ReadSingleElement() => SingleElementBuffer.ReadUInt16();
}