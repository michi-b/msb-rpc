using System.CodeDom.Compiler;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MsbRpc.Messaging;
using MsbRpc.Serialization.Buffers;
using MsbRpc.Serialization.Primitives;
using MsbRpc.Test.Base.Generic;
using MsbRpc.Test.Network.Utility;
using MessageList = System.Collections.Generic.List<System.ArraySegment<byte>>;

namespace MsbRpc.Test.Network;

public abstract class ListenerTest : Test<ListenerTest>
{
    protected abstract Task<List<ArraySegment<byte>>> Listen(Messenger messenger);

    protected async Task TestClosingConnectionStopsListening()
    {
        CancellationToken cancellationToken = CancellationToken;
        (Messenger client, Task<MessageList> listen) = await Setup(cancellationToken);
        client.Dispose();
        MessageList messages = await listen;
        Assert.AreEqual(0, messages.Count);
    }

    protected async Task TestSingleByteMessagesIsDelivered()
    {
        const byte value = 123;

        Message message = BufferUtility.CreateMessage(PrimitiveSerializer.ByteSize);

        message.Buffer.WriteByte(value);

        CancellationToken cancellationToken = CancellationToken;

        (Messenger client, Task<MessageList> listen) = await Setup(cancellationToken);
        using (client)
        {
            await client.SendAsync(message);
        }

        MessageList messages = await listen;

        Log(messages);

        Assert.IsTrue(messages.Count == 1);
        ArraySegment<byte> arraySegment = messages[0];
        Assert.AreEqual(value, arraySegment.ElementAt(0));
    }

    protected async Task TestEmptyMessageIsDelivered()
    {
        CancellationToken cancellationToken = CancellationToken;

        (Messenger client, Task<MessageList> listen) = await Setup(cancellationToken);

        using (client)
        {
            await client.SendAsync(Message.Empty);
        }

        MessageList messages = await listen;

        Log(messages);

        Assert.IsTrue(messages.Count == 1);
        Assert.IsTrue(messages[0].Count == 0);
    }

    protected async Task TestIntMessageIsDelivered()
    {
        const int value = -912347287;

        CancellationToken cancellationToken = CancellationToken;

        Message message = BufferUtility.CreateMessage(PrimitiveSerializer.IntSize);
        message.Buffer.WriteInt(value);

        (Messenger client, Task<MessageList> listen) = await Setup(cancellationToken);
        using (client)
        {
            await client.SendAsync(message);
        }

        MessageList messagesIn = await listen;
        Log(messagesIn);

        Assert.AreEqual(1, messagesIn.Count);

        int valueReceived = messagesIn[0].Array!.ReadInt();

        Assert.AreEqual(value, valueReceived);
    }

    protected async Task TestMultipleIntMessagesAreDelivered()
    {
        int[] values = { -912347287, 1, 0, -1, int.MaxValue, int.MinValue };

        CancellationToken cancellationToken = CancellationToken;

        (Messenger client, Task<MessageList> listen) = await Setup(cancellationToken);

        using (client)
        {
            Message message = BufferUtility.CreateMessage(PrimitiveSerializer.IntSize);
            ArraySegment<byte> messageBuffer = message.Buffer;
            foreach (int value in values)
            {
                messageBuffer.WriteInt(value);
                await client.SendAsync(message);
            }
        }

        MessageList messagesIn = await listen;
        Log(messagesIn);

        Assert.AreEqual(values.Length, messagesIn.Count);

        for (int i = 0; i < messagesIn.Count; i++)
        {
            ArraySegment<byte> message = messagesIn[i];
            int value = values[i];
            int valueReceived = message.Array!.ReadInt(message.Offset);
            Assert.AreEqual(value, valueReceived);
        }
    }

    private async Task<(Messenger client, Task<List<ArraySegment<byte>>> listen)> Setup(CancellationToken cancellationToken)
    {
        LocalConnection connection = await LocalConnection.ConnectAsync(cancellationToken);
        Messenger client = connection.CreateClientMessenger();
        Messenger server = connection.CreateServerMessenger();
        return (client, Listen(server));
    }

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
            string[] bytes = message.Select(b => b.ToString()).ToArray();
            writer.WriteLine($"{{{string.Join(", ", bytes)}}}");
            writer.Indent--;
        }

        writer.Indent--;

        writer.Indent--;
    }
}