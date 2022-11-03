using System.Diagnostics;
using System.Net;
using System.Net.Sockets;
using MsbRpc.Concurrent;
using MsbRpc.Extensions;
using MsbRpc.Messaging;
using MsbRpc.Messaging.Listeners;
using MsbRpc.Network;
using MsbRpcTest.Serialization.Network.Listeners;

namespace MsbRpcTest.Serialization.Network;

public static class NetworkUtility
{
    public const int DefaultBufferSize = ActiveListener.DefaultBufferSize;
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

    public static async Task<Socket> ConnectAsync(EndPoint ep, CancellationToken cancellationToken)
    {
        Socket socket = CreateSocket();
        await socket.ConnectAsync(ep, cancellationToken);
        return socket;
    }

    public static async Task<byte[]> ReceiveBytesAsync
        (EndPoint ep, CancellationToken cancellationToken)
        => await new BytesListener(await AcceptAsync(ep, cancellationToken)).Listen(cancellationToken);

    public static async Task<List<ArraySegment<byte>>> ReceiveMessagesActiveAsync
        (EndPoint ep, CancellationToken cancellationToken)
        => await ReceiveMessagesActiveAsync(ep, DefaultBufferSize, cancellationToken);

    public static async Task<List<ArraySegment<byte>>> ReceiveMessagesActiveAsync(EndPoint ep, int bufferSize, CancellationToken cancellationToken)
    {
        List<ArraySegment<byte>> result = new();
        var canReceiveNextMessageEvent = new AutoResetEvent(true);
        var listener = new ActiveListener
        (
            new Messenger(await AcceptAsync(ep, cancellationToken)),
            arb =>
            {
                Debug.Assert(!canReceiveNextMessageEvent.WaitOne(0));
                result.Add(arb.Copy());
                canReceiveNextMessageEvent.Set();
            },
            canReceiveNextMessageEvent,
            bufferSize
        );
        await listener.Listen(cancellationToken);
        return result;
    }

    public static async Task<List<ArraySegment<byte>>> ReceiveMessagesLazyAsync(EndPoint ep, CancellationToken cancellationToken)
    {
        var listener = new LazyListener(new Messenger(await AcceptAsync(ep, cancellationToken)));
        await listener.Listen(cancellationToken);
        List<ArraySegment<byte>> result = new();
        while (listener.HasMessageAvailable)
        {
            result.Add(listener.ConsumeNextMessage());
        }

        return result;
    }

    public static async Task<Socket> AcceptAsync(EndPoint ep, CancellationToken cancellationToken)
    {
        Socket listenSocket = CreateSocket();
        listenSocket.Bind(ep);
        listenSocket.Listen(1);
        return await listenSocket.AcceptAsync(cancellationToken);
    }

    private static EndPoint GetLocalEndPoint(int port) => new IPEndPoint(LocalHost, port);
}