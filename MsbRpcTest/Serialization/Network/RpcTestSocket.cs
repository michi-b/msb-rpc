using System.Net.Sockets;
using MsbRpc;

namespace MsbRpcTest.Serialization.Network;

public class RpcTestSocket : RpcSocket
{
    public RpcTestSocket(AddressFamily addressFamily, int capacity = DefaultCapacity) : base(addressFamily, capacity) { }

    public RpcTestSocket(Socket socket, int capacity = DefaultCapacity) : base(socket, capacity) { }

    public readonly struct ListenResult
    {
        public ListenReturnType ReturnType { get; init; }
        public List<byte[]> Messages { get; init; }
    }

    public async Task<ListenResult> ListenAsync(CancellationToken cancellationToken)
    {
        List<byte[]> messages = new();

        ListenReturnType returnType = await ListenAsync
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
            ReturnType = returnType
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

        await SendAsync(count);
    }
}