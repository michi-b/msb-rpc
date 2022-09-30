using System.Net;
using MsbRpc.Messaging.Messenger;

namespace MsbRpcTest.Serialization.Network;

using ListenTask = Task<SingleConnectionMessageReceiver.ListenResult>;

public readonly struct SingleConnectionServer
{
    public ListenTask Listen { get; }

    private EndPoint EndPoint { get; }

    public SingleConnectionServer(CancellationToken cancellationToken)
    {
        EndPoint = NetworkUtility.GetLocalEndPoint();
        Listen = NetworkUtility.ReceiveMessagesAsync(EndPoint, cancellationToken);
    }

    public async Task<Messenger> Connect() => new(await NetworkUtility.CreateConnectedSocket(EndPoint));
}