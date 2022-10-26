using System.Diagnostics.CodeAnalysis;
using JetBrains.Annotations;

namespace MsbRpc.Serialization.Primitives;

#pragma warning disable IDE0079 // Remove unnecessary suppression
[SuppressMessage("ReSharper", "BuiltInTypeReferenceStyle")]
#pragma warning restore IDE0079 // Remove unnecessary suppression
public static partial class PrimitiveSerializer
{
    /// <summary>
    /// shortcut access for <see cref="BitConverter"/>.IsLittleEndian
    /// </summary>
    private static readonly bool IsLittleEndian = BitConverter.IsLittleEndian;

    #region Boolean

    public static void WriteBoolean(Boolean value, byte[] buffer, int offset = 0) => buffer[offset] = value ? (byte)1 : (byte)0;

    public static Boolean ReadBoolean(byte[] buffer, int offset = 0) => buffer[offset] == 1;

    [PublicAPI]
    public const int BooleanSize = 1;

    #endregion

    #region Byte

    public static void WriteByte(Byte value, byte[] buffer, int offset = 0) => buffer[offset] = value;

    public static Byte ReadByte(byte[] buffer, int offset = 0) => buffer[offset];

    [PublicAPI]
    public const int ByteSize = 1;

    #endregion

    #region SByte

    public static void WriteSByte(SByte value, byte[] buffer, int offset = 0) => buffer[offset] = (Byte)value;

    public static SByte ReadSByte(byte[] buffer, int offset = 0) => (sbyte)buffer[offset];

    [PublicAPI]
    public const int SByteSize = 1;

    #endregion

    #region Char

    public static void WriteChar(Char value, byte[] buffer, int offset = 0)
    {
        var converter2 = new ByteConverter2(value);
        converter2.Write(buffer, offset);
    }

    public static Char ReadChar(byte[] buffer, int offset = 0) => BitConverter.ToChar(buffer, offset);

    [PublicAPI]
    public const int CharSize = 2;

    #endregion

    #region Decimal

    public static void WriteDecimal(Decimal value, byte[] buffer, int offset = 0)
    {
        var converter16 = new ByteConverter16(value);
        converter16.Write(buffer, offset);
    }

    public static Decimal ReadDecimal(byte[] buffer, int offset = 0)
    {
        var converter16 = new ByteConverter16(buffer, offset);
        return converter16.Read();
    }

    [PublicAPI]
    public const int DecimalSize = 16;

    #endregion

    #region Double

    public static void WriteDouble(Double value, byte[] buffer, int offset = 0)
    {
        var converter8 = new ByteConverter8(value);
        converter8.Write(buffer, offset);
    }

    public static Double ReadDouble(byte[] buffer, int offset = 0) => BitConverter.ToDouble(buffer, offset);

    public const int DoubleSize = 8;

    #endregion

    #region Single

    public static void WriteSingle(Single value, byte[] buffer, int offset = 0)
    {
        var converter4 = new ByteConverter4(value);
        converter4.Write(buffer, offset);
    }

    public static Single ReadSingle(byte[] buffer, int offset = 0) => BitConverter.ToSingle(buffer, offset);

    [PublicAPI]
    public const int SingleSize = 8;

    #endregion

    #region Int32

    public static void WriteInt32(Int32 value, byte[] buffer, int offset = 0)
    {
        var converter4 = new ByteConverter4(value);
        converter4.Write(buffer, offset);
    }

    public static Int32 ReadInt32(byte[] buffer, int offset = 0) => BitConverter.ToInt32(buffer, offset);

    [PublicAPI]
    public const int Int32Size = 4;

    #endregion

    #region UInt32

    public static void WriteUInt32(UInt32 value, byte[] buffer, int offset = 0)
    {
        var converter4 = new ByteConverter4(value);
        converter4.Write(buffer, offset);
    }

    public static UInt32 ReadUInt32(byte[] buffer, int offset = 0) => BitConverter.ToUInt32(buffer, offset);

    [PublicAPI]
    public const int UInt32Size = 4;

    #endregion

    #region Int64

    public static void WriteInt64(Int64 value, byte[] buffer, int offset = 0)
    {
        var converter8 = new ByteConverter8(value);
        converter8.Write(buffer, offset);
    }

    public static Int64 ReadInt64(byte[] buffer, int offset = 0) => BitConverter.ToInt64(buffer, offset);

    [PublicAPI]
    public const int Int64Size = 8;

    #endregion

    #region UInt64

    public static void WriteUInt64(UInt64 value, byte[] buffer, int offset = 0)
    {
        var converter8 = new ByteConverter8(value);
        converter8.Write(buffer, offset);
    }

    public static UInt64 ReadUInt64(byte[] buffer, int offset = 0) => BitConverter.ToUInt64(buffer, offset);

    [PublicAPI]
    public const int UInt64Size = 8;

    #endregion

    #region Int16

    public static void WriteInt16(Int16 value, byte[] buffer, int offset = 0)
    {
        var converter2 = new ByteConverter2(value);
        converter2.Write(buffer, offset);
    }

    public static Int16 ReadInt16(byte[] buffer, int offset = 0) => BitConverter.ToInt16(buffer, offset);

    [PublicAPI]
    public const int Int16Size = 2;

    #endregion

    #region UInt16

    public static void WriteUInt16(UInt16 value, byte[] buffer, int offset = 0)
    {
        var converter2 = new ByteConverter2(value);
        converter2.Write(buffer, offset);
    }   

    public static UInt16 ReadUInt16(byte[] buffer, int offset = 0) => BitConverter.ToUInt16(buffer, offset);

    [PublicAPI]
    public const int UInt16Size = 2;

    #endregion

    public static int GetSizeOf<TPrimitive>() where TPrimitive : struct =>
        Type.GetTypeCode(typeof(TPrimitive)) switch
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
}