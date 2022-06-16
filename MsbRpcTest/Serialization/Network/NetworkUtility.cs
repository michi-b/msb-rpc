using System.Net;
using System.Net.Sockets;

namespace MsbRpcTest.Serialization.Network;

public static class NetworkUtility
{
    public const int DefaultBufferSize = 1024;

    public static readonly IPAddress LocalHost;

    static NetworkUtility() => LocalHost = Dns.GetHostEntry("localhost").AddressList[0];

    public static Task<byte[]> ReceiveBufferAsync
    (
        IPEndPoint ep,
        CancellationToken cancellationToken
    ) =>
        ReceiveBufferAsync(ep, DefaultBufferSize, cancellationToken);

    public static async Task<byte[]> ReceiveBufferAsync
    (
        IPEndPoint ep,
        int bufferSize,
        CancellationToken cancellationToken
    )
    {
        byte[] buffer = new byte[bufferSize];
        var serverSocket = new Socket(ep.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
        serverSocket.Bind(ep);
        serverSocket.Listen(1);
        Socket handler = await serverSocket.AcceptAsync(cancellationToken);
        int position = 0;
        while (true)
        {
            ArraySegment<byte> targetSegment = new(buffer, position, bufferSize - position);

            int bytesRead = await handler.ReceiveAsync
            (
                targetSegment,
                SocketFlags.None,
                cancellationToken
            );

            if (bytesRead == 0)
            {
                break;
            }

            position += bytesRead;
        }

        return buffer;
    }
}