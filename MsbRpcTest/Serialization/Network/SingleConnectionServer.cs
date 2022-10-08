using System.Net;
using MsbRpc.Messaging;
using MsbRpc.Messaging.Listeners;
using MsbRpcTest.Serialization.Network.Listeners;

namespace MsbRpcTest.Serialization.Network;

using ListenTask = Task<Listener.ReturnCode>;

public readonly struct SingleConnectionServer
{
    public Task<List<ArraySegment<byte>>> ListenTask { get; }

    private EndPoint EndPoint { get; }

    public SingleConnectionServer(CancellationToken cancellationToken)
    {
        EndPoint = NetworkUtility.GetLocalEndPoint();
        ListenTask = NetworkUtility.ReceiveMessagesAsync(EndPoint, cancellationToken);
    }

    public async Task<Messenger> Connect() => new(await NetworkUtility.CreateConnectedSocket(EndPoint));
}