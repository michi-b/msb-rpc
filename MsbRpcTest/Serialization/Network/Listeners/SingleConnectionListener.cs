using System.Net;
using JetBrains.Annotations;
using MsbRpc.Messaging;

namespace MsbRpcTest.Serialization.Network.Listeners;

public readonly struct SingleConnectionListener
{
    [PublicAPI] public Task<List<ArraySegment<byte>>> ListenTask { get; }

    private EndPoint EndPoint { get; }

    public delegate Task<List<ArraySegment<byte>>> GetReceiveMessagesTask
    (
        EndPoint ep,
        CancellationToken cancellationToken
    );

    public SingleConnectionListener(GetReceiveMessagesTask getReceiveMessagesTask, CancellationToken cancellationToken)
    {
        EndPoint = NetworkUtility.GetLocalEndPoint();
        ListenTask = getReceiveMessagesTask(EndPoint, cancellationToken);
    }

    public async Task<Messenger> Connect() => new(await NetworkUtility.CreateConnectedSocket(EndPoint));
}