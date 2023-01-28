using Microsoft.VisualStudio.TestTools.UnitTesting;
using MsbRpc.Serialization;
using MsbRpc.Serialization.Buffers;
using MsbRpc.Serialization.Primitives;

namespace MsbRpc.Test.Serialization;

[TestClass]
public class StringSerializationTest
{
    [TestMethod]
    public void PreservesHelloWorldInArraySegment()
    {
        const string value = "Hello World!";
        var buffer = new ArraySegment<byte>(new byte[value.Length * PrimitiveSerializer.CharSize]);
        buffer.Serialize(value);
        string result = buffer.DeserializeString();
        Assert.AreEqual(value, result);
    }

    [TestMethod]
    public void PreservesHelloWorldInBufferReaderAndWriter()
    {
        const string value = "Hello World!";
        var buffer = new ArraySegment<byte>(new byte[value.Length * PrimitiveSerializer.CharSize + PrimitiveSerializer.IntSize]);
        var bufferWriter = new BufferWriter(buffer);
        bufferWriter.Write(value);
        var bufferReader = new BufferReader(buffer);
        string result = bufferReader.ReadString();
        Assert.AreEqual(value, result);
    }
}