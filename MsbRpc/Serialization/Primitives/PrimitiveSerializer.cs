using JetBrains.Annotations;

namespace MsbRpc.Serialization.Primitives;

public static partial class PrimitiveSerializer
{
    /// <summary>
    ///     shortcut access for <see cref="BitConverter" />.IsLittleEndian
    /// </summary>
    private static readonly bool IsLittleEndian = BitConverter.IsLittleEndian;

    public static int GetSize(PrimitiveType primitiveType)
    {
        return primitiveType switch
        {
            PrimitiveType.Boolean => BooleanSize,
            PrimitiveType.Byte => ByteSize,
            PrimitiveType.Char => CharSize,
            PrimitiveType.Decimal => DecimalSize,
            PrimitiveType.Double => DoubleSize,
            PrimitiveType.Int16 => Int16Size,
            PrimitiveType.Int32 => Int32Size,
            PrimitiveType.Int64 => Int64Size,
            PrimitiveType.SByte => SByteSize,
            PrimitiveType.Single => SingleSize,
            PrimitiveType.UInt16 => UInt16Size,
            PrimitiveType.UInt32 => UInt32Size,
            PrimitiveType.UInt64 => UInt64Size,
            _ => throw new ArgumentOutOfRangeException(nameof(primitiveType), primitiveType, null)
        };
    }

    #region Byte

    public static void WriteByte(this byte[] buffer, byte value, int offset = 0) => buffer[offset] = value;

    public static byte ReadByte(this byte[] buffer, int offset = 0) => buffer[offset];

    [PublicAPI] public const int ByteSize = 1;

    #endregion

    #region SByte

    public static void WriteSbyte(this byte[] buffer, sbyte value, int offset = 0) => buffer[offset] = (byte)value;

    public static sbyte ReadSbyte(this byte[] buffer, int offset = 0) => (sbyte)buffer[offset];

    [PublicAPI] public const int SByteSize = 1;

    #endregion

    #region Bool

    public static void WriteBool(this byte[] buffer, bool value, int offset = 0) => buffer[offset] = value ? (byte)1 : (byte)0;

    public static bool ReadBool(this byte[] buffer, int offset = 0) => buffer[offset] == 1;

    [PublicAPI] public const int BooleanSize = 1;

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

    [PublicAPI] public const int Int32Size = 4;

    #endregion

    #region Long

    public static void WriteLong(this byte[] buffer, long value, int offset = 0)
    {
        var converter8 = new ByteConverter8(value);
        converter8.Write(buffer, offset);
    }

    public static long ReadLong(this byte[] buffer, int offset = 0) => BitConverter.ToInt64(buffer, offset);

    [PublicAPI] public const int Int64Size = 8;

    #endregion

    #region Short

    public static void WriteShort(this byte[] buffer, short value, int offset = 0)
    {
        var converter2 = new ByteConverter2(value);
        converter2.Write(buffer, offset);
    }

    public static short ReadShort(this byte[] buffer, int offset = 0) => BitConverter.ToInt16(buffer, offset);

    [PublicAPI] public const int Int16Size = 2;

    #endregion

    #region UInt

    public static void WriteUint(this byte[] buffer, uint value, int offset = 0)
    {
        var converter4 = new ByteConverter4(value);
        converter4.Write(buffer, offset);
    }

    public static uint ReadUint(this byte[] buffer, int offset = 0) => BitConverter.ToUInt32(buffer, offset);

    [PublicAPI] public const int UInt32Size = 4;

    #endregion

    #region ULong

    public static void WriteUlong(this byte[] buffer, ulong value, int offset = 0)
    {
        var converter8 = new ByteConverter8(value);
        converter8.Write(buffer, offset);
    }

    public static ulong ReadUlong(this byte[] buffer, int offset = 0) => BitConverter.ToUInt64(buffer, offset);

    [PublicAPI] public const int UInt64Size = 8;

    #endregion

    #region UShort

    public static void WriteUshort(this byte[] buffer, ushort value, int offset = 0)
    {
        var converter2 = new ByteConverter2(value);
        converter2.Write(buffer, offset);
    }

    public static ushort ReadUshort(this byte[] buffer, int offset = 0) => BitConverter.ToUInt16(buffer, offset);

    [PublicAPI] public const int UInt16Size = 2;

    #endregion

    #region Float

    public static void WriteFloat(this byte[] buffer, float value, int offset = 0)
    {
        var converter4 = new ByteConverter4(value);
        converter4.Write(buffer, offset);
    }

    public static float ReadFloat(this byte[] buffer, int offset = 0) => BitConverter.ToSingle(buffer, offset);

    [PublicAPI] public const int SingleSize = 8;

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
}