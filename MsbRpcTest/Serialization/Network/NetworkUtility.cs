using System.Net;
using System.Net.Sockets;
using JetBrains.Annotations;

namespace MsbRpcTest.Serialization.Network;

public static class NetworkUtility
{
    private const int Port = 19269;

    public const int DefaultBufferSize = 1024;

    public static readonly IPEndPoint LocalHost;

    static NetworkUtility()
    {
        IPAddress localHostAddress = Dns.GetHostEntry("localhost").AddressList[0];
        LocalHost = new IPEndPoint(localHostAddress, Port);
    }

    public static async Task<byte[]> ReceiveBufferAsync
    (
        CancellationToken cancellationToken,
        IPEndPoint? endPoint = default,
        int bufferSize = DefaultBufferSize
    )
    {
        endPoint ??= LocalHost;

        byte[] buffer = new byte[bufferSize];
        var serverSocket = new Socket(endPoint.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
        serverSocket.Bind(endPoint);
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