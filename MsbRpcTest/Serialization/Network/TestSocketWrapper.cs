using System.Net.Sockets;
using MsbRpc.Messaging;

namespace MsbRpcTest.Serialization.Network;

public class TestSocketWrapper : SocketWrapper
{
    public TestSocketWrapper(AddressFamily addressFamily, int capacity = DefaultCapacity) : base(addressFamily, capacity) { }

    public TestSocketWrapper(Socket socket, int capacity = DefaultCapacity) : base(socket, capacity) { }

    public readonly struct ListenResult
    {
        public ListenForMessagesReturnCode ForMessagesReturnCode { get; init; }
        public List<byte[]> Messages { get; init; }
    }

    public async Task<ListenResult> ListenAsync(CancellationToken cancellationToken)
    {
        List<byte[]> messages = new();

        ListenForMessagesReturnCode forMessagesReturnCode = await ListenForMessagesAsync
        (
            count =>
            {
                byte[] buffer = new byte[count];
                for (int offset = 0; offset < count; offset++)
                {
                    buffer[offset] = ReadByte(offset);
                }

                messages.Add(buffer);
            },
            cancellationToken
        );

        return new ListenResult
        {
            Messages = messages,
            ForMessagesReturnCode = forMessagesReturnCode
        };
    }

    public async Task SendAsync(byte[] bytes) => await SendAsync(bytes, 0, bytes.Length);

    public async Task SendAsync(byte[] bytes, int offset, int count)
    {
        Reserve(count);

        int end = offset + count;

        for (int currentByteOffset = offset; currentByteOffset < end; currentByteOffset++)
        {
            WriteByte(bytes[currentByteOffset], currentByteOffset);
        }

        await SendMessageAsync(count);
    }
}