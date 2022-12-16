﻿using System.Diagnostics;
using JetBrains.Annotations;
using MsbRpc.Serialization.Exceptions;
using MsbRpc.Serialization.Primitives;

namespace MsbRpc.Serialization.Buffers;

public static class BufferExtensions {
    [PublicAPI]
    public static ArraySegment<byte> GetSubSegment(this ArraySegment<byte> target, int count) => new(target.Array!, target.Offset, count);

    [PublicAPI]
    public static ArraySegment<byte> GetOffsetSubSegment
        (this ArraySegment<byte> target, int offset)
        => target.GetOffsetSubSegment(offset, target.Count - offset);

    [PublicAPI]
    public static ArraySegment<byte> GetOffsetSubSegment(this ArraySegment<byte> target, int offset, int count)
    {
        Debug.Assert(offset > 0);
        Debug.Assert(count > 0);
        Debug.Assert(target.Array != null);
        target.AssertContains(offset, count);
        return new ArraySegment<byte>(target.Array!, target.Offset + offset, count);
    }

    public static ArraySegment<byte> CopySubSegment(this ArraySegment<byte> source, int count) => source.CopyOffsetSubSegment(0, count);

    public static ArraySegment<byte> CopyOffsetSubSegment(this ArraySegment<byte> source, int sourceOffset, int count)
    {
        var ret = new ArraySegment<byte>(new byte[count], 0, count);
        Buffer.BlockCopy(source.Array!, source.Offset + sourceOffset, ret.Array!, 0, count);
        return ret;
    }

    public static string CreateContentString(this ArraySegment<byte> target)
    {
        byte[] array = target.Array!;
        return array.CreateContentString(target.Offset, target.Count);
    }

    public static BufferReader CreateReader(this ArraySegment<byte> target) => new(target);

    public static BufferWriter CreateWriter(this ArraySegment<byte> target) => new(target);
    private static int Offset(this ArraySegment<byte> target, int offset) => target.Offset + offset;

    #region Boolean

    public static bool ReadBoolean(this ArraySegment<byte> target, int offset = 0)
    {
        target.AssertContains(PrimitiveSerializer.BooleanSize, offset);
        return target.Array!.ReadBoolean(target.Offset(offset));
    }

    public static void WriteBoolean(this ArraySegment<byte> target, bool value, int offset = 0)
    {
        target.AssertContains(PrimitiveSerializer.BooleanSize, offset);
        target.Array!.WriteBoolean(value, target.Offset(offset));
    }

    #endregion

    #region Byte

    public static byte ReadByte(this ArraySegment<byte> target, int offset = 0)
    {
        target.AssertContains(PrimitiveSerializer.ByteSize, offset);
        return target.Array!.ReadByte(target.Offset(offset));
    }

    public static void WriteByte(this ArraySegment<byte> target, byte value, int offset = 0)
    {
        target.AssertContains(PrimitiveSerializer.ByteSize, offset);
        target.Array!.WriteByte(value, target.Offset(offset));
    }

    #endregion

    #region SByte

    public static sbyte ReadSByte(this ArraySegment<byte> target, int offset = 0)
    {
        target.AssertContains(PrimitiveSerializer.SByteSize, offset);
        return target.Array!.ReadSByte(target.Offset(offset));
    }

    public static void WriteSByte(this ArraySegment<byte> target, sbyte value, int offset = 0)
    {
        target.AssertContains(PrimitiveSerializer.SByteSize, offset);
        target.Array!.WriteSByte(value, target.Offset(offset));
    }

    #endregion

    #region Char

    public static char ReadChar(this ArraySegment<byte> target, int offset = 0)
    {
        target.AssertContains(PrimitiveSerializer.CharSize, offset);
        return target.Array!.ReadChar(target.Offset(offset));
    }

    public static void WriteChar(this ArraySegment<byte> target, char value, int offset = 0)
    {
        target.AssertContains(PrimitiveSerializer.CharSize, offset);
        target.Array!.WriteChar(value, target.Offset(offset));
    }

    #endregion

    #region Decimal

    public static decimal ReadDecimal(this ArraySegment<byte> target, int offset = 0)
    {
        target.AssertContains(PrimitiveSerializer.DecimalSize, offset);
        return target.Array!.ReadDecimal(target.Offset(offset));
    }

    public static void WriteDecimal(this ArraySegment<byte> target, decimal value, int offset = 0)
    {
        target.AssertContains(PrimitiveSerializer.DecimalSize, offset);
        target.Array!.WriteDecimal(value, target.Offset(offset));
    }

    #endregion

    #region Double

    public static double ReadDouble(this ArraySegment<byte> target, int offset = 0)
    {
        target.AssertContains(PrimitiveSerializer.DoubleSize, offset);
        return target.Array!.ReadDouble(target.Offset(offset));
    }

