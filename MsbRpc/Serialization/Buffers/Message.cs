using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using MsbRpc.Serialization.Primitives;

namespace MsbRpc.Serialization.Buffers;

public readonly struct Message
{
    public const int Offset = PrimitiveSerializer.IntSize;

    public readonly ArraySegment<byte> Buffer;

    public static readonly Message Empty = new(new byte[Offset], 0);

    public Message(Request request) : this(request.Buffer.Array!, request.Buffer.Count + Request.Offset - Offset) { }

    public Message(byte[] bytes, int count)
    {
        Debug.Assert(bytes.Length >= count + Offset);
        Buffer = new ArraySegment<byte>(bytes, Offset, count);
        bytes.WriteInt(count);
    }

    public int Length => Buffer.Count;

    public BufferReader GetReader() => new(Buffer);

    public BufferWriter GetWriter() => new(Buffer);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public ArraySegment<byte> GetFullMessageBuffer() => new(Buffer.Array!, 0, Buffer.Count + Offset);
}