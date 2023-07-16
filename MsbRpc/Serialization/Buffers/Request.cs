#region

using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using MsbRpc.Serialization.Primitives;

#endregion

namespace MsbRpc.Serialization.Buffers;

public readonly struct Request
{
    public const int Offset = Message.Offset + PrimitiveSerializer.IntSize; //add 4 bytes for the request id

    public readonly ArraySegment<byte> Buffer;

    public readonly int ProcedureId;

    public Request(Message message)
    {
        Debug.Assert(message.Length >= Offset - Message.Offset);
        byte[] allBytes = message.Buffer.Array!;
        Buffer = new ArraySegment<byte>(allBytes, Offset, message.Length + Message.Offset - Offset);
        ProcedureId = allBytes.ReadInt(Message.Offset);
    }

    public Request(byte[] bytes, int count, int procedureId)
    {
        Debug.Assert(bytes.Length >= count + Offset);
        Buffer = new ArraySegment<byte>(bytes, Offset, count);
        bytes.WriteInt(count + PrimitiveSerializer.IntSize);
        bytes.WriteInt(procedureId, Message.Offset);
        ProcedureId = procedureId;
    }

    public int Length => Buffer.Count;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public BufferReader GetReader() => new(Buffer);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public BufferWriter GetWriter() => new(Buffer);
}