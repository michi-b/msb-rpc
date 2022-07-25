using System.Collections.Concurrent;
using System.Net.Sockets;
using MsbRpc.Messaging;

namespace MsbRpcTest.Serialization.Network;

public class TestSocketWrapper : SocketWrapper
{
    private readonly ConcurrentQueue<ArraySegment<byte>> _messages = new();

    public TestSocketWrapper(AddressFamily addressFamily, int capacity = DefaultCapacity) : base(addressFamily, capacity) { }

    public TestSocketWrapper(Socket socket, int capacity = DefaultCapacity) : base(socket, capacity) { }

    public readonly struct ListenResult
    {
        public ListenReturnCode ReturnCode { get; init; }
        public List<byte[]> Messages { get; init; }
    }

    public async Task<ListenResult> ListenAsync(CancellationToken cancellationToken)
    {
        Task<ListenReturnCode> listen = Listen();
        List<byte[]> messages = new();

        while (!cancellationToken.IsCancellationRequested && listen.Status == TaskStatus.Running || IsMessageAvailable)
        {
            messages.Add(await AwaitNextMessage(cancellationToken));
        }

        return new ListenResult
        {
            Messages = messages,
            ReturnCode = await listen
        };
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