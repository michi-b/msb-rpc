using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Net.Sockets;
using MsbRpc.Serialization.Primitives;

namespace MsbRpc;

#pragma warning disable IDE0079 // Remove unnecessary suppression
[SuppressMessage("ReSharper", "BuiltInTypeReferenceStyle")]
#pragma warning restore IDE0079 // Remove unnecessary suppression
public abstract class RpcReceiver : RpcSocket
{
    protected RpcReceiver(Socket socket, int initialBufferSize) : base(socket, initialBufferSize) { }

    public async Task ListenAsync(CancellationToken cancellationToken)
    {
        while (!cancellationToken.IsCancellationRequested)
        {
            await Receive(PrimitiveSerializer.Int32Size);
            Int32 messageSize = ReadInt32();
            await Receive(messageSize);
            ReadMessage(messageSize);
        }
    }

    protected abstract void ReadMessage(int size);
}