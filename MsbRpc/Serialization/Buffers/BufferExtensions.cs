#region

using System;
using System.Diagnostics;
using JetBrains.Annotations;
using MsbRpc.Serialization.Exceptions;
using MsbRpc.Serialization.Primitives;

#endregion

namespace MsbRpc.Serialization.Buffers;

public static class BufferExtensions
{
    [PublicAPI]
    public static ArraySegment<byte> GetSubSegment(this ArraySegment<byte> target, int count) => new(target.Array!, target.Offset, count);

    [PublicAPI]
    public static ArraySegment<byte> GetOffsetSubSegment(this ArraySegment<byte> target, int offset) => target.GetOffsetSubSegment(offset, target.Count - offset);

    [PublicAPI]
    public static ArraySegment<byte> GetOffsetSubSegment(this ArraySegment<byte> target, int offset, int count)
    {
        Debug.Assert(offset >= 0);
        Debug.Assert(count >= 0);
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
        target.AssertContains(PrimitiveSerializer.SbyteSize, offset);
        return target.Array!.ReadSbyte(target.Offset(offset));
    }

    public static void WriteSbyte(this ArraySegment<byte> target, sbyte value, int offset = 0)
    {
        target.AssertContains(PrimitiveSerializer.SbyteSize, offset);
        target.Array!.WriteSbyte(value, target.Offset(offset));
    }

    #endregion

    #region Bool

    public static bool ReadBool(this ArraySegment<byte> target, int offset = 0)
    {
        target.AssertContains(PrimitiveSerializer.BoolSize, offset);
        return target.Array!.ReadBool(target.Offset(offset));
    }

    public static void WriteBool(this ArraySegment<byte> target, bool value, int offset = 0)
    {
        target.AssertContains(PrimitiveSerializer.BoolSize, offset);
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
        target.AssertContains(PrimitiveSerializer.IntSize, offset);
        return target.Array!.ReadInt(target.Offset(offset));
    }

    public static void WriteInt(this ArraySegment<byte> target, int value, int offset = 0)
    {
        target.AssertContains(PrimitiveSerializer.IntSize, offset);
        target.Array!.WriteInt(value, target.Offset(offset));
    }

    #endregion

    #region Long

    public static long ReadLong(this ArraySegment<byte> target, int offset = 0)
    {
        target.AssertContains(PrimitiveSerializer.LongSize, offset);
        return target.Array!.ReadLong(target.Offset(offset));
    }

    public static void WriteLong(this ArraySegment<byte> target, long value, int offset = 0)
    {
        target.AssertContains(PrimitiveSerializer.LongSize, offset);
        target.Array!.WriteLong(value, target.Offset(offset));
    }

    #endregion

    #region Short

    public static short ReadShort(this ArraySegment<byte> target, int offset = 0)
    {
        target.AssertContains(PrimitiveSerializer.ShortSize, offset);
        return target.Array!.ReadShort(target.Offset(offset));
    }

    public static void WriteShort(this ArraySegment<byte> target, short value, int offset = 0)
    {
        target.AssertContains(PrimitiveSerializer.ShortSize, offset);
        target.Array!.WriteShort(value, target.Offset(offset));
    }

    #endregion

    #region Uint

    public static uint ReadUint(this ArraySegment<byte> target, int offset = 0)
    {
        target.AssertContains(PrimitiveSerializer.UintSize, offset);
        return target.Array!.ReadUint(target.Offset(offset));
    }

    public static void WriteUint(this ArraySegment<byte> target, uint value, int offset = 0)
    {
        target.AssertContains(PrimitiveSerializer.UintSize, offset);
        target.Array!.WriteUint(value, target.Offset(offset));
    }

    #endregion

    #region ULong

    public static ulong ReadUlong(this ArraySegment<byte> target, int offset = 0)
    {
        target.AssertContains(PrimitiveSerializer.UlongSize, offset);
        return target.Array!.ReadUlong(target.Offset(offset));
    }

    public static void WriteUlong(this ArraySegment<byte> target, ulong value, int offset = 0)
    {
        target.AssertContains(PrimitiveSerializer.UlongSize, offset);
        target.Array!.WriteUlong(value, target.Offset(offset));
    }

