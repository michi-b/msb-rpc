using System.Net;
using JetBrains.Annotations;
using MsbRpc.Sockets;

namespace MsbRpcTest.Serialization.Network.Utility;

public readonly struct Connection
{
    [PublicAPI] public RpcSocket ServerSocket { get; private init; }
    [PublicAPI] public RpcSocket ClientSocket { get; private init; }

    public Connection(RpcSocket serverSocket, RpcSocket clientSocket)
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
        Task<RpcSocket> acceptClient = NetworkUtility.AcceptAsync(serverEndpoint, cancellationToken);
        return new Connection
        {
            ClientSocket = await NetworkUtility.ConnectAsync(serverEndpoint, cancellationToken),
            ServerSocket = await acceptClient
        };
    }
}