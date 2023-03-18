using Microsoft.VisualStudio.TestTools.UnitTesting;
using MsbRpc.Serialization;
using MsbRpc.Serialization.Buffers;

namespace MsbRpc.Test.Serialization;

[TestClass]
public class StringSerializationTest
{
    [TestMethod]
    public void PreservesHelloWorldInBufferReaderAndWriter()
    {
        const string value = "Hello World!";
        int size = NullableStringSerializer.GetSize(value);
        var buffer = new ArraySegment<byte>(new byte[size]);
        var writer = new BufferWriter(buffer);
        writer.Write(value);
        var reader = new BufferReader(buffer);
        string result = reader.ReadString();
        Assert.AreEqual(value, result);
    }
}