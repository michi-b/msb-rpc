using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using MsbRpc.Serialization.Primitives;

namespace MsbRpc.Serialization.Buffers;

public readonly struct Response
{
    public const int Offset = Message.Offset + PrimitiveSerializer.BoolSize;

    public readonly ArraySegment<byte> Buffer;
    
    public readonly bool RanToCompletion;

    public Response(Message message)
    {
        Debug.Assert(message.Length >= Offset - Message.Offset);
        byte[] allBytes = message.Buffer.Array!;
        Buffer = new ArraySegment<byte>(allBytes, Offset, message.Length + Message.Offset - Offset);
        RanToCompletion = allBytes.ReadBool(Message.Offset);
    }

    public Response(byte[] bytes, int count, bool ranToCompletion)
    {
        Debug.Assert(bytes.Length >= count + Offset);
        Buffer = new ArraySegment<byte>(bytes, Offset, count);
        bytes.WriteInt(count + PrimitiveSerializer.BoolSize);
        bytes.WriteBool(ranToCompletion, Message.Offset);
        RanToCompletion = ranToCompletion;
    }

    public int Length => Buffer.Count;
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public BufferReader GetReader() => new(Buffer);
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public BufferWriter GetWriter() => new(Buffer);
}