using System.Net;
using MsbRpc.Messaging.Messenger;
using MsbRpcTest.Serialization.Network.Listeners;

namespace MsbRpcTest.Serialization.Network;

using ListenTask = Task<MessagesListener.ListenResult>;

public readonly struct SingleConnectionServer
{
    public ListenTask ListenTask { get; }

    private EndPoint EndPoint { get; }

    public SingleConnectionServer(CancellationToken cancellationToken)
    {
        EndPoint = NetworkUtility.GetLocalEndPoint();
        ListenTask = NetworkUtility.ReceiveMessagesAsync(EndPoint, cancellationToken);
    }

    public async Task<Messenger> Connect() => new(await NetworkUtility.CreateConnectedSocket(EndPoint));
}