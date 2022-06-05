using System.Diagnostics.CodeAnalysis;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MsbRps.Serialization.Primitives;

namespace MsbRpsTest.Serialization.Primitives;

[TestClass]
#pragma warning disable IDE0079 // Remove unnecessary suppression
[SuppressMessage("ReSharper", "BuiltInTypeReferenceStyle")]
[SuppressMessage("ReSharper", "BuiltInTypeReferenceStyleForMemberAccess")]
#pragma warning restore IDE0079 // Remove unnecessary suppression
public class Int16SerializationTest : PrimitiveSerializationTest<Int16>
{
    protected override int ElementSize => sizeof(Int16);

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
        byte[] buffer = GetBuffer(5);
        Serializer.WriteInt16(value, buffer, offset);
        Int16 result = PrimitiveSerializer.ReadInt16(buffer, offset);
        Assert.AreEqual(value, result);
    }

        [TestMethod]
    public void PreservesSeries()
    {
        const Int16 value0 = 1234;
        const Int16 value1 = -981;
        const Int16 value2 = -3324;

        byte[] buffer = GetBuffer(3);

        Serializer.WriteInt16(value0, buffer, GetOffset(0));
        Serializer.WriteInt16(value1, buffer, GetOffset(1));
        Serializer.WriteInt16(value2, buffer, GetOffset(2));

        Int16 result0 = PrimitiveSerializer.ReadInt16(buffer, GetOffset(0));
        Int16 result1 = PrimitiveSerializer.ReadInt16(buffer, GetOffset(1));
        Int16 result2 = PrimitiveSerializer.ReadInt16(buffer, GetOffset(2));

        Assert.AreEqual(value0, result0);
        Assert.AreEqual(value1, result1);
        Assert.AreEqual(value2, result2);
    }

    protected override void WriteSingleElement(Int16 value)
    {
        Serializer.WriteInt16(value, SingleElementBuffer);
    }

    protected override Int16 ReadSingleElement() => PrimitiveSerializer.ReadInt16(SingleElementBuffer);
}