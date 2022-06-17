using System.Diagnostics.CodeAnalysis;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MsbRpc.Serialization.Primitives;

namespace MsbRpcTest.Serialization.Primitives;

[TestClass]
#pragma warning disable IDE0079 // Remove unnecessary suppression
[SuppressMessage("ReSharper", "BuiltInTypeReferenceStyle")]
[SuppressMessage("ReSharper", "BuiltInTypeReferenceStyleForMemberAccess")]
#pragma warning restore IDE0079 // Remove unnecessary suppression
public class UInt16SerializationTest : PrimitiveSerializationTest<UInt16>
{
    protected override int ElementSize => sizeof(UInt16);

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
        byte[] buffer = GetBuffer(5);
        Serializer.WriteUInt16(value, buffer, offset);
        UInt16 result = PrimitiveSerializer.ReadUInt16(buffer, offset);
        Assert.AreEqual(value, result);
    }

    [TestMethod]
    public void PreservesSeries()
    {
        const UInt16 value0 = 1234;
        const UInt16 value1 = 21543;
        const UInt16 value2 = 53249;

        byte[] buffer = GetBuffer(3);

        Serializer.WriteUInt16(value0, buffer, GetOffset(0));
        Serializer.WriteUInt16(value1, buffer, GetOffset(1));
        Serializer.WriteUInt16(value2, buffer, GetOffset(2));

        UInt16 result0 = PrimitiveSerializer.ReadUInt16(buffer, GetOffset(0));
        UInt16 result1 = PrimitiveSerializer.ReadUInt16(buffer, GetOffset(1));
        UInt16 result2 = PrimitiveSerializer.ReadUInt16(buffer, GetOffset(2));

        Assert.AreEqual(value0, result0);
        Assert.AreEqual(value1, result1);
        Assert.AreEqual(value2, result2);
    }

    protected override void WriteSingleElement(UInt16 value)
    {
        Serializer.WriteUInt16(value, SingleElementBuffer);
    }

    protected override UInt16 ReadSingleElement() => PrimitiveSerializer.ReadUInt16(SingleElementBuffer);
}