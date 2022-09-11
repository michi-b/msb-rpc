using System.Net;
using System.Net.Sockets;
using MsbRpc.Concurrent;
using MsbRpc.Messaging;

namespace MsbRpcTest.Serialization.Network;

public static class NetworkUtility
{
    public const int DefaultBufferSize = SocketWrapper.DefaultCapacity;

    public static readonly IPAddress LocalHost;

    private static readonly UniqueIntProvider UniquePortProvider = MsbRpc.NetworkUtility.CreateUniquePortProvider(false);

    static NetworkUtility() => LocalHost = Dns.GetHostEntry("localhost").AddressList[0];

    public static EndPoint GetLocalEndPoint()
    {
        int port = UniquePortProvider.Get();
        Console.WriteLine($"using port {port}");
        return GetLocalEndPoint(port);
    }

    public static Socket CreateSocket() => SocketUtility.CreateTcpSocket(LocalHost.AddressFamily);

    public static async Task<TestSocketWrapper.ListenResult> ReceiveMessagesAsync(EndPoint ep, CancellationToken cancellationToken) =>
        await ReceiveMessagesAsync
        (
            ep,
            DefaultBufferSize,
            cancellationToken
        );

    public static Task<byte[]> ReceiveBufferAsync
    (
        EndPoint ep,
        CancellationToken cancellationToken
    ) =>
        ReceiveBufferAsync
        (
            ep,
            DefaultBufferSize,
            SocketUtility.DefaultSocketSendBufferSize,
            SocketUtility.DefaultSocketReceiveBufferSize,
            cancellationToken
        );

    private static EndPoint GetLocalEndPoint(int port) => new IPEndPoint(LocalHost, port);

    private static async Task<TestSocketWrapper.ListenResult> ReceiveMessagesAsync
    (
        EndPoint ep,
        int initialCapacity,
        CancellationToken cancellationToken
    )
    {
        var socket = new TestSocketWrapper(await AcceptAsync(ep, cancellationToken), initialCapacity);
        TestSocketWrapper.ListenResult ret = await socket.ListenAsync(cancellationToken);
        return ret;
    }

    private static async Task<byte[]> ReceiveBufferAsync
    (
        EndPoint ep,
        int bufferSize,
        int socketSendBufferSize,
        int socketReceiveBufferSize,
        CancellationToken cancellationToken
    )
    {
        byte[] buffer = new byte[bufferSize];
        Socket handler = await AcceptAsync(ep, cancellationToken);
        handler.SendBufferSize = socketSendBufferSize;
        handler.ReceiveBufferSize = socketReceiveBufferSize;
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

    private static async Task<Socket> AcceptAsync(EndPoint ep, CancellationToken cancellationToken)
    {
        Socket listenSocket = CreateSocket();
        listenSocket.Bind(ep);
        listenSocket.Listen(1);
        return await listenSocket.AcceptAsync(cancellationToken);
    }
}