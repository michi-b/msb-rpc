using System;
using System.Diagnostics;
using MsbRpc.Serialization.Buffers;
using MsbRpc.Serialization.Primitives;

namespace MsbRpc.Servers.Listener;

[DebuggerDisplay("{DebuggerDisplay,nq}")]
public readonly struct InitialConnectionMessage
{
    public const int MaxSize = PrimitiveSerializer.IntSize + PrimitiveSerializer.ByteSize;
    public const int MessageMaxSize = Message.Offset + MaxSize;

    public ConnectionType ConnectionType { get; }
    public int? Id { get; }

    public InitialConnectionMessage(ArraySegment<byte> messageBuffer)
    {
        Debug.Assert(messageBuffer.Count <= MessageMaxSize);

        BufferReader bufferReader = new(messageBuffer);

        ConnectionType = bufferReader.ReadByte() switch
        {
            0 => ConnectionType.UnIdentified,
            1 => ConnectionType.Identified,
            _ => throw new ArgumentOutOfRangeException()
        };

        Id = ConnectionType switch
        {
            ConnectionType.UnIdentified => null,
            ConnectionType.Identified => bufferReader.ReadInt(),
            _ => throw new ArgumentOutOfRangeException()
        };
    }

    public string DebuggerDisplay => ToString();

    public override string ToString()
    {
        string idString = Id.HasValue ? $"ID : {Id.Value}" : "no ID";

        return $"'{ConnectionType}, {idString}'";
    }
}