    public static void WriteDouble(this ArraySegment<byte> target, double value, int offset = 0)
    {
        target.AssertContains(PrimitiveSerializer.DoubleSize, offset);
        target.Array!.WriteDouble(value, target.Offset(offset));
    }

    #endregion

    #region Single

    public static float ReadSingle(this ArraySegment<byte> target, int offset = 0)
    {
        target.AssertContains(PrimitiveSerializer.SingleSize, offset);
        return target.Array!.ReadSingle(target.Offset(offset));
    }

    public static void WriteSingle(this ArraySegment<byte> target, float value, int offset = 0)
    {
        target.AssertContains(PrimitiveSerializer.SingleSize, offset);
        target.Array!.WriteSingle(value, target.Offset(offset));
    }

    #endregion

    #region Int32

    public static int ReadInt32(this ArraySegment<byte> target, int offset = 0)
    {
        target.AssertContains(PrimitiveSerializer.Int32Size, offset);
        return target.Array!.ReadInt32(target.Offset(offset));
    }

    public static void WriteInt32(this ArraySegment<byte> target, int value, int offset = 0)
    {
        target.AssertContains(PrimitiveSerializer.Int32Size, offset);
        target.Array!.WriteInt32(value, target.Offset(offset));
    }

    #endregion

    #region Uint32

    public static uint ReadUInt32(this ArraySegment<byte> target, int offset = 0)
    {
        target.AssertContains(PrimitiveSerializer.UInt32Size, offset);
        return target.Array!.ReadUInt32(target.Offset(offset));
    }

    public static void WriteUInt32(this ArraySegment<byte> target, uint value, int offset = 0)
    {
        target.AssertContains(PrimitiveSerializer.UInt32Size, offset);
        target.Array!.WriteUInt32(value, target.Offset(offset));
    }

    #endregion

    #region Int64

    public static long ReadInt64(this ArraySegment<byte> target, int offset = 0)
    {
        target.AssertContains(PrimitiveSerializer.Int64Size, offset);
        return target.Array!.ReadInt64(target.Offset(offset));
    }

    public static void WriteInt64(this ArraySegment<byte> target, long value, int offset = 0)
    {
        target.AssertContains(PrimitiveSerializer.Int64Size, offset);
        target.Array!.WriteInt64(value, target.Offset(offset));
    }

    #endregion

    #region UInt64

    public static ulong ReadUInt64(this ArraySegment<byte> target, int offset = 0)
    {
        target.AssertContains(PrimitiveSerializer.UInt64Size, offset);
        return target.Array!.ReadUInt64(target.Offset(offset));
    }

    public static void WriteUInt64(this ArraySegment<byte> target, ulong value, int offset = 0)
    {
        target.AssertContains(PrimitiveSerializer.UInt64Size, offset);
        target.Array!.WriteUInt64(value, target.Offset(offset));
    }

    #endregion

    #region Int16

    public static short ReadInt16(this ArraySegment<byte> target, int offset = 0)
    {
        target.AssertContains(PrimitiveSerializer.Int16Size, offset);
        return target.Array!.ReadInt16(target.Offset(offset));
    }

    public static void WriteInt16(this ArraySegment<byte> target, short value, int offset = 0)
    {
        target.AssertContains(PrimitiveSerializer.Int16Size, offset);
        target.Array!.WriteInt16(value, target.Offset(offset));
    }

    #endregion

    #region UInt16

    public static ushort ReadUInt16(this ArraySegment<byte> target, int offset = 0)
    {
        target.AssertContains(PrimitiveSerializer.UInt16Size, offset);
        return target.Array!.ReadUInt16(target.Offset(offset));
    }

    public static void WriteUInt16(this ArraySegment<byte> target, ushort value, int offset = 0)
    {
        target.AssertContains(PrimitiveSerializer.UInt16Size, offset);
        target.Array!.WriteUInt16(value, target.Offset(offset));
    }

    #endregion

    #region Assertions

    /// <exception cref="ByteArraySegmentHasNoArrayException"></exception>
    /// <exception cref="OutOfByteArraySegmentBoundsException"></exception>
    [AssertionMethod]
    [Conditional("DEBUG")]
    private static void AssertContains(this ArraySegment<byte> target, int count, int offset)
    {
        target.AssertHasArray();
        if (offset + count > target.Count)
        {
            throw new OutOfByteArraySegmentBoundsException(target, offset, count);
        }
    }

    /// <exception cref="ByteArraySegmentHasNoArrayException"></exception>
    [AssertionMethod]
    [Conditional("DEBUG")]
    private static void AssertHasArray(this ArraySegment<byte> target)
    {
        if (target.Array == null)
        {
            throw new ByteArraySegmentHasNoArrayException(nameof(target));
        }
    }

    #endregion
}