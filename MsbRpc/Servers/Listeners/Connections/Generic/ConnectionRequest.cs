#region

using System;
using System.Diagnostics;
using MsbRpc.Serialization.Buffers;
using MsbRpc.Serialization.Primitives;

#endregion

namespace MsbRpc.Servers.Listeners.Connections.Generic;

[DebuggerDisplay("{DebuggerDisplay,nq}")]
public abstract class ConnectionRequest<TId> where TId : struct
{
    protected const int MessageSizeWithoutId = PrimitiveSerializer.ByteSize + Message.Offset;
    public ConnectionRequestType ConnectionRequestType { get; }
    public TId? Id { get; }

    protected abstract int IdSize { get; }

    public string DebuggerDisplay => ToString();

    protected ConnectionRequest(Message message, Func<BufferReader, TId> readId)
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

    protected ConnectionRequest(TId id)
    {
        ConnectionRequestType = ConnectionRequestType.Identified;
        Id = id;
    }

    protected ConnectionRequest()
    {
        ConnectionRequestType = ConnectionRequestType.UnIdentified;
        Id = null;
    }

    public override string ToString()
    {
        string idString = Id.HasValue ? $"ID : {Id.Value}" : "no ID";

        return $"'{ConnectionRequestType}, {idString}'";
    }

    private int GetSize(ConnectionRequestSizeOptions options)
    {
        int size = PrimitiveSerializer.ByteSize;

        if (options.HasFlag(ConnectionRequestSizeOptions.WithId))
        {
            size += IdSize;
        }

        if (options.HasFlag(ConnectionRequestSizeOptions.WithMessageOffset))
        {
            size += Message.Offset;
        }

        return size;
    }

    public Message GetMessage(RpcBuffer buffer)
    {
        int count = GetSize(Id.HasValue ? ConnectionRequestSizeOptions.WithId : ConnectionRequestSizeOptions.None);

        Message message = buffer.GetMessage(count);

        BufferWriter bufferWriter = new(message.Buffer);

        bufferWriter.Write((byte)ConnectionRequestType);

        if (Id.HasValue)
        {
            Write(bufferWriter, Id.Value);
        }

        return message;
    }

    protected abstract void Write(BufferWriter bufferWriter, TId id);
}