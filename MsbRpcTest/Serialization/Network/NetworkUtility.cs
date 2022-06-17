using System.Net;
using System.Net.Sockets;
using MsbRpc;
using MsbRpc.Concurrent;

namespace MsbRpcTest.Serialization.Network;

public static class NetworkUtility
{
    public const int DefaultBufferSize = RpcSocket.DefaultCapacity;

    public static readonly IPAddress LocalHost;

    public static readonly UniqueIntProvider UniquePortProvider = MsbRpc.NetworkUtility.CreateUniquePortProvider(false);

    static NetworkUtility() => LocalHost = Dns.GetHostEntry("localhost").AddressList[0];

    public static IPEndPoint GetLocalEndPoint()
    {
        int port = UniquePortProvider.Get();
        Console.WriteLine($"using port {port}");
        return GetLocalEndPoint(port);
    }

    public static IPEndPoint GetLocalEndPoint(int port) => new(LocalHost, port);

    public static Socket CreateSocket() => MsbRpc.NetworkUtility.CreateTcpSocket(LocalHost.AddressFamily);

    public static async Task<RpcTestSocket.ListenResult> ReceiveMessagesAsync
        (IPEndPoint ep, CancellationToken cancellationToken) =>
        await ReceiveMessagesAsync(ep, DefaultBufferSize, cancellationToken);

    public static async Task<RpcTestSocket.ListenResult> ReceiveMessagesAsync(IPEndPoint ep, int capacity, CancellationToken cancellationToken)
    {
        var socket = new RpcTestSocket(await AcceptAsync(ep, cancellationToken), capacity);
        return await socket.ListenAsync(cancellationToken);
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

    public static async Task<Socket> AcceptAsync(IPEndPoint ep, CancellationToken cancellationToken)
    {
        Socket listenSocket = CreateSocket();
        listenSocket.Bind(ep);
        listenSocket.Listen(1);
        return await listenSocket.AcceptAsync(cancellationToken);
    }
}