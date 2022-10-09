using System.Net;
using MsbRpc.Messaging;
using MsbRpc.Messaging.Listeners;
using MsbRpcTest.Serialization.Network.Listeners;

namespace MsbRpcTest.Serialization.Network;

using ListenTask = Task<Listener.ReturnCode>;

public readonly struct LazySingleConnectionMessageListener
{
    public Task<List<ArraySegment<byte>>> ListenTask { get; }

    private EndPoint EndPoint { get; }

    public LazySingleConnectionMessageListener(CancellationToken cancellationToken)
    {
        EndPoint = NetworkUtility.GetLocalEndPoint();
        ListenTask = NetworkUtility.ReceiveMessagesLazyAsync(EndPoint, cancellationToken);
    }

    public async Task<Messenger> Connect() => new(await NetworkUtility.CreateConnectedSocket(EndPoint));
}