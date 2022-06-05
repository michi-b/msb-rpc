using System.Diagnostics.CodeAnalysis;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MsbRps.Serialization.Primitives;

namespace MsbRpsTest.Serialization.Primitives;

[TestClass]
#pragma warning disable IDE0079 // Remove unnecessary suppression
[SuppressMessage("ReSharper", "BuiltInTypeReferenceStyle")]
[SuppressMessage("ReSharper", "BuiltInTypeReferenceStyleForMemberAccess")]
#pragma warning restore IDE0079 // Remove unnecessary suppression
public class Int32SerializationTest : PrimitiveSerializationTest<Int32>
{
    protected override int ElementSize => sizeof(Int32);

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
        byte[] buffer = GetBuffer(5);
        Serializer.WriteInt32(value, buffer, offset);
        Int32 result = PrimitiveSerializer.ReadInt32(buffer, offset);
        Assert.AreEqual(value, result);
    }

        [TestMethod]
    public void PreservesSeries()
    {
        const Int32 value0 = 1234;
        const Int32 value1 = -981;
        const Int32 value2 = 1324981234;

        byte[] buffer = GetBuffer(3);

        Serializer.WriteInt32(value0, buffer, GetOffset(0));
        Serializer.WriteInt32(value1, buffer, GetOffset(1));
        Serializer.WriteInt32(value2, buffer, GetOffset(2));

        Int32 result0 = PrimitiveSerializer.ReadInt32(buffer, GetOffset(0));
        Int32 result1 = PrimitiveSerializer.ReadInt32(buffer, GetOffset(1));
        Int32 result2 = PrimitiveSerializer.ReadInt32(buffer, GetOffset(2));

        Assert.AreEqual(value0, result0);
        Assert.AreEqual(value1, result1);
        Assert.AreEqual(value2, result2);
    }

    protected override void WriteSingleElement(Int32 value)
    {
        Serializer.WriteInt32(value, SingleElementBuffer);
    }

    protected override Int32 ReadSingleElement() => PrimitiveSerializer.ReadInt32(SingleElementBuffer);
}