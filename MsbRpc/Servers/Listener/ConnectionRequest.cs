#region

using System;
using System.Diagnostics;
using MsbRpc.Serialization.Buffers;
using MsbRpc.Serialization.Primitives;

#endregion

namespace MsbRpc.Servers.Listener;

[DebuggerDisplay("{DebuggerDisplay,nq}")]
public abstract class ConnectionRequest<TId> where TId : struct
{
    public ConnectionRequestType ConnectionRequestType { get; }
    public TId? Id { get; }

    public ConnectionRequest(Message message, Func<BufferReader, TId> readId)
    {
        ArraySegment<byte> messageBuffer = message.Buffer;

        BufferReader bufferReader = new(messageBuffer);

        ConnectionRequestType connectionRequestType = bufferReader.ReadByte() switch
        {
            0 => ConnectionRequestType.UnIdentified,
            1 => ConnectionRequestType.Identified,
            _ => throw new ArgumentOutOfRangeException()
        };
        
        Id = connectionRequestType switch
        {
            ConnectionRequestType.UnIdentified => null,
            ConnectionRequestType.Identified => readId(bufferReader),
            _ => throw new ArgumentOutOfRangeException()
        };
    }
    
    public ConnectionRequest(TId id)
    {
        ConnectionRequestType = ConnectionRequestType.Identified;
        Id = id;
    }

    public ConnectionRequest()
    {
        ConnectionRequestType = ConnectionRequestType.UnIdentified;
        Id = null;
    }

    protected abstract TId ReadId(BufferReader bufferReader);

    protected abstract int IdSize { get; }

    public Message WriteMessage(RpcBuffer buffer, Action<BufferWriter, TId> writeId)
    {
        int count = PrimitiveSerializer.ByteSize;

        if (Id.HasValue)
        {
            count += IdSize;
        }

        Message message = buffer.GetMessage(count);

        BufferWriter bufferWriter = new(message.Buffer);

        bufferWriter.Write((byte)ConnectionRequestType);

        if (Id.HasValue)
        {
            writeId(bufferWriter, Id.Value);
        }

        return message;
    }

    public string DebuggerDisplay => ToString();

    public override string ToString()
    {
        string idString = Id.HasValue ? $"ID : {Id.Value}" : "no ID";

        return $"'{ConnectionRequestType}, {idString}'";
    }
}