using System.Net;
using MsbRpc.Messaging;

namespace MsbRpcTest.Serialization.Network.Listeners;

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