    #endregion

    #region UShort

    public static ushort ReadUshort(this ArraySegment<byte> target, int offset = 0)
    {
        target.AssertContains(PrimitiveSerializer.UshortSize, offset);
        return target.Array!.ReadUshort(target.Offset(offset));
    }

    public static void WriteUshort(this ArraySegment<byte> target, ushort value, int offset = 0)
    {
        target.AssertContains(PrimitiveSerializer.UshortSize, offset);
        target.Array!.WriteUshort(value, target.Offset(offset));
    }

    #endregion

    #region Float

    public static float ReadFloat(this ArraySegment<byte> target, int offset = 0)
    {
        target.AssertContains(PrimitiveSerializer.FloatSize, offset);
        return target.Array!.ReadFloat(target.Offset(offset));
    }

    public static void WriteFloat(this ArraySegment<byte> target, float value, int offset = 0)
    {
        target.AssertContains(PrimitiveSerializer.FloatSize, offset);
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

    #region NullableByte

    public static byte? ReadByteNullable(this ArraySegment<byte> target, int offset = 0)
    {
        target.AssertContains(PrimitiveSerializer.NullableByteSize, offset);
        return target.Array!.ReadByteNullable(target.Offset(offset));
    }

    public static void WriteByteNullable(this ArraySegment<byte> target, byte? value, int offset = 0)
    {
        target.AssertContains(PrimitiveSerializer.NullableByteSize, offset);
        target.Array!.WriteByteNullable(value, target.Offset(offset));
    }

    #endregion

    #region NullableSByte

    public static sbyte? ReadSbyteNullable(this ArraySegment<byte> target, int offset = 0)
    {
        target.AssertContains(PrimitiveSerializer.NullableSbyteSize, offset);
        return target.Array!.ReadSbyteNullable(target.Offset(offset));
    }

    public static void WriteSbyteNullable(this ArraySegment<byte> target, sbyte? value, int offset = 0)
    {
        target.AssertContains(PrimitiveSerializer.NullableSbyteSize, offset);
        target.Array!.WriteSbyteNullable(value, target.Offset(offset));
    }

    #endregion

    #region NullableBool

    public static bool? ReadBoolNullable(this ArraySegment<byte> target, int offset = 0)
    {
        target.AssertContains(PrimitiveSerializer.NullableBoolSize, offset);
        return target.Array!.ReadBoolNullable(target.Offset(offset));
    }

    public static void WriteBoolNullable(this ArraySegment<byte> target, bool? value, int offset = 0)
    {
        target.AssertContains(PrimitiveSerializer.NullableBoolSize, offset);
        target.Array!.WriteBoolNullable(value, target.Offset(offset));
    }

    #endregion

    #region NullableChar

    public static char? ReadCharNullable(this ArraySegment<byte> target, int offset = 0)
    {
        target.AssertContains(PrimitiveSerializer.NullableCharSize, offset);
        return target.Array!.ReadCharNullable(target.Offset(offset));
    }

    public static void WriteCharNullable(this ArraySegment<byte> target, char? value, int offset = 0)
    {
        target.AssertContains(PrimitiveSerializer.NullableCharSize, offset);
        target.Array!.WriteCharNullable(value, target.Offset(offset));
    }

    #endregion

    #region NullableInt

    public static int? ReadIntNullable(this ArraySegment<byte> target, int offset = 0)
    {
        target.AssertContains(PrimitiveSerializer.NullableIntSize, offset);
        return target.Array!.ReadIntNullable(target.Offset(offset));
    }

    public static void WriteIntNullable(this ArraySegment<byte> target, int? value, int offset = 0)
    {
        target.AssertContains(PrimitiveSerializer.NullableIntSize, offset);
        target.Array!.WriteIntNullable(value, target.Offset(offset));
    }

    #endregion

    #region NullableLong

    public static long? ReadLongNullable(this ArraySegment<byte> target, int offset = 0)
    {
        target.AssertContains(PrimitiveSerializer.NullableLongSize, offset);
        return target.Array!.ReadLongNullable(target.Offset(offset));
    }

    public static void WriteLongNullable(this ArraySegment<byte> target, long? value, int offset = 0)
    {
        target.AssertContains(PrimitiveSerializer.NullableLongSize, offset);
        target.Array!.WriteLongNullable(value, target.Offset(offset));
    }

    #endregion

    #region NullableShort

    public static short? ReadShortNullable(this ArraySegment<byte> target, int offset = 0)
    {
        target.AssertContains(PrimitiveSerializer.NullableShortSize, offset);
        return target.Array!.ReadShortNullable(target.Offset(offset));
    }

    public static void WriteShortNullable(this ArraySegment<byte> target, short? value, int offset = 0)
    {
        target.AssertContains(PrimitiveSerializer.NullableShortSize, offset);
        target.Array!.WriteShortNullable(value, target.Offset(offset));
    }

    #endregion

    #region NullableUInt

    public static uint? ReadUintNullable(this ArraySegment<byte> target, int offset = 0)
    {
        target.AssertContains(PrimitiveSerializer.NullableUintSize, offset);
        return target.Array!.ReadUintNullable(target.Offset(offset));
    }

    public static void WriteUintNullable(this ArraySegment<byte> target, uint? value, int offset = 0)
    {
        target.AssertContains(PrimitiveSerializer.NullableUintSize, offset);
        target.Array!.WriteUintNullable(value, target.Offset(offset));
    }

    #endregion

    #region NullableULong

    public static ulong? ReadUlongNullable(this ArraySegment<byte> target, int offset = 0)
    {
        target.AssertContains(PrimitiveSerializer.NullableUlongSize, offset);
        return target.Array!.ReadUlongNullable(target.Offset(offset));
    }

    public static void WriteUlongNullable(this ArraySegment<byte> target, ulong? value, int offset = 0)
    {
        target.AssertContains(PrimitiveSerializer.NullableUlongSize, offset);
        target.Array!.WriteUlongNullable(value, target.Offset(offset));
    }

    #endregion

    #region NullableUShort

    public static ushort? ReadUshortNullable(this ArraySegment<byte> target, int offset = 0)
    {
        target.AssertContains(PrimitiveSerializer.NullableUshortSize, offset);
        return target.Array!.ReadUshortNullable(target.Offset(offset));
    }

    public static void WriteUshortNullable(this ArraySegment<byte> target, ushort? value, int offset = 0)
    {
        target.AssertContains(PrimitiveSerializer.NullableUshortSize, offset);
        target.Array!.WriteUshortNullable(value, target.Offset(offset));
    }

    #endregion

    #region NullableFloat

    public static float? ReadFloatNullable(this ArraySegment<byte> target, int offset = 0)
    {
        target.AssertContains(PrimitiveSerializer.NullableFloatSize, offset);
        return target.Array!.ReadFloatNullable(target.Offset(offset));
    }

    public static void WriteFloatNullable(this ArraySegment<byte> target, float? value, int offset = 0)
    {
        target.AssertContains(PrimitiveSerializer.NullableFloatSize, offset);
        target.Array!.WriteFloatNullable(value, target.Offset(offset));
    }

    #endregion

    #region NullableDouble

    public static double? ReadDoubleNullable(this ArraySegment<byte> target, int offset = 0)
    {
        target.AssertContains(PrimitiveSerializer.NullableDoubleSize, offset);
        return target.Array!.ReadDoubleNullable(target.Offset(offset));
    }

    public static void WriteDoubleNullable(this ArraySegment<byte> target, double? value, int offset = 0)
    {
        target.AssertContains(PrimitiveSerializer.NullableDoubleSize, offset);
        target.Array!.WriteDoubleNullable(value, target.Offset(offset));
    }

    #endregion

    #region NullableDecimal

    public static decimal? ReadDecimalNullable(this ArraySegment<byte> target, int offset = 0)
    {
        target.AssertContains(PrimitiveSerializer.NullableDecimalSize, offset);
        return target.Array!.ReadDecimalNullable(target.Offset(offset));
    }

    public static void WriteDecimalNullable(this ArraySegment<byte> target, decimal? value, int offset = 0)
    {
        target.AssertContains(PrimitiveSerializer.NullableDecimalSize, offset);
        target.Array!.WriteDecimalNullable(value, target.Offset(offset));
    }

    #endregion
}