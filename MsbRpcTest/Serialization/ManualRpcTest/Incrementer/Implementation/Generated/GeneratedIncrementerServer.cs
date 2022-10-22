using System.Net.Sockets;
using MsbRpc.Messaging;
using MsbRpc.Messaging.Listeners;
using MsbRpc.Serialization.Primitives;

namespace MsbRpcTest.Serialization.ManualRpcTest.Incrementer.Implementation.Generated;

public abstract class GeneratedIncrementerServer : IIncrementerServer
{
    public const int IncrementProcedureId = 0;

    private readonly Task<Listener.ReturnCode> _listenTask;

    /// <remarks>
    ///     strictly only use on the same thread that the listener runs on,
    ///     like when the ReceiveMessage() callback method is invoked,
    ///     because the serializer is not thread-safe (which is why it is a instance and not a static class in the first place)
    /// </remarks>
    /// >
    private readonly PrimitiveSerializer _serializer;

    protected GeneratedIncrementerServer(Socket connectedSocket, CancellationToken cancellationToken)
    {
        var messenger = new Messenger(connectedSocket);
        var listener = new ActiveListener(messenger, ReceiveMessage);
        _serializer = new PrimitiveSerializer();
        _listenTask = listener.Listen(cancellationToken);
    }

    public abstract int Increment();

    public int Increment(int value) => throw new NotImplementedException();

    private void ReceiveMessage(ArraySegment<byte> bytes)
    {
        // int procedureId = PrimitiveSerializer.ReadInt32()
    }
}