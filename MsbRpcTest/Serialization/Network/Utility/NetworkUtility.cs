using System.Net;
using System.Net.Sockets;
using MsbRpc.Network;
using MsbRpc.Sockets;

namespace MsbRpcTest.Serialization.Network.Utility;

public static class NetworkUtility
{
    public const int DefaultBufferSize = SocketUtility.DefaultSocketReceiveBufferSize;
    private static readonly IPAddress LocalHost;

    static NetworkUtility() => LocalHost = Dns.GetHostEntry("localhost").AddressList[0];

    public static Socket CreateSocket() => SocketUtility.CreateTcpSocket(LocalHost.AddressFamily);

    public static (IPEndPoint endPoint, Task<RpcSocket>) AcceptAsync(CancellationToken cancellationToken)
    {
        Socket socket = SocketUtility.CreateTcpSocket(LocalHost.AddressFamily);
        socket.Bind(new IPEndPoint(LocalHost, 0));
        return new ValueTuple<IPEndPoint, Task<RpcSocket>>((IPEndPoint)socket.LocalEndPoint, AcceptAsync(socket, cancellationToken));
    }

    public static async Task<RpcSocket> ConnectAsync(IPEndPoint endPoint, CancellationToken cancellationToken)
    {
        Socket socket = SocketUtility.CreateTcpSocket(endPoint.AddressFamily);
        await socket.ConnectAsync(endPoint, cancellationToken);
        return new RpcSocket(socket);
    }

    private static async Task<RpcSocket> AcceptAsync
        (Socket socket, CancellationToken cancellationToken)
        => new(await socket.AcceptAsync(cancellationToken));
}