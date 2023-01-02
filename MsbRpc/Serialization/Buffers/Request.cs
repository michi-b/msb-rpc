using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using MsbRpc.Serialization.Primitives;

namespace MsbRpc.Serialization.Buffers;

public readonly struct Request
{
    public const int Offset = PrimitiveSerializer.IntSize * 2;

    public readonly ArraySegment<byte> Buffer;

    public readonly int ProcedureId;

    public Request(Message message)
    {
        Debug.Assert(message.Length >= Offset);
        Buffer = message.Buffer;
        ProcedureId = Buffer.Array!.ReadInt(Message.Offset);
    }
    
    public Request(byte[] bytes, int count, int procedureId)
    {
        Debug.Assert(bytes.Length >= count + Offset);
        Buffer = new ArraySegment<byte>(bytes, Offset, count);
        bytes.WriteInt(count + PrimitiveSerializer.IntSize);
        bytes.WriteInt(procedureId, PrimitiveSerializer.IntSize);
        ProcedureId = procedureId;
    }
    
    public int Length => Buffer.Count;

    public BufferReader GetReader() => new BufferReader(Buffer);

    public BufferWriter GetWriter() => new BufferWriter(Buffer);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public ArraySegment<byte> GetFullMessageBuffer() => new(Buffer.Array!, 0, Buffer.Count + Offset);
}