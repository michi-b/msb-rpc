#region

using MsbRpc.Serialization.Buffers;
using MsbRpc.Serialization.Primitives;
using MsbRpc.Servers.Listeners.Connections.Generic;

#endregion

namespace MsbRpc.Servers.Listeners.Connections;

public class IntIdentifiedConnectionRequest : ConnectionRequest<int>
{
    public const int MessageSize = MessageSizeWithoutId + PrimitiveSerializer.IntSize;
    protected override int IdSize => PrimitiveSerializer.IntSize;

    public IntIdentifiedConnectionRequest(Message message) : base(message, bufferReader => bufferReader.ReadInt()) { }
    public IntIdentifiedConnectionRequest(int id) : base(id) { }
    public IntIdentifiedConnectionRequest() { }

    protected override void Write(BufferWriter bufferWriter, int id)
    {
        bufferWriter.Write(id);
    }
}