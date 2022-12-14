using System.Net;
using System.Net.Sockets;
using MsbRpc.Messaging;
using MsbRpc.Sockets;

namespace MsbRpcTest.Serialization.Network.Utility;

public readonly struct Connection
{
    private RpcSocket Server { get; init; }
    private RpcSocket Client { get; init; }

    public static async Task<Connection> ConnectAsync(CancellationToken cancellationToken)
    {
        (IPEndPoint endPoint, Task<RpcSocket> acceptClient) = NetworkUtility.AcceptAsync(cancellationToken);
        
        Console.WriteLine("Using port {0}", endPoint.Port);
        
        return new Connection
        {
            Client = await NetworkUtility.ConnectAsync(endPoint, cancellationToken),
            Server = await acceptClient
        };
    }

    public static async Task<(RpcSocket client, RpcSocket server)> ConnectSocketsAsync(CancellationToken cancellationToken)
        => (await ConnectAsync(cancellationToken)).Tuple;

    public static async Task<(Messenger client, Messenger server)> ConnectMessengersAsync(CancellationToken cancellationToken)
        => (await ConnectAsync(cancellationToken)).AsMessengers;

    private (RpcSocket client, RpcSocket server) Tuple => (Client, Server);

    public (Messenger client, Messenger server) AsMessengers => (new Messenger(Client), new Messenger(Server));
}