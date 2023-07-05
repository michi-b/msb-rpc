using System;
using System.Diagnostics;
using MsbRpc.Attributes;
using MsbRpc.Serialization.Buffers;
using MsbRpc.Serialization.Primitives;

namespace MsbRpc.Servers.Listener;

[DebuggerDisplay("{DebuggerDisplay,nq}")]
public readonly struct ConnectionRequest
{
    /// <summary>
    ///     byte size of <see cref="ConnectionRequestType" /> (byte) and <see cref="Id" /> (int32)
    /// </summary>
    private const int MaxSize = PrimitiveSerializer.ByteSize + PrimitiveSerializer.IntSize;

    public const int MessageMaxSize = Message.Offset + MaxSize;

    public ConnectionRequestType ConnectionRequestType { get; }
    public int? Id { get; }

    [MayBeUsedByGeneratedCode]
    private ConnectionRequest(ConnectionRequestType connectionRequestType, int? id)
    {
        Debug.Assert(connectionRequestType == ConnectionRequestType.UnIdentified || id.HasValue);
        ConnectionRequestType = connectionRequestType;
        Id = id;
    }

    public static readonly ConnectionRequest UnIdentified = new(ConnectionRequestType.UnIdentified, null);

    public static ConnectionRequest CreateIdentified(int id) => new(ConnectionRequestType.Identified, id);

    public static ConnectionRequest Read(Message message)
    {
        ArraySegment<byte> messageBuffer = message.Buffer;

        Debug.Assert(messageBuffer.Count <= MessageMaxSize);

        BufferReader bufferReader = new(messageBuffer);

        ConnectionRequestType connectionRequestType = bufferReader.ReadByte() switch
        {
            0 => ConnectionRequestType.UnIdentified,
            1 => ConnectionRequestType.Identified,
            _ => throw new ArgumentOutOfRangeException()
        };

        int? id = connectionRequestType switch
        {
            ConnectionRequestType.UnIdentified => null,
            ConnectionRequestType.Identified => bufferReader.ReadInt(),
            _ => throw new ArgumentOutOfRangeException()
        };

        return new ConnectionRequest(connectionRequestType, id);
    }

    public Message Write(RpcBuffer buffer)
    {
        int count = PrimitiveSerializer.ByteSize;

        if (Id.HasValue)
        {
            count += PrimitiveSerializer.IntSize;
        }

        Message message = buffer.GetMessage(count);

        BufferWriter bufferWriter = new(message.Buffer);

        bufferWriter.Write((byte)ConnectionRequestType);

        if (Id.HasValue)
        {
            bufferWriter.Write(Id.Value);
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