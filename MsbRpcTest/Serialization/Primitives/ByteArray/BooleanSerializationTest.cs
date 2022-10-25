using System.Diagnostics.CodeAnalysis;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MsbRpc.Serialization.Primitives;

namespace MsbRpcTest.Serialization.Primitives.ByteArray;

[TestClass]
[SuppressMessage("ReSharper", "BuiltInTypeReferenceStyle")]
public class BooleanSerializationTest : PrimitiveByteArraySerializationTest<Boolean>
{
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
        const Boolean value = true;
        const int offset = 3;
        byte[] buffer = GetBuffer(7);
        PrimitiveSerializer.WriteBoolean(value, buffer, offset);
        Boolean result = PrimitiveSerializer.ReadBoolean(buffer, offset);
        Assert.AreEqual(value, result);
    }

    [TestMethod]
    public void PreservesFalseWithOffset()
    {
        const Boolean value = false;
        const int offset = 2;
        byte[] buffer = GetBuffer(4);
        PrimitiveSerializer.WriteBoolean(value, buffer, offset);
        Boolean result = PrimitiveSerializer.ReadBoolean(buffer, offset);
        Assert.AreEqual(value, result);
    }

    [TestMethod]
    public void PreservesSeries()
    {
        const Boolean value0 = true;
        const Boolean value1 = false;
        const Boolean value2 = false;
        const Boolean value3 = true;
        const Boolean value4 = false;

        byte[] buffer = GetBuffer(5);

        PrimitiveSerializer.WriteBoolean(value0, buffer);
        PrimitiveSerializer.WriteBoolean(value1, buffer, GetOffset(1));
        PrimitiveSerializer.WriteBoolean(value2, buffer, GetOffset(2));
        PrimitiveSerializer.WriteBoolean(value3, buffer, GetOffset(3));
        PrimitiveSerializer.WriteBoolean(value4, buffer, GetOffset(4));

        Boolean result0 = PrimitiveSerializer.ReadBoolean(buffer, GetOffset(0));
        Boolean result1 = PrimitiveSerializer.ReadBoolean(buffer, GetOffset(1));
        Boolean result2 = PrimitiveSerializer.ReadBoolean(buffer, GetOffset(2));
        Boolean result3 = PrimitiveSerializer.ReadBoolean(buffer, GetOffset(3));
        Boolean result4 = PrimitiveSerializer.ReadBoolean(buffer, GetOffset(4));

        Assert.AreEqual(value0, result0);
        Assert.AreEqual(value1, result1);
        Assert.AreEqual(value2, result2);
        Assert.AreEqual(value3, result3);
        Assert.AreEqual(value4, result4);
    }

    protected override void WriteSingleElement(Boolean value)
    {
        PrimitiveSerializer.WriteBoolean(value, SingleElementBuffer);
    }

    protected override Boolean ReadSingleElement() => PrimitiveSerializer.ReadBoolean(SingleElementBuffer);
}