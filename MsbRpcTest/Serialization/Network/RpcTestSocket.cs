using System.Net.Sockets;
using MsbRpc;

namespace MsbRpcTest.Serialization.Network;

public class RpcTestSocket : RpcSocket
{
    public RpcTestSocket(Socket socket, int initialBufferSize) : base(socket, initialBufferSize) { }

    public async Task<List<byte[]>> ListenAsync(CancellationToken cancellationToken)
    {
        List<byte[]> messages = new();

        await ListenAsync(count =>
        {
            byte[] buffer = new byte[count];
            for (int offset = 0; offset < count; offset++)
            {
                buffer[offset] = ReadByte(offset);
            }
            messages.Add(buffer);
        }, cancellationToken);

        return messages;
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