using System.Diagnostics.CodeAnalysis;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MsbRpc.Serialization.Buffers;

namespace MsbRpcTest.Serialization.Primitives.ByteArraySegmentExtensions;

[TestClass]
[SuppressMessage("ReSharper", "BuiltInTypeReferenceStyle")]
public class BooleanSerializationTest : PrimitiveBufferSerializationTest<bool>
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
    public void TestPreservesTrueWithOffset()
    {
        const Boolean value = true;
        int offset = GetOffset(4);
        ArraySegment<byte> buffer = GetBuffer(10);
        buffer.WriteBool(value, offset);
        Boolean result = buffer.ReadBool(offset);
        Assert.AreEqual(value, result);
    }

    [TestMethod]
    public void TestPreservesFalseWithOffset()
    {
        const Boolean value = false;
        int offset = GetOffset(2);
        ArraySegment<byte> buffer = GetBuffer(3);
        buffer.WriteBool(value, offset);
        Boolean result = buffer.ReadBool(offset);
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

        ArraySegment<byte> buffer = GetBuffer(5);

        buffer.WriteBool(value0);
        buffer.WriteBool(value1, GetOffset(1));
        buffer.WriteBool(value2, GetOffset(2));
        buffer.WriteBool(value3, GetOffset(3));
        buffer.WriteBool(value4, GetOffset(4));

        Boolean result0 = buffer.ReadBool(GetOffset(0));
        Boolean result1 = buffer.ReadBool(GetOffset(1));
        Boolean result2 = buffer.ReadBool(GetOffset(2));
        Boolean result3 = buffer.ReadBool(GetOffset(3));
        Boolean result4 = buffer.ReadBool(GetOffset(4));

        Assert.AreEqual(value0, result0);
        Assert.AreEqual(value1, result1);
        Assert.AreEqual(value2, result2);
        Assert.AreEqual(value3, result3);
        Assert.AreEqual(value4, result4);
    }

    protected override void WriteSingleElement(bool value) => SingleElementBuffer.WriteBool(value);

    protected override bool ReadSingleElement() => SingleElementBuffer.ReadBool();
}