using System.Diagnostics;
using JetBrains.Annotations;
using MsbRpc.Serialization.Exceptions;
using MsbRpc.Serialization.Primitives;

namespace MsbRpc.Serialization.Buffers;

public static class BufferExtensions
{
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

    public static string CreateContentString(this ArraySegment<byte> target)
    {
        byte[] array = target.Array!;
        return array.CreateContentString(target.Offset, target.Count);
    }

    private static ArraySegment<byte> CopyOffsetSubSegment(this ArraySegment<byte> source, int sourceOffset, int count)
    {
        var ret = new ArraySegment<byte>(new byte[count], 0, count);
        Buffer.BlockCopy(source.Array!, source.Offset + sourceOffset, ret.Array!, 0, count);
        return ret;
    }

    private static int Offset(this ArraySegment<byte> target, int offset) => target.Offset + offset;

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

    #region Sbyte

    public static sbyte ReadSbyte(this ArraySegment<byte> target, int offset = 0)
    {
        target.AssertContains(PrimitiveSerializer.SByteSize, offset);
        return target.Array!.ReadSbyte(target.Offset(offset));
    }

    public static void WriteSbyte(this ArraySegment<byte> target, sbyte value, int offset = 0)
    {
        target.AssertContains(PrimitiveSerializer.SByteSize, offset);
        target.Array!.WriteSbyte(value, target.Offset(offset));
    }

    #endregion

    #region Bool

    public static bool ReadBool(this ArraySegment<byte> target, int offset = 0)
    {
        target.AssertContains(PrimitiveSerializer.BooleanSize, offset);
        return target.Array!.ReadBool(target.Offset(offset));
    }

    public static void WriteBool(this ArraySegment<byte> target, bool value, int offset = 0)
    {
        target.AssertContains(PrimitiveSerializer.BooleanSize, offset);
        target.Array!.WriteBool(value, target.Offset(offset));
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

    #region Int

    public static int ReadInt(this ArraySegment<byte> target, int offset = 0)
    {
        target.AssertContains(PrimitiveSerializer.Int32Size, offset);
        return target.Array!.ReadInt(target.Offset(offset));
    }

    public static void WriteInt(this ArraySegment<byte> target, int value, int offset = 0)
    {
        target.AssertContains(PrimitiveSerializer.Int32Size, offset);
        target.Array!.WriteInt(value, target.Offset(offset));
    }

    #endregion

    #region Long

    public static long ReadLong(this ArraySegment<byte> target, int offset = 0)
    {
        target.AssertContains(PrimitiveSerializer.Int64Size, offset);
        return target.Array!.ReadLong(target.Offset(offset));
    }

    public static void WriteLong(this ArraySegment<byte> target, long value, int offset = 0)
    {
        target.AssertContains(PrimitiveSerializer.Int64Size, offset);
        target.Array!.WriteLong(value, target.Offset(offset));
    }

    #endregion

    #region Short

    public static short ReadShort(this ArraySegment<byte> target, int offset = 0)
    {
        target.AssertContains(PrimitiveSerializer.Int16Size, offset);
        return target.Array!.ReadShort(target.Offset(offset));
    }

    public static void WriteShort(this ArraySegment<byte> target, short value, int offset = 0)
    {
        target.AssertContains(PrimitiveSerializer.Int16Size, offset);
        target.Array!.WriteShort(value, target.Offset(offset));
    }

    #endregion

    #region Uint

    public static uint ReadUint(this ArraySegment<byte> target, int offset = 0)
    {
        target.AssertContains(PrimitiveSerializer.UInt32Size, offset);
        return target.Array!.ReadUint(target.Offset(offset));
    }

    public static void WriteUint(this ArraySegment<byte> target, uint value, int offset = 0)
    {
        target.AssertContains(PrimitiveSerializer.UInt32Size, offset);
        target.Array!.WriteUint(value, target.Offset(offset));
    }

    #endregion

    #region UInt64

    public static ulong ReadUlong(this ArraySegment<byte> target, int offset = 0)
    {
        target.AssertContains(PrimitiveSerializer.UInt64Size, offset);
        return target.Array!.ReadUlong(target.Offset(offset));
    }

    public static void WriteUlong(this ArraySegment<byte> target, ulong value, int offset = 0)
    {
        target.AssertContains(PrimitiveSerializer.UInt64Size, offset);
        target.Array!.WriteUlong(value, target.Offset(offset));
    }

    #endregion

    #region UShort

    public static ushort ReadUshort(this ArraySegment<byte> target, int offset = 0)
    {
        target.AssertContains(PrimitiveSerializer.UInt16Size, offset);
        return target.Array!.ReadUshort(target.Offset(offset));
    }

    public static void WriteUshort(this ArraySegment<byte> target, ushort value, int offset = 0)
    {
        target.AssertContains(PrimitiveSerializer.UInt16Size, offset);
        target.Array!.WriteUshort(value, target.Offset(offset));
    }

    #endregion

    #region Float

    public static float ReadFloat(this ArraySegment<byte> target, int offset = 0)
    {
        target.AssertContains(PrimitiveSerializer.SingleSize, offset);
        return target.Array!.ReadFloat(target.Offset(offset));
    }

    public static void WriteFloat(this ArraySegment<byte> target, float value, int offset = 0)
    {
        target.AssertContains(PrimitiveSerializer.SingleSize, offset);
        target.Array!.WriteFloat(value, target.Offset(offset));
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
}