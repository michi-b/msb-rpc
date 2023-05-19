using Microsoft.VisualStudio.TestTools.UnitTesting;
using MsbRpc.Serialization;
using MsbRpc.Serialization.Buffers;

namespace MsbRpc.Test.Serialization;

[TestClass]
public class StringSerializationTest : Base.Test
{
    [TestMethod]
    public void PreservesHelloWorldInBufferReaderAndWriter()
    {
        const string testValue = "Hello World!";

        int size = StringSerializer.GetSize(testValue);
        
        ArraySegment<byte> buffer = BufferUtility.Create(size);
        
        var writer = new BufferWriter(buffer);
        
        writer.Write(testValue);
        var reader = new BufferReader(buffer);
        string result = reader.ReadString();
        
        Assert.AreEqual(testValue, result);
    }
}