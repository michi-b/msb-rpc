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
    ///     byte size of <see cref="MessengerType" /> (byte) and <see cref="Id" /> (int32)
    /// </summary>
    private const int MaxSize = PrimitiveSerializer.ByteSize + PrimitiveSerializer.IntSize;

    public const int MessageMaxSize = Message.Offset + MaxSize;

    public MessengerType MessengerType { get; }
    public int? Id { get; }

    [MayBeUsedByGeneratedCode]
    private InitialConnectionMessage(MessengerType messengerType, int? id)
    {
        Debug.Assert(messengerType == MessengerType.UnIdentified || id.HasValue);
        MessengerType = messengerType;
        Id = id;
    }

    public static readonly InitialConnectionMessage UnIdentified = new(MessengerType.UnIdentified, null);

    public static InitialConnectionMessage CreateIdentified(int id) => new(MessengerType.Identified, id);

    public static InitialConnectionMessage Read(Message message)
    {
        ArraySegment<byte> messageBuffer = message.Buffer;

        Debug.Assert(messageBuffer.Count <= MessageMaxSize);

        BufferReader bufferReader = new(messageBuffer);

        MessengerType messengerType = bufferReader.ReadByte() switch
        {
            0 => MessengerType.UnIdentified,
            1 => MessengerType.Identified,
            _ => throw new ArgumentOutOfRangeException()
        };

        int? id = messengerType switch
        {
            MessengerType.UnIdentified => null,
            MessengerType.Identified => bufferReader.ReadInt(),
            _ => throw new ArgumentOutOfRangeException()
        };

        return new InitialConnectionMessage(messengerType, id);
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

        bufferWriter.Write((byte)MessengerType);

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

        return $"'{MessengerType}, {idString}'";
    }
}