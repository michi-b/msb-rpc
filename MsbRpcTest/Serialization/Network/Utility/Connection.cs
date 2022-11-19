using System.Net;
using MsbRpc.Messaging;
using MsbRpc.Sockets;

namespace MsbRpcTest.Serialization.Network.Utility;

public readonly struct Connection
{
    private RpcSocket Server { get; init; }
    private RpcSocket Client { get; init; }

    public static async Task<Connection> ConnectAsync
        (CancellationToken cancellationToken)
        => await ConnectAsync(NetworkUtility.GetLocalEndPoint(), cancellationToken);

    private static async Task<Connection> ConnectAsync(EndPoint serverEndpoint, CancellationToken cancellationToken)
    {
        Task<RpcSocket> acceptClient = NetworkUtility.AcceptAsync(serverEndpoint, cancellationToken);
        return new Connection
        {
            Client = await NetworkUtility.ConnectAsync(serverEndpoint, cancellationToken),
            Server = await acceptClient
        };
    }

    public static async Task<(RpcSocket client, RpcSocket server)> ConnectSocketsAsync(CancellationToken cancellationToken)
        => (await ConnectAsync(NetworkUtility.GetLocalEndPoint(), cancellationToken)).Tuple;

    public static async Task<(Messenger client, Messenger server)> ConnectMessengersAsync(CancellationToken cancellationToken)
        => (await ConnectAsync(NetworkUtility.GetLocalEndPoint(), cancellationToken)).AsMessengers;

    public (RpcSocket client, RpcSocket server) Tuple => (Client, Server);

    public (Messenger client, Messenger server) AsMessengers => (new Messenger(Client), new Messenger(Server));
}