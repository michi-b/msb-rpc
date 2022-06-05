using System.Diagnostics.CodeAnalysis;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MsbRpc.Serialization.Primitives;

namespace MsbRpcTest.Serialization.Primitives;

[TestClass]
#pragma warning disable IDE0079 // Remove unnecessary suppression
[SuppressMessage("ReSharper", "BuiltInTypeReferenceStyle")]
[SuppressMessage("ReSharper", "BuiltInTypeReferenceStyleForMemberAccess")]
#pragma warning restore IDE0079 // Remove unnecessary suppression
public class Int64SerializationTest : PrimitiveSerializationTest<Int64>
{
    protected override int ElementSize => sizeof(Int64);

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
        TestPreserves(Int64.MinValue);
    }

    [TestMethod]
    public void PreservesMaximum()
    {
        TestPreserves(Int64.MaxValue);
    }

    [TestMethod]
    public void PreservesWithOffset()
    {
        const Int64 value = -3;
        const int offset = 13;
        byte[] buffer = GetBuffer(5);
        Serializer.WriteInt64(value, buffer, offset);
        Int64 result = PrimitiveSerializer.ReadInt64(buffer, offset);
        Assert.AreEqual(value, result);
    }

        [TestMethod]
    public void PreservesSeries()
    {
        const Int64 value0 = 1234;
        const Int64 value1 = -981;
        const Int64 value2 = 1324981234;

        byte[] buffer = GetBuffer(3);

        Serializer.WriteInt64(value0, buffer, GetOffset(0));
        Serializer.WriteInt64(value1, buffer, GetOffset(1));
        Serializer.WriteInt64(value2, buffer, GetOffset(2));

        Int64 result0 = PrimitiveSerializer.ReadInt64(buffer, GetOffset(0));
        Int64 result1 = PrimitiveSerializer.ReadInt64(buffer, GetOffset(1));
        Int64 result2 = PrimitiveSerializer.ReadInt64(buffer, GetOffset(2));

        Assert.AreEqual(value0, result0);
        Assert.AreEqual(value1, result1);
        Assert.AreEqual(value2, result2);
    }

    protected override void WriteSingleElement(Int64 value)
    {
        Serializer.WriteInt64(value, SingleElementBuffer);
    }

    protected override Int64 ReadSingleElement() => PrimitiveSerializer.ReadInt64(SingleElementBuffer);
}