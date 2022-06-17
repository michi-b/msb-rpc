using System.Net.Sockets;
using MsbRpc.Messaging;

namespace MsbRpcTest.Serialization.Network;

public class TestSocketWrapper : SocketWrapper
{
    public TestSocketWrapper(AddressFamily addressFamily, int capacity = DefaultCapacity) : base(addressFamily, capacity) { }

    public TestSocketWrapper(Socket socket, int capacity = DefaultCapacity) : base(socket, capacity) { }

    public readonly struct ListenResult
    {
        public ListenReturnCode ReturnCode { get; init; }
        public List<byte[]> Messages { get; init; }
    }

    public async Task<ListenResult> ListenAsync(CancellationToken cancellationToken)
    {
        List<byte[]> messages = new();

        ListenReturnCode returnCode = await ListenAsync
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
            ReturnCode = returnCode
        };
    }

    public async Task SendAsync(byte[] bytes)
    {
        int count = bytes.Length;
        Reserve(count);
        for (int offset = 0; offset < count; offset++)
        {
            WriteByte(bytes[offset], offset);
        }

        await SendMessageAsync(count);
    }
}