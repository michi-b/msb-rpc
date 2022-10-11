using System.CodeDom.Compiler;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MsbRpc.Messaging;
using MsbRpc.Serialization.Primitives;
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
    //
    // [TestMethod]
    // public async Task TooFewBytesReturnsConnectionClosedUnexpectedly()
    // {
    //     EndPoint ep = NetworkUtility.GetLocalEndPoint();
    //     using Task<SingleConnectionMessageReceiver.ListenResult> listen = NetworkUtility.ReceiveMessagesAsync(ep, CancellationToken);
    //     var client = new SingleConnectionMessageReceiver(await NetworkUtility.CreateConnectedSocket(ep));
    //
    //     const int value = 96192;
    //     byte[] message = BitConverter.GetBytes(value);
    //
    //     await clientRpcSocket.SendAsync(new ArraySegment<byte>(message, 0, 3), 4);
    //     clientRpcSocket.Close();
    //
    //     SingleConnectionMessageReceiver.ListenResult serverResult = await serverTask;
    //     Log(serverResult);
    //     Assert.AreEqual(ListenReturnCode.ConnectionClosedUnexpectedly, serverResult.ReturnCode);
    //     Assert.AreEqual(0, serverResult.Messages.Count);
    // }
    //
    // [TestMethod]
    // public async Task TooManyBytesReturnsConnectionClosedUnexpectedly()
    // {
    //     EndPoint ep = NetworkUtility.GetLocalEndPoint();
    //     using Task<SingleConnectionMessageReceiver.ListenResult> listen = NetworkUtility.ReceiveMessagesAsync(ep, CancellationToken);
    //     var client = new SingleConnectionMessageReceiver(await NetworkUtility.CreateConnectedSocket(ep));
    //
    //     const int value = 96192;
    //     byte[] valueBytes = BitConverter.GetBytes(value);
    //     int valueLength = valueBytes.Length;
    //     byte[] message = new byte[valueLength + 1];
    //     Buffer.BlockCopy(valueBytes, 0, message, 0, valueLength);
    //
    //     await clientRpcSocket.SendAsync(message, valueLength);
    //     clientRpcSocket.Close();
    //
    //     SingleConnectionMessageReceiver.ListenResult serverResult = await serverTask;
    //     Log(serverResult);
    //     Assert.AreEqual(ListenReturnCode.ConnectionClosedUnexpectedly, serverResult.ReturnCode);
    //     Assert.AreEqual(1, serverResult.Messages.Count);
    // }

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