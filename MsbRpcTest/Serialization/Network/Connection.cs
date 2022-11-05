using System.Net;
using System.Net.Sockets;
using JetBrains.Annotations;

namespace MsbRpcTest.Serialization.Network;

public readonly struct Connection
{
    [PublicAPI] public Socket ServerSocket { get; private init; }
    [PublicAPI] public Socket ClientSocket { get; private init; }

    public Connection(Socket serverSocket, Socket clientSocket)
    {
        ServerSocket = serverSocket;
        ClientSocket = clientSocket;
    }

    public static async Task<Connection> ConnectAsync
        (CancellationToken cancellationToken)
        => await ConnectAsync(NetworkUtility.GetLocalEndPoint(), cancellationToken);

    [PublicAPI]
    public static async Task<Connection> ConnectAsync(EndPoint serverEndpoint, CancellationToken cancellationToken)
    {
        Task<Socket> acceptClient = NetworkUtility.AcceptAsync(serverEndpoint, cancellationToken);
        return new Connection
        {
            ClientSocket = await NetworkUtility.ConnectAsync(serverEndpoint, cancellationToken),
            ServerSocket = await acceptClient
        };
    }
}