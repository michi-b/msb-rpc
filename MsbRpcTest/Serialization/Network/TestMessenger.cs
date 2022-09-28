using System.Net.Sockets;
using JetBrains.Annotations;
using MsbRpc.Messaging.Messenger;

namespace MsbRpcTest.Serialization.Network;

public class TestMessenger : Messenger
{
    public enum ListenReturnCode
    {
        ConnectionClosed = 1,
        ConnectionClosedUnexpectedly = 2
    }

    public TestMessenger(Socket connectedSocket, int capacity = DefaultCapacity) : base(connectedSocket, capacity) { }

    public readonly struct ListenResult
    {
        public ListenReturnCode ReturnCode { get; init; }
        public List<byte[]> Messages { get; init; }
    }

    public async Task<ListenResult> ListenAsync(CancellationToken cancellationToken, int timeout = 1000)
    {
        List<byte[]> messages = new();

        ReceiveMessageReturnCode lastReceiveMessageReturnCode;
        do
        {
            ReceiveMessageResult result = await ReceiveMessageAsync();
            lastReceiveMessageReturnCode = result.MessageReturnCode;
            if (lastReceiveMessageReturnCode == ReceiveMessageReturnCode.Success)
            {
                messages.Add(result.Bytes);
            }
        } while (lastReceiveMessageReturnCode == ReceiveMessageReturnCode.Success);

        return new ListenResult
        {
            Messages = messages,
            ReturnCode = lastReceiveMessageReturnCode switch
            {
                ReceiveMessageReturnCode.Success => throw new InvalidOperationException(),
                ReceiveMessageReturnCode.ConnectionClosed => ListenReturnCode.ConnectionClosed,
                ReceiveMessageReturnCode.ConnectionClosedUnexpectedly => ListenReturnCode.ConnectionClosedUnexpectedly,
                _ => throw new ArgumentOutOfRangeException()
            }
        };
    }

    [PublicAPI]
    public async Task<SendMessageReturnCode> SendAsync(ArraySegment<byte> bytes)
    {
        int count = bytes.Count;
        Reserve(count);
        WriteBytes(bytes);
        return await SendMessageAsync(count);
    }

    private void WriteBytes(ArraySegment<byte> bytes)
    {
        for (int i = 0; i < bytes.Count; i++)
        {
            WriteByte(bytes[i], i);
        }
    }
}