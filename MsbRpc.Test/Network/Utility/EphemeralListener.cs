using System.Net;
using System.Net.Sockets;
using JetBrains.Annotations;

namespace MsbRpc.Test.Network.Utility;

public class EphemeralListener : IDisposable
{
    [PublicAPI] public IPEndPoint EndPoint { get; }
    private Socket ListenSocket { get; }

    public EphemeralListener(IPEndPoint endPoint, Socket listenSocket)
    {
        EndPoint = endPoint;
        ListenSocket = listenSocket;
    }

    public static async ValueTask<EphemeralListener> CreateAsync(CancellationToken cancellationToken) => await CreateAsync(1, cancellationToken);

    public async ValueTask<Socket> AcceptAsync(CancellationToken cancellationToken) => await ListenSocket.AcceptAsync(cancellationToken);

    public void Dispose()
    {
        GC.SuppressFinalize(this);
        ListenSocket.Dispose();
    }

    private static async ValueTask<EphemeralListener> CreateAsync(int backlogSize, CancellationToken cancellationToken)
    {
        IPAddress localHost = await NetworkUtility.GetLocalHostAsync(cancellationToken);

        var listenSocket = new Socket(localHost.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
        listenSocket.Bind(new IPEndPoint(localHost, 0));
        var listenEndPoint = (IPEndPoint)listenSocket.LocalEndPoint!;
        Console.WriteLine($"using port {listenEndPoint.Port}");
        listenSocket.Listen(backlogSize);

        return new EphemeralListener(listenEndPoint, listenSocket);
    }
}