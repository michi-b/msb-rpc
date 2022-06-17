using System.Diagnostics.CodeAnalysis;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MsbRpc.Serialization.Primitives;

namespace MsbRpcTest.Serialization.Primitives;

[TestClass]
#pragma warning disable IDE0079 // Remove unnecessary suppression
[SuppressMessage("ReSharper", "BuiltInTypeReferenceStyle")]
[SuppressMessage("ReSharper", "BuiltInTypeReferenceStyleForMemberAccess")]
#pragma warning restore IDE0079 // Remove unnecessary suppression
public class UInt64SerializationTest : PrimitiveSerializationTest<UInt64>
{
    protected override int ElementSize => sizeof(UInt64);

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
        Serializer.WriteUInt64(value, buffer, offset);
        UInt64 result = PrimitiveSerializer.ReadUInt64(buffer, offset);
        Assert.AreEqual(value, result);
    }

    [TestMethod]
    public void PreservesSeries()
    {
        const UInt64 value0 = 1234;
        const UInt64 value1 = 21543;
        const UInt64 value2 = 1324981234;

        byte[] buffer = GetBuffer(3);

        Serializer.WriteUInt64(value0, buffer, GetOffset(0));
        Serializer.WriteUInt64(value1, buffer, GetOffset(1));
        Serializer.WriteUInt64(value2, buffer, GetOffset(2));

        UInt64 result0 = PrimitiveSerializer.ReadUInt64(buffer, GetOffset(0));
        UInt64 result1 = PrimitiveSerializer.ReadUInt64(buffer, GetOffset(1));
        UInt64 result2 = PrimitiveSerializer.ReadUInt64(buffer, GetOffset(2));

        Assert.AreEqual(value0, result0);
        Assert.AreEqual(value1, result1);
        Assert.AreEqual(value2, result2);
    }

    protected override void WriteSingleElement(UInt64 value)
    {
        Serializer.WriteUInt64(value, SingleElementBuffer);
    }

    protected override UInt64 ReadSingleElement() => PrimitiveSerializer.ReadUInt64(SingleElementBuffer);
}