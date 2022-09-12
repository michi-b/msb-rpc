using System.Collections.Concurrent;
using System.Diagnostics;
using System.Net.Sockets;
using MsbRpc.Messaging;
using MsbRpc.Messaging.Sockets;

namespace MsbRpcTest.Serialization.Network;

public class TestSocketWrapper : SocketWrapper
{
    public enum ListenReturnCode
    {
        ConnectionClosed = 1,
        ConnectionClosedUnexpectedly = 2,
        Timout = 3
    }

    private readonly ConcurrentQueue<ArraySegment<byte>> _messages = new();

    public TestSocketWrapper(AddressFamily addressFamily, int capacity = DefaultCapacity) : base(addressFamily, capacity) { }

    public TestSocketWrapper(Socket socket, int capacity = DefaultCapacity) : base(socket, capacity) { }

    public readonly struct ListenResult
    {
        public ListenReturnCode ReturnCode { get; init; }
        public List<byte[]> Messages { get; init; }
    }

    public async Task<ListenResult> ListenAsync(CancellationToken cancellationToken, int timeout = 1000)
    {
        Task<MsbRpc.Messaging.ListenReturnCode> listen = Listen();
        List<byte[]> messages = new();

        using var cts = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);
        try
        {
            cts.CancelAfter(timeout);

            CancellationToken ct = cts.Token;

            while (!listen.IsCompleted || IsMessageAvailable)
            {
                messages.Add(await AwaitNextMessage(cancellationToken));
            }

            return new ListenResult
            {
                Messages = messages,
                ReturnCode = (ListenReturnCode)await listen
            };
        }
        catch (OperationCanceledException e)
        {
            Debug.Assert(e.CancellationToken == cts.Token);
            return new ListenResult
            {
                Messages = messages,
                ReturnCode = ListenReturnCode.Timout
            };
        }
    }

    public async Task<SendMessageReturnCode> SendAsync(ArraySegment<byte> bytes)
    {
        int count = bytes.Count;
        Reserve(count);
        WriteBytes(bytes);
        return await SendMessageAsync(count);
    }

    public async Task<SendMessageReturnCode> SendAsync(ArraySegment<byte> bytes, int indicatedLength)
    {
        int count = bytes.Count;
        Reserve(count);
        WriteBytes(bytes);
        return await SendMessageAsync(indicatedLength, count);
    }

    private void WriteBytes(ArraySegment<byte> bytes)
    {
        for (int i = 0; i < bytes.Count; i++)
        {
            WriteByte(bytes[i], i);
        }
    }
}