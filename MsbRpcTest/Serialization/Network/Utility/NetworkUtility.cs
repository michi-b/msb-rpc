using System.Net;
using System.Net.Sockets;
using MsbRpc.Concurrent;
using MsbRpc.Network;
using MsbRpc.Sockets;

namespace MsbRpcTest.Serialization.Network.Utility;

public static class NetworkUtility
{
    public const int DefaultBufferSize = SocketUtility.DefaultSocketReceiveBufferSize;
    private static readonly IPAddress LocalHost;
    private static readonly UniqueIntProvider UniquePortProvider = MsbRpc.Network.NetworkUtility.CreateUniquePortProvider(false);

    static NetworkUtility() => LocalHost = Dns.GetHostEntry("localhost").AddressList[0];

    public static EndPoint GetLocalEndPoint()
    {
        int port = UniquePortProvider.Get();
        Console.WriteLine($"using port {port}");
        return GetLocalEndPoint(port);
    }

    public static Socket CreateSocket() => SocketUtility.CreateTcpSocket(LocalHost.AddressFamily);

    public static async Task<RpcSocket> ConnectAsync(EndPoint ep, CancellationToken cancellationToken)
    {
        Socket socket = CreateSocket();
        await socket.ConnectAsync(ep, cancellationToken);
        return new RpcSocket(socket);
    }

    public static async Task<RpcSocket> AcceptAsync(EndPoint ep, CancellationToken cancellationToken)
    {
        Socket listenSocket = CreateSocket();
        listenSocket.Bind(ep);
        listenSocket.Listen(1);
        return new RpcSocket(await listenSocket.AcceptAsync(cancellationToken));
    }

    private static EndPoint GetLocalEndPoint(int port) => new IPEndPoint(LocalHost, port);
}