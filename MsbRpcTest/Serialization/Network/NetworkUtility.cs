using System.Net;
using System.Net.Sockets;

namespace MsbRpcTest.Serialization.Network;

public static class NetworkUtility
{
    public const int DefaultBufferSize = 1024;

    public static readonly IPAddress LocalHost;

    static NetworkUtility() => LocalHost = Dns.GetHostEntry("localhost").AddressList[0];

    public static IPEndPoint GetLocalEndPoint(int port) => new(LocalHost, port);

    public static async Task<Socket> AcceptAsync(IPEndPoint ep, CancellationToken cancellationToken)
    {
        var listenSocket = new Socket(ep.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
        listenSocket.Bind(ep);
        listenSocket.Listen(1);
        return await listenSocket.AcceptAsync(cancellationToken);
    }

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
        Socket handler = await AcceptAsync(ep, cancellationToken);
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