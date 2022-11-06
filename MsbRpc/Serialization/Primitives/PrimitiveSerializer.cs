using System.Diagnostics.CodeAnalysis;
using JetBrains.Annotations;

namespace MsbRpc.Serialization.Primitives;

#pragma warning disable IDE0079 // Remove unnecessary suppression
[SuppressMessage("ReSharper", "BuiltInTypeReferenceStyle")]
#pragma warning restore IDE0079 // Remove unnecessary suppression
public static partial class PrimitiveSerializer
{
    /// <summary>
    ///     shortcut access for <see cref="BitConverter" />.IsLittleEndian
    /// </summary>
    private static readonly bool IsLittleEndian = BitConverter.IsLittleEndian;

    public static int SizeOf<TPrimitive>() where TPrimitive : struct
        => Type.GetTypeCode(typeof(TPrimitive)) switch
        {
            TypeCode.Boolean => BooleanSize,
            TypeCode.Byte => ByteSize,
            TypeCode.Char => CharSize,
            TypeCode.DateTime => throw new ArgumentOutOfRangeException(nameof(TPrimitive)),
            TypeCode.DBNull => throw new ArgumentOutOfRangeException(nameof(TPrimitive)),
            TypeCode.Decimal => DecimalSize,
            TypeCode.Double => DoubleSize,
            TypeCode.Empty => throw new ArgumentOutOfRangeException(nameof(TPrimitive)),
            TypeCode.Int16 => Int16Size,
            TypeCode.Int32 => Int32Size,
            TypeCode.Int64 => Int64Size,
            TypeCode.Object => throw new ArgumentOutOfRangeException(nameof(TPrimitive)),
            TypeCode.SByte => SByteSize,
            TypeCode.Single => SingleSize,
            TypeCode.String => throw new ArgumentOutOfRangeException(nameof(TPrimitive)),
            TypeCode.UInt16 => UInt16Size,
            TypeCode.UInt32 => UInt32Size,
            TypeCode.UInt64 => UInt64Size,
            _ => throw new ArgumentOutOfRangeException()
        };

    #region Boolean

    public static void WriteBoolean(this byte[] buffer, bool value, int offset = 0) => buffer[offset] = value ? (byte)1 : (byte)0;

    public static Boolean ReadBoolean(this byte[] buffer, int offset = 0) => buffer[offset] == 1;

    [PublicAPI] public const int BooleanSize = 1;

    #endregion

    #region Byte

    public static void WriteByte(this byte[] buffer, byte value, int offset = 0) => buffer[offset] = value;

    public static Byte ReadByte(this byte[] buffer, int offset = 0) => buffer[offset];

    [PublicAPI] public const int ByteSize = 1;

    #endregion

    #region SByte

    public static void WriteSByte(this byte[] buffer, sbyte value, int offset = 0) => buffer[offset] = (Byte)value;

    public static SByte ReadSByte(this byte[] buffer, int offset = 0) => (sbyte)buffer[offset];

    [PublicAPI] public const int SByteSize = 1;

    #endregion

    #region Char

    public static void WriteChar(this byte[] buffer, char value, int offset = 0)
    {
        var converter2 = new ByteConverter2(value);
        converter2.Write(buffer, offset);
    }

    public static Char ReadChar(this byte[] buffer, int offset = 0) => BitConverter.ToChar(buffer, offset);

    [PublicAPI] public const int CharSize = 2;

    #endregion

    #region Decimal

    public static void WriteDecimal(this byte[] buffer, decimal value, int offset = 0)
    {
        var converter16 = new ByteConverter16(value);
        converter16.Write(buffer, offset);
    }

    public static Decimal ReadDecimal(this byte[] buffer, int offset = 0)
    {
        var converter16 = new ByteConverter16(buffer, offset);
        return converter16.Read();
    }

    [PublicAPI] public const int DecimalSize = 16;

    #endregion

    #region Double

    public static void WriteDouble(this byte[] buffer, double value, int offset = 0)
    {
        var converter8 = new ByteConverter8(value);
        converter8.Write(buffer, offset);
    }

    public static Double ReadDouble(this byte[] buffer, int offset = 0) => BitConverter.ToDouble(buffer, offset);

    public const int DoubleSize = 8;

    #endregion

    #region Single

    public static void WriteSingle(this byte[] buffer, float value, int offset = 0)
    {
        var converter4 = new ByteConverter4(value);
        converter4.Write(buffer, offset);
    }

    public static Single ReadSingle(this byte[] buffer, int offset = 0) => BitConverter.ToSingle(buffer, offset);

    [PublicAPI] public const int SingleSize = 8;

    #endregion

    #region Int32

    public static void WriteInt32(this byte[] buffer, int value, int offset = 0)
    {
        var converter4 = new ByteConverter4(value);
        converter4.Write(buffer, offset);
    }

    public static Int32 ReadInt32(this byte[] buffer, int offset = 0) => BitConverter.ToInt32(buffer, offset);

    [PublicAPI] public const int Int32Size = 4;

    #endregion

    #region UInt32

    public static void WriteUInt32(this byte[] buffer, uint value, int offset = 0)
    {
        var converter4 = new ByteConverter4(value);
        converter4.Write(buffer, offset);
    }

    public static UInt32 ReadUInt32(this byte[] buffer, int offset = 0) => BitConverter.ToUInt32(buffer, offset);

    [PublicAPI] public const int UInt32Size = 4;

    #endregion

    #region Int64

    public static void WriteInt64(this byte[] buffer, long value, int offset = 0)
    {
        var converter8 = new ByteConverter8(value);
        converter8.Write(buffer, offset);
    }

    public static Int64 ReadInt64(this byte[] buffer, int offset = 0) => BitConverter.ToInt64(buffer, offset);

    [PublicAPI] public const int Int64Size = 8;

    #endregion

    #region UInt64

    public static void WriteUInt64(this byte[] buffer, ulong value, int offset = 0)
    {
        var converter8 = new ByteConverter8(value);
        converter8.Write(buffer, offset);
    }

    public static UInt64 ReadUInt64(this byte[] buffer, int offset = 0) => BitConverter.ToUInt64(buffer, offset);

    [PublicAPI] public const int UInt64Size = 8;

    #endregion

    #region Int16

    public static void WriteInt16(this byte[] buffer, short value, int offset = 0)
    {
        var converter2 = new ByteConverter2(value);
        converter2.Write(buffer, offset);
    }

    public static Int16 ReadInt16(this byte[] buffer, int offset = 0) => BitConverter.ToInt16(buffer, offset);

    [PublicAPI] public const int Int16Size = 2;

    #endregion

    #region UInt16

    public static void WriteUInt16(this byte[] buffer, ushort value, int offset = 0)
    {
        var converter2 = new ByteConverter2(value);
        converter2.Write(buffer, offset);
    }

    public static UInt16 ReadUInt16(this byte[] buffer, int offset = 0) => BitConverter.ToUInt16(buffer, offset);

    [PublicAPI] public const int UInt16Size = 2;

    #endregion
}