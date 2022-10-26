using System.Diagnostics.CodeAnalysis;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MsbRpc.Serialization.Primitives;

namespace MsbRpcTest.Serialization.Primitives.ByteArray;

[TestClass]
#pragma warning disable IDE0079 // Remove unnecessary suppression
[SuppressMessage("ReSharper", "BuiltInTypeReferenceStyle")]
[SuppressMessage("ReSharper", "BuiltInTypeReferenceStyleForMemberAccess")]
#pragma warning restore IDE0079 // Remove unnecessary suppression
public class UInt64SerializationTest : PrimitiveByteArraySerializationTest<UInt64>
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
        TestPreserves(UInt64.MinValue);
    }

    [TestMethod]
    public void PreservesMaximum()
    {
        TestPreserves(UInt64.MaxValue);
    }

    [TestMethod]
    public void PreservesWithOffset()
    {
        const UInt64 value = 5142;
        const int offset = 13;
        byte[] buffer = GetBuffer(5);
        buffer.WriteUInt64(value, offset);
        UInt64 result = buffer.ReadUInt64(offset);
        Assert.AreEqual(value, result);
    }

    [TestMethod]
    public void PreservesSeries()
    {
        const UInt64 value0 = 1234;
        const UInt64 value1 = 21543;
        const UInt64 value2 = 1324981234;

        byte[] buffer = GetBuffer(3);

        buffer.WriteUInt64(value0, GetOffset(0));
        buffer.WriteUInt64(value1, GetOffset(1));
        buffer.WriteUInt64(value2, GetOffset(2));

        UInt64 result0 = buffer.ReadUInt64(GetOffset(0));
        UInt64 result1 = buffer.ReadUInt64(GetOffset(1));
        UInt64 result2 = buffer.ReadUInt64(GetOffset(2));

        Assert.AreEqual(value0, result0);
        Assert.AreEqual(value1, result1);
        Assert.AreEqual(value2, result2);
    }

    protected override void WriteSingleElement(UInt64 value)
    {
        SingleElementBuffer.WriteUInt64(value);
    }

    protected override UInt64 ReadSingleElement() => SingleElementBuffer.ReadUInt64();
}