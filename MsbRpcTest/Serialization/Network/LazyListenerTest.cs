using System.CodeDom.Compiler;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MsbRpc.Messaging;
using MsbRpc.Serialization.Primitives;
using MsbRpcTest.Serialization.Network.Listeners;
using MessageList = System.Collections.Generic.List<System.ArraySegment<byte>>;

namespace MsbRpcTest.Serialization.Network;

[TestClass]
public class LazyListenerTest : Test
{
    [TestMethod]
    public async Task ClosingConnectionStopsListening()
    {
        var server = new LazySingleConnectionMessageListener(CancellationToken);
        using (Messenger client = await server.Connect())
        {
            //do nothing, just close connection
        }

        MessageList messages = await server.ListenTask;
        Assert.AreEqual(0, messages.Count);
    }

    [TestMethod]
    public async Task SingleByteMessagesIsDelivered()
    {
        const byte value = 123;

        var server = new LazySingleConnectionMessageListener(CancellationToken);
        using (Messenger client = await server.Connect())
        {
            await client.SendMessageAsync(new[] { value }, CancellationToken);
        }

        MessageList messages = await server.ListenTask;

        Log(messages);

        Assert.IsTrue(messages.Count == 1);
        Assert.AreEqual(value, messages[0][0]);
    }

    [TestMethod]
    public async Task EmptyMessageIsDelivered()
    {
        var server = new LazySingleConnectionMessageListener(CancellationToken);
        using (Messenger client = await server.Connect())
        {
            byte[] message = Array.Empty<byte>();
            await client.SendMessageAsync(message, CancellationToken);
        }

        MessageList messages = await server.ListenTask;

        Log(messages);

        Assert.IsTrue(messages.Count == 1);
        Assert.IsTrue(messages[0].Count == 0);
    }

    [TestMethod]
    public async Task IntMessageIsDelivered()
    {
        const int value = -912347287;

        var server = new LazySingleConnectionMessageListener(CancellationToken);

        PrimitiveSerializer primitiveSerializer = new();

        using (Messenger client = await server.Connect())
        {
            byte[] messageOut = new byte[PrimitiveSerializer.Int32Size];
            primitiveSerializer.WriteInt32(value, messageOut);
            await client.SendMessageAsync(messageOut);
        }

        MessageList messagesIn = await server.ListenTask;
        Log(messagesIn);

        Assert.AreEqual(1, messagesIn.Count);

        int valueReceived = PrimitiveSerializer.ReadInt32(messagesIn[0].Array!);

        Assert.AreEqual(value, valueReceived);
    }

    private static void Log(MessageList messages)
    {
        using var writer = new IndentedTextWriter(Console.Out);

        writer.Indent++;
        writer.WriteLine($"received {messages.Count} messages:");

        writer.Indent++;
        for (int i = 0; i < messages.Count; i++)
        {
            ArraySegment<byte> message = messages[i];
            writer.WriteLine($"message {i} is {message.Count} bytes:");

            writer.Indent++;
            writer.WriteLine($"{{{string.Join(',', message)}}}");
            writer.Indent--;
        }

        writer.Indent--;

        writer.Indent--;
    }
}