using System.Net;
using System.Net.Sockets;
using MsbRpc.Concurrent;
using MsbRpc.Messaging;
using MsbRpc.Messaging.Messenger;


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
    
   private static EndPoint GetLocalEndPoint(int port) => new IPEndPoint(LocalHost, port);

    public static async Task<SingleConnectionMessageReceiver.ListenResult> ReceiveMessagesAsync
    (
        EndPoint ep,
        CancellationToken cancellationToken
    )
    {
        var socket = new SingleConnectionMessageReceiver(new Messenger(await AcceptAsync(ep, cancellationToken)));
        SingleConnectionMessageReceiver.ListenResult ret = await socket.ListenAsync(cancellationToken);
        return ret;
    }

    private static async Task<Socket> AcceptAsync(EndPoint ep, CancellationToken cancellationToken)
    {
        Socket listenSocket = CreateSocket();
        listenSocket.Bind(ep);
        listenSocket.Listen(1);
        return await listenSocket.AcceptAsync(cancellationToken);
    }
}