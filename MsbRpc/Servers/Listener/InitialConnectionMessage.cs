using System;
using System.Diagnostics;
using MsbRpc.Attributes;
using MsbRpc.Serialization.Buffers;
using MsbRpc.Serialization.Primitives;

namespace MsbRpc.Servers.Listener;

[DebuggerDisplay("{DebuggerDisplay,nq}")]
public readonly struct InitialConnectionMessage
{
    /// <summary>
    ///     byte size of <see cref="ConnectionType" /> (byte) and <see cref="Id" /> (int32)
    /// </summary>
    private const int MaxSize = PrimitiveSerializer.ByteSize + PrimitiveSerializer.IntSize;

    public const int MessageMaxSize = Message.Offset + MaxSize;

    public ConnectionType ConnectionType { get; }
    public int? Id { get; }

    [MayBeUsedByGeneratedCode]
    private InitialConnectionMessage(ConnectionType connectionType, int? id)
    {
        Debug.Assert(connectionType == ConnectionType.UnIdentified || id.HasValue);
        ConnectionType = connectionType;
        Id = id;
    }

    public static InitialConnectionMessage CreateUnIdentified() => new(ConnectionType.UnIdentified, null);

    public static InitialConnectionMessage CreateIdentified(int id) => new(ConnectionType.Identified, id);

    public static InitialConnectionMessage Read(Message message)
    {
        ArraySegment<byte> messageBuffer = message.Buffer;

        Debug.Assert(messageBuffer.Count <= MessageMaxSize);

        BufferReader bufferReader = new(messageBuffer);

        ConnectionType connectionType = bufferReader.ReadByte() switch
        {
            0 => ConnectionType.UnIdentified,
            1 => ConnectionType.Identified,
            _ => throw new ArgumentOutOfRangeException()
        };

        int? id = connectionType switch
        {
            ConnectionType.UnIdentified => null,
            ConnectionType.Identified => bufferReader.ReadInt(),
            _ => throw new ArgumentOutOfRangeException()
        };

        return new InitialConnectionMessage(connectionType, id);
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

        bufferWriter.Write((byte)ConnectionType);

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

        return $"'{ConnectionType}, {idString}'";
    }
}