using System.Diagnostics.CodeAnalysis;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MsbRps.Serialization.Primitives;

namespace MsbRpsTest.Serialization.Primitives;

[TestClass]
#pragma warning disable IDE0079 // Remove unnecessary suppression
[SuppressMessage("ReSharper", "BuiltInTypeReferenceStyle")]
[SuppressMessage("ReSharper", "BuiltInTypeReferenceStyleForMemberAccess")]
#pragma warning restore IDE0079 // Remove unnecessary suppression
public class UInt32SerializationTest : PrimitiveSerializationTest<UInt32>
{
    protected override int ElementSize => sizeof(UInt32);

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
        byte[] buffer = GetBuffer(5);
        Serializer.WriteUInt32(value, buffer, offset);
        UInt32 result = PrimitiveSerializer.ReadUInt32(buffer, offset);
        Assert.AreEqual(value, result);
    }

        [TestMethod]
    public void PreservesSeries()
    {
        const UInt32 value0 = 1234;
        const UInt32 value1 = 21543;
        const UInt32 value2 = 1324981234;

        byte[] buffer = GetBuffer(3);

        Serializer.WriteUInt32(value0, buffer, GetOffset(0));
        Serializer.WriteUInt32(value1, buffer, GetOffset(1));
        Serializer.WriteUInt32(value2, buffer, GetOffset(2));

        UInt32 result0 = PrimitiveSerializer.ReadUInt32(buffer, GetOffset(0));
        UInt32 result1 = PrimitiveSerializer.ReadUInt32(buffer, GetOffset(1));
        UInt32 result2 = PrimitiveSerializer.ReadUInt32(buffer, GetOffset(2));

        Assert.AreEqual(value0, result0);
        Assert.AreEqual(value1, result1);
        Assert.AreEqual(value2, result2);
    }

    protected override void WriteSingleElement(UInt32 value)
    {
        Serializer.WriteUInt32(value, SingleElementBuffer);
    }

    protected override UInt32 ReadSingleElement() => PrimitiveSerializer.ReadUInt32(SingleElementBuffer);
}