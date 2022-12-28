using System.Net.Sockets;
using MsbRpc.Messaging;
using MsbRpc.Sockets;

namespace MsbRpc.Test.Network.Utility;

public class LocalConnection : IDisposable
{
    public RpcSocket Server { get; }
    public RpcSocket Client { get; }

    private LocalConnection(RpcSocket client, RpcSocket server)
    {
        Client = client;
        Server = server;
    }

    public static async Task<LocalConnection> ConnectAsync(CancellationToken cancellationToken)
    {
        Socket clientSocket;
        Socket serverSocket;

        using (var listener = await EphemeralListener.CreateAsync(cancellationToken))
        {
            ValueTask<Socket> acceptClientTask = listener.AcceptAsync(cancellationToken);

            clientSocket = new Socket(listener.EndPoint.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
            await clientSocket.ConnectAsync(listener.EndPoint, cancellationToken);

            serverSocket = await acceptClientTask;
        }

        return new LocalConnection(new RpcSocket(clientSocket), new RpcSocket(serverSocket));
    }

    public Messenger CreateServerMessenger() => new(Server);
    public Messenger CreateClientMessenger() => new(Client);

    public void Dispose()
    {
        Client.Dispose();
        Server.Dispose();
    }
}