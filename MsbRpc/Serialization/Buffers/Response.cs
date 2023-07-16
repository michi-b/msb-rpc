#region

using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using MsbRpc.Serialization.Primitives;

#endregion

namespace MsbRpc.Serialization.Buffers;

public readonly struct Response
{
    public const int Offset = Message.Offset + PrimitiveSerializer.ByteSize;

    public readonly ArraySegment<byte> Buffer;

    public readonly ResponseFlags Flags;

    public Response(Message message)
    {
        Debug.Assert(message.Length >= Offset - Message.Offset);
        byte[] allBytes = message.Buffer.Array!;
        Buffer = new ArraySegment<byte>(allBytes, Offset, message.Length + Message.Offset - Offset);
        Flags = (ResponseFlags)allBytes.ReadByte(Message.Offset);
    }

    public Response(byte[] bytes, int count, ResponseFlags flags)
    {
        Debug.Assert(bytes.Length >= count + Offset);
        Buffer = new ArraySegment<byte>(bytes, Offset, count);
        bytes.WriteInt(count + PrimitiveSerializer.BoolSize);
        bytes.WriteByte((byte)flags, Message.Offset);
        Flags = flags;
    }

    public int Length => Buffer.Count;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public BufferReader GetReader() => new(Buffer);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public BufferWriter GetWriter() => new(Buffer);
}