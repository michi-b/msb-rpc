using System.Diagnostics.CodeAnalysis;
using System.Net.Sockets;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MsbRps.Serialization;

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
        Serializer.Write(value, buffer, offset);
        bool result = Serializer.ReadBoolean(buffer, offset);
        Assert.AreEqual(value, result);
    }
    
    [TestMethod]
    public void PreservesFalseWithOffset()
    {
        const bool value = false;
        const int offset = 2;
        byte[] buffer = GetBuffer(4);
        Serializer.Write(value, buffer, offset);
        bool result = Serializer.ReadBoolean(buffer, offset);
        Assert.AreEqual(value, result);
    }

    [TestMethod]
    public void PreservesSeries()
    {
        bool value0 = true;
        bool value1 = false;
        bool value2 = false;
        bool value3 = true;
        bool value4 = false;

        byte[] buffer = GetBuffer(5);

        Serializer.Write(value0, buffer, 0);
        Serializer.Write(value1, buffer, GetOffset(1));
        Serializer.Write(value2, buffer, GetOffset(2));
        Serializer.Write(value3, buffer, GetOffset(3));
        Serializer.Write(value4, buffer, GetOffset(4));

        bool result0 = Serializer.ReadBoolean(buffer, GetOffset(0));
        bool result1 = Serializer.ReadBoolean(buffer, GetOffset(1));
        bool result2 = Serializer.ReadBoolean(buffer, GetOffset(2));
        bool result3 = Serializer.ReadBoolean(buffer, GetOffset(3));
        bool result4 = Serializer.ReadBoolean(buffer, GetOffset(4));

        Assert.AreEqual(value0, result0);
        Assert.AreEqual(value1, result1);
        Assert.AreEqual(value2, result2);
        Assert.AreEqual(value3, result3);
        Assert.AreEqual(value4, result4);
    }

    protected override void WriteSingleElement(bool value)
    {
        Serializer.Write(value, SingleElementBuffer);
    }

    protected override bool ReadSingleElement() => Serializer.ReadBoolean(SingleElementBuffer);
}