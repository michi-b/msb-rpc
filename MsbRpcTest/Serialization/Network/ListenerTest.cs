using System.CodeDom.Compiler;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MsbRpc.Messaging;
using MsbRpc.Serialization.Buffer;
using MsbRpc.Serialization.Primitives;
using MsbRpcTest.Serialization.Network.Utility;
using MessageList = System.Collections.Generic.List<System.ArraySegment<byte>>;

namespace MsbRpcTest.Serialization.Network;

public abstract class ListenerTest : Test
{
    protected abstract Task<List<ArraySegment<byte>>> ListenAsync(Messenger messenger, CancellationToken cancellationToken);

    protected async Task TestClosingConnectionStopsListening()
    {
        CancellationToken cancellationToken = CancellationToken;
        (Messenger client, Task<MessageList> listen) = await Setup(cancellationToken);
        client.Dispose();
        MessageList messages = await listen;
        Assert.AreEqual(0, messages.Count);
    }

    private async Task<(Messenger client, Task<List<ArraySegment<byte>>> listen)> Setup(CancellationToken cancellationToken)
    {
        (Messenger client, Messenger server) = await Connection.ConnectMessengersAsync(cancellationToken);
        return (client, ListenAsync(server, cancellationToken));
    }

    protected async Task TestSingleByteMessagesIsDelivered()
    {
        const byte value = 123;
        ArraySegment<byte> message = BufferUtility.Create(value);
        CancellationToken cancellationToken = CancellationToken;
        
        (Messenger client, Task<MessageList> listen) = await Setup(cancellationToken);
        using (client)
        {
            await client.SendMessageAsync(message, cancellationToken);
        }

        MessageList messages = await listen;

        Log(messages);

        Assert.IsTrue(messages.Count == 1);
        Assert.AreEqual(value, messages[0][0]);
    }

    protected async Task TestEmptyMessageIsDelivered()
    {
        CancellationToken cancellationToken = CancellationToken;
        
        (Messenger client, Task<MessageList> listen) = await Setup(cancellationToken);
        using (client)
        {
            await client.SendMessageAsync(BufferUtility.Empty, cancellationToken);
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

        byte[] messageOut = new byte[PrimitiveSerializer.Int32Size];
        messageOut.WriteInt32(value);
        var messageOutSegment = new ArraySegment<byte>(messageOut);
        
        (Messenger client, Task<MessageList> listen) = await Setup(cancellationToken);
        using (client)
        {
            await client.SendMessageAsync(messageOutSegment, cancellationToken);
        }

        MessageList messagesIn = await listen;
        Log(messagesIn);

        Assert.AreEqual(1, messagesIn.Count);

        int valueReceived = messagesIn[0].Array!.ReadInt32();

        Assert.AreEqual(value, valueReceived);
    }

    protected async Task TestMultipleIntMessagesAreDelivered()
    {
        int[] values = { -912347287, 1, 0, -1, int.MaxValue, int.MinValue };

        CancellationToken cancellationToken = CancellationToken;
        
        (Messenger client, Task<MessageList> listen) = await Setup(cancellationToken);

        using (client)
        {
            byte[] message = new byte[PrimitiveSerializer.Int32Size];
            foreach (int value in values)
            {
                message.WriteInt32(value);
                var messageSegment = new ArraySegment<byte>(message);
                await client.SendMessageAsync(messageSegment, cancellationToken);
            }
        }

        MessageList messagesIn = await listen;
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