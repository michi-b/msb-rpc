#region

using System;
using JetBrains.Annotations;

#endregion

namespace MsbRpc.Serialization.Primitives;

public static partial class PrimitiveSerializer
{
    private const int NullableSizeOverhead = 1;

    /// <summary>
    ///     shortcut access for <see cref="BitConverter" />.IsLittleEndian
    /// </summary>
    private static readonly bool IsLittleEndian = BitConverter.IsLittleEndian;

    public static int GetSize(PrimitiveType primitiveType)
    {
        return primitiveType switch
        {
            PrimitiveType.Boolean => BoolSize,
            PrimitiveType.Byte => ByteSize,
            PrimitiveType.Char => CharSize,
            PrimitiveType.Decimal => DecimalSize,
            PrimitiveType.Double => DoubleSize,
            PrimitiveType.Int16 => ShortSize,
            PrimitiveType.Int32 => IntSize,
            PrimitiveType.Int64 => LongSize,
            PrimitiveType.SByte => SbyteSize,
            PrimitiveType.Single => FloatSize,
            PrimitiveType.UInt16 => UshortSize,
            PrimitiveType.UInt32 => UintSize,
            PrimitiveType.UInt64 => UlongSize,
            _ => throw new ArgumentOutOfRangeException(nameof(primitiveType), primitiveType, null)
        };
    }

    private static void WriteNullable<T>(this byte[] buffer, T? value, int offset, Action<byte[], T, int> writeNonNullable)
        where T : struct
    {
        if (value == null)
        {
            buffer[offset] = 0;
            return;
        }

        buffer[offset] = 1;
        writeNonNullable(buffer, value.Value, offset + 1);
    }

    private static T? ReadNullable<T>(this byte[] buffer, int offset, Func<byte[], int, T> readNonNullable)
        where T : struct
    {
        if (buffer[offset] == 0)
        {
            return null;
        }

        return readNonNullable(buffer, offset + 1);
    }

    #region Byte

    public static void WriteByte(this byte[] buffer, byte value, int offset = 0) => buffer[offset] = value;

    public static byte ReadByte(this byte[] buffer, int offset = 0) => buffer[offset];

    [PublicAPI] public const int ByteSize = 1;

    #endregion

    #region SByte

    public static void WriteSbyte(this byte[] buffer, sbyte value, int offset = 0) => buffer[offset] = (byte)value;

    public static sbyte ReadSbyte(this byte[] buffer, int offset = 0) => (sbyte)buffer[offset];

    [PublicAPI] public const int SbyteSize = 1;

    #endregion

    #region Bool

    public static void WriteBool(this byte[] buffer, bool value, int offset = 0) => buffer[offset] = value ? (byte)1 : (byte)0;

    public static bool ReadBool(this byte[] buffer, int offset = 0) => buffer[offset] == 1;

    [PublicAPI] public const int BoolSize = 1;

    #endregion

    #region Char

    public static void WriteChar(this byte[] buffer, char value, int offset = 0)
    {
        var converter2 = new ByteConverter2(value);
        converter2.Write(buffer, offset);
    }

    public static char ReadChar(this byte[] buffer, int offset = 0) => BitConverter.ToChar(buffer, offset);

    [PublicAPI] public const int CharSize = 2;

    #endregion

    #region Int

    public static void WriteInt(this byte[] buffer, int value, int offset = 0)
    {
        var converter4 = new ByteConverter4(value);
        converter4.Write(buffer, offset);
    }

    public static int ReadInt(this byte[] buffer, int offset = 0) => BitConverter.ToInt32(buffer, offset);

    [PublicAPI] public const int IntSize = 4;

    #endregion

    #region Long

    public static void WriteLong(this byte[] buffer, long value, int offset = 0)
    {
        var converter8 = new ByteConverter8(value);
        converter8.Write(buffer, offset);
    }

    public static long ReadLong(this byte[] buffer, int offset = 0) => BitConverter.ToInt64(buffer, offset);

    [PublicAPI] public const int LongSize = 8;

    #endregion

    #region Short

    public static void WriteShort(this byte[] buffer, short value, int offset = 0)
    {
        var converter2 = new ByteConverter2(value);
        converter2.Write(buffer, offset);
    }

    public static short ReadShort(this byte[] buffer, int offset = 0) => BitConverter.ToInt16(buffer, offset);

    [PublicAPI] public const int ShortSize = 2;

    #endregion

    #region UInt

    public static void WriteUint(this byte[] buffer, uint value, int offset = 0)
    {
        var converter4 = new ByteConverter4(value);
        converter4.Write(buffer, offset);
    }

    public static uint ReadUint(this byte[] buffer, int offset = 0) => BitConverter.ToUInt32(buffer, offset);

    [PublicAPI] public const int UintSize = 4;

    #endregion

    #region ULong

    public static void WriteUlong(this byte[] buffer, ulong value, int offset = 0)
    {
        var converter8 = new ByteConverter8(value);
        converter8.Write(buffer, offset);
    }

    public static ulong ReadUlong(this byte[] buffer, int offset = 0) => BitConverter.ToUInt64(buffer, offset);

    [PublicAPI] public const int UlongSize = 8;

    #endregion

    #region UShort

    public static void WriteUshort(this byte[] buffer, ushort value, int offset = 0)
    {
        var converter2 = new ByteConverter2(value);
        converter2.Write(buffer, offset);
    }

    public static ushort ReadUshort(this byte[] buffer, int offset = 0) => BitConverter.ToUInt16(buffer, offset);

    [PublicAPI] public const int UshortSize = 2;

    #endregion

    #region Float

    public static void WriteFloat(this byte[] buffer, float value, int offset = 0)
    {
        var converter4 = new ByteConverter4(value);
        converter4.Write(buffer, offset);
    }

    public static float ReadFloat(this byte[] buffer, int offset = 0) => BitConverter.ToSingle(buffer, offset);

    [PublicAPI] public const int FloatSize = 8;

    #endregion

    #region Double

    public static void WriteDouble(this byte[] buffer, double value, int offset = 0)
    {
        var converter8 = new ByteConverter8(value);
        converter8.Write(buffer, offset);
    }

    public static double ReadDouble(this byte[] buffer, int offset = 0) => BitConverter.ToDouble(buffer, offset);

    public const int DoubleSize = 8;

    #endregion

    #region Decimal

    public static void WriteDecimal(this byte[] buffer, decimal value, int offset = 0)
    {
        var converter16 = new ByteConverter16(value);
        converter16.Write(buffer, offset);
    }

    public static decimal ReadDecimal(this byte[] buffer, int offset = 0)
    {
        var converter16 = new ByteConverter16(buffer, offset);
        return converter16.Read();
    }

    [PublicAPI] public const int DecimalSize = 16;

    #endregion

    #region Byte?

    public static void WriteByteNullable(this byte[] buffer, byte? value, int offset = 0)
    {
        buffer.WriteNullable(value, offset, WriteByte);
    }

    public static byte? ReadByteNullable(this byte[] buffer, int offset = 0) => buffer.ReadNullable(offset, ReadByte);

    [PublicAPI] public const int NullableByteSize = ByteSize + NullableSizeOverhead;

    #endregion

    #region SByte?

    public static void WriteSbyteNullable(this byte[] buffer, sbyte? value, int offset = 0)
    {
        buffer.WriteNullable(value, offset, WriteSbyte);
    }

    public static sbyte? ReadSbyteNullable(this byte[] buffer, int offset = 0) => buffer.ReadNullable(offset, ReadSbyte);

    [PublicAPI] public const int NullableSbyteSize = SbyteSize + NullableSizeOverhead;

    #endregion

    #region Bool?

    public static void WriteBoolNullable(this byte[] buffer, bool? value, int offset = 0)
    {
        buffer.WriteNullable(value, offset, WriteBool);
    }

    public static bool? ReadBoolNullable(this byte[] buffer, int offset = 0)
    {
        if (buffer[offset] == 0)
        {
            return null;
        }

        return buffer.ReadBool(offset + 1);
    }

    [PublicAPI] public const int NullableBoolSize = 2;

    #endregion

    #region Char?

    public static void WriteCharNullable(this byte[] buffer, char? value, int offset = 0)
    {
        buffer.WriteNullable(value, offset, WriteChar);
    }

    public static char? ReadCharNullable(this byte[] buffer, int offset = 0) => buffer.ReadNullable(offset, ReadChar);

    [PublicAPI] public const int NullableCharSize = CharSize + NullableSizeOverhead;

    #endregion

    #region Int?

    public static void WriteIntNullable(this byte[] buffer, int? value, int offset = 0)
    {
        buffer.WriteNullable(value, offset, WriteInt);
    }

    public static int? ReadIntNullable(this byte[] buffer, int offset = 0) => buffer.ReadNullable(offset, ReadInt);

    [PublicAPI] public const int NullableIntSize = IntSize + NullableSizeOverhead;

    #endregion

    #region Long?

    public static void WriteLongNullable(this byte[] buffer, long? value, int offset = 0)
    {
        buffer.WriteNullable(value, offset, WriteLong);
    }

    public static long? ReadLongNullable(this byte[] buffer, int offset = 0) => buffer.ReadNullable(offset, ReadLong);

    [PublicAPI] public const int NullableLongSize = LongSize + NullableSizeOverhead;

    #endregion

    #region Short?

    public static void WriteShortNullable(this byte[] buffer, short? value, int offset = 0)
    {
        buffer.WriteNullable(value, offset, WriteShort);
    }

    public static short? ReadShortNullable(this byte[] buffer, int offset = 0) => buffer.ReadNullable(offset, ReadShort);

    [PublicAPI] public const int NullableShortSize = ShortSize + NullableSizeOverhead;

    #endregion

    #region UInt?

    public static void WriteUintNullable(this byte[] buffer, uint? value, int offset = 0)
    {
        buffer.WriteNullable(value, offset, WriteUint);
    }

    public static uint? ReadUintNullable(this byte[] buffer, int offset = 0) => buffer.ReadNullable(offset, ReadUint);

    [PublicAPI] public const int NullableUintSize = UintSize + NullableSizeOverhead;

    #endregion

    #region ULong?

    public static void WriteUlongNullable(this byte[] buffer, ulong? value, int offset = 0)
    {
        buffer.WriteNullable(value, offset, WriteUlong);
    }

    public static ulong? ReadUlongNullable(this byte[] buffer, int offset = 0) => buffer.ReadNullable(offset, ReadUlong);

    [PublicAPI] public const int NullableUlongSize = UlongSize + NullableSizeOverhead;

    #endregion

    #region UShort?

    public static void WriteUshortNullable(this byte[] buffer, ushort? value, int offset = 0)
    {
        buffer.WriteNullable(value, offset, WriteUshort);
    }

    public static ushort? ReadUshortNullable(this byte[] buffer, int offset = 0) => buffer.ReadNullable(offset, ReadUshort);

    [PublicAPI] public const int NullableUshortSize = UshortSize + NullableSizeOverhead;

    #endregion

    #region Float?

    public static void WriteFloatNullable(this byte[] buffer, float? value, int offset = 0)
    {
        buffer.WriteNullable(value, offset, WriteFloat);
    }

    public static float? ReadFloatNullable(this byte[] buffer, int offset = 0) => buffer.ReadNullable(offset, ReadFloat);

    [PublicAPI] public const int NullableFloatSize = FloatSize + NullableSizeOverhead;

    #endregion

    #region Double?

    public static void WriteDoubleNullable(this byte[] buffer, double? value, int offset = 0)
    {
        buffer.WriteNullable(value, offset, WriteDouble);
    }

    public static double? ReadDoubleNullable(this byte[] buffer, int offset = 0) => buffer.ReadNullable(offset, ReadDouble);

    [PublicAPI] public const int NullableDoubleSize = DoubleSize + NullableSizeOverhead;

    #endregion

    #region Decimal?

    public static void WriteDecimalNullable(this byte[] buffer, decimal? value, int offset = 0)
    {
        buffer.WriteNullable(value, offset, WriteDecimal);
    }

    public static decimal? ReadDecimalNullable(this byte[] buffer, int offset = 0) => buffer.ReadNullable(offset, ReadDecimal);

    [PublicAPI] public const int NullableDecimalSize = DecimalSize + NullableSizeOverhead;

    #endregion
}