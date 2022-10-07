using System.Net;
using System.Net.Sockets;
using MsbRpc.Concurrent;
using MsbRpc.Messaging;
using MsbRpc.Messaging.Messenger;
using MsbRpcTest.Serialization.Network.Listeners;

namespace MsbRpcTest.Serialization.Network;

public static class NetworkUtility
{
    public const int DefaultBufferSize = Messenger.DefaultCapacity;

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

    public static async Task<Socket> CreateConnectedSocket(EndPoint ep)
    {
        Socket socket = CreateSocket();
        await socket.ConnectAsync(ep);
        return socket;
    }

    public static async Task<byte[]> ReceiveBytesAsync
        (EndPoint ep, CancellationToken cancellationToken) =>
        await new BytesListener(await AcceptAsync(ep, cancellationToken)).Listen(cancellationToken);

    public static async Task<MessagesListener.ListenResult> ReceiveMessagesAsync
        (EndPoint ep, CancellationToken cancellationToken) =>
        await new MessagesListener(new Messenger(await AcceptAsync(ep, cancellationToken))).ListenAsync(cancellationToken);

    private static EndPoint GetLocalEndPoint(int port) => new IPEndPoint(LocalHost, port);

    private static async Task<Socket> AcceptAsync(EndPoint ep, CancellationToken cancellationToken)
    {
        Socket listenSocket = CreateSocket();
        listenSocket.Bind(ep);
        listenSocket.Listen(1);
        return await listenSocket.AcceptAsync(cancellationToken);
    }
}