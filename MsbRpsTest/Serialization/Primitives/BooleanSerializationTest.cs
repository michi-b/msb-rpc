using System.Diagnostics.CodeAnalysis;
using System.Net.Sockets;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MsbRps.Serialization;
using MsbRps.Serialization.Primitives;

namespace MsbRpsTest.Serialization.Primitives;

[TestClass]
[SuppressMessage("ReSharper", "BuiltInTypeReferenceStyle")]
public class BooleanSerializationTest : PrimitiveSerializationTest<Boolean>
{
    protected override int ElementSize { get; } = sizeof(Boolean);

    [TestMethod]
    public void PreservesTrue()
    {
        TestPreserves(true);
    }

    [TestMethod]
    public void PreservesFalse()
    {
        TestPreserves(false);
    }

    [TestMethod]
    public void PreservesTrueWithOffset()
    {
        const bool value = true;
        const int offset = 3;
        byte[] buffer = GetBuffer(7);
        PrimitiveSerializer.Write(value, buffer, offset);
        bool result = PrimitiveSerializer.ReadBoolean(buffer, offset);
        Assert.AreEqual(value, result);
    }
    
    [TestMethod]
    public void PreservesFalseWithOffset()
    {
        const bool value = false;
        const int offset = 2;
        byte[] buffer = GetBuffer(4);
        PrimitiveSerializer.Write(value, buffer, offset);
        bool result = PrimitiveSerializer.ReadBoolean(buffer, offset);
        Assert.AreEqual(value, result);
    }

    [TestMethod]
    public void PreservesSeries()
    {
        const bool value0 = true;
        const bool value1 = false;
        const bool value2 = false;
        const bool value3 = true;
        const bool value4 = false;

        byte[] buffer = GetBuffer(5);

        PrimitiveSerializer.Write(value0, buffer, 0);
        PrimitiveSerializer.Write(value1, buffer, GetOffset(1));
        PrimitiveSerializer.Write(value2, buffer, GetOffset(2));
        PrimitiveSerializer.Write(value3, buffer, GetOffset(3));
        PrimitiveSerializer.Write(value4, buffer, GetOffset(4));

        bool result0 = PrimitiveSerializer.ReadBoolean(buffer, GetOffset(0));
        bool result1 = PrimitiveSerializer.ReadBoolean(buffer, GetOffset(1));
        bool result2 = PrimitiveSerializer.ReadBoolean(buffer, GetOffset(2));
        bool result3 = PrimitiveSerializer.ReadBoolean(buffer, GetOffset(3));
        bool result4 = PrimitiveSerializer.ReadBoolean(buffer, GetOffset(4));

        Assert.AreEqual(value0, result0);
        Assert.AreEqual(value1, result1);
        Assert.AreEqual(value2, result2);
        Assert.AreEqual(value3, result3);
        Assert.AreEqual(value4, result4);
    }

    protected override void WriteSingleElement(bool value)
    {
        PrimitiveSerializer.Write(value, SingleElementBuffer);
    }

    protected override bool ReadSingleElement() => PrimitiveSerializer.ReadBoolean(SingleElementBuffer);
}