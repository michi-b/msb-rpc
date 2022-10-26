using System.CodeDom.Compiler;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MsbRpc.Messaging;
using MsbRpc.Serialization.Primitives;
using MsbRpcTest.Serialization.Network.Listeners;
using MessageList = System.Collections.Generic.List<System.ArraySegment<byte>>;

namespace MsbRpcTest.Serialization.Network;

public abstract class ListenerTest : Test
{
    protected abstract SingleConnectionListener.GetReceiveMessagesTask CreateGetReceiveMessagesTask();

    protected async Task TestClosingConnectionStopsListening()
    {
        SingleConnectionListener server = CreateSingleConnectionListener(CancellationToken);
        using (Messenger client = await server.Connect())
        {
            //do nothing, just close connection
        }

        MessageList messages = await server.ListenTask;
        Assert.AreEqual(0, messages.Count);
    }

    protected async Task TestSingleByteMessagesIsDelivered()
    {
        const byte value = 123;

        SingleConnectionListener server = CreateSingleConnectionListener(CancellationToken);
        using (Messenger client = await server.Connect())
        {
            await client.SendMessageAsync(new[] { value }, CancellationToken);
        }

        MessageList messages = await server.ListenTask;

        Log(messages);

        Assert.IsTrue(messages.Count == 1);
        Assert.AreEqual(value, messages[0][0]);
    }

    protected async Task TestEmptyMessageIsDelivered()
    {
        SingleConnectionListener server = CreateSingleConnectionListener(CancellationToken);
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

    protected async Task TestIntMessageIsDelivered()
    {
        const int value = -912347287;

        SingleConnectionListener server = CreateSingleConnectionListener(CancellationToken);

        using (Messenger client = await server.Connect())
        {
            byte[] messageOut = new byte[PrimitiveSerializer.Int32Size];
            messageOut.WriteInt32(value);
            await client.SendMessageAsync(messageOut);
        }

        MessageList messagesIn = await server.ListenTask;
        Log(messagesIn);

        Assert.AreEqual(1, messagesIn.Count);

        int valueReceived = messagesIn[0].Array!.ReadInt32();

        Assert.AreEqual(value, valueReceived);
    }

    protected async Task TestMultipleIntMessagesAreDelivered()
    {
        int[] values = { -912347287, 1, 0, -1, int.MaxValue, int.MinValue };

        SingleConnectionListener server = CreateSingleConnectionListener(CancellationToken);

        using (Messenger client = await server.Connect())
        {
            byte[] message = new byte[PrimitiveSerializer.Int32Size];
            foreach (int value in values)
            {
                message.WriteInt32(value);
                await client.SendMessageAsync(message);
            }
        }

        MessageList messagesIn = await server.ListenTask;
        Log(messagesIn);

        Assert.AreEqual(values.Length, messagesIn.Count);

        for (int i = 0; i < messagesIn.Count; i++)
        {
            ArraySegment<byte> message = messagesIn[i];
            int value = values[i];
            int valueReceived = message.Array!.ReadInt32(message.Offset);
            Assert.AreEqual(value, valueReceived);
        }
    }

    private SingleConnectionListener CreateSingleConnectionListener
        (CancellationToken cancellationToken) =>
        new(CreateGetReceiveMessagesTask(), cancellationToken);

    private static void Log(IReadOnlyList<ArraySegment<byte>> messages)
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