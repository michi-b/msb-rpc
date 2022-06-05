using System.Diagnostics.CodeAnalysis;
using Decimal = System.Decimal;

namespace MsbRpc.Serialization.Primitives;

#pragma warning disable IDE0079 // Remove unnecessary suppression
[SuppressMessage("ReSharper", "BuiltInTypeReferenceStyle")]
#pragma warning restore IDE0079 // Remove unnecessary suppression
public class PrimitiveSerializer
{
    private Union _union;

    #region Boolean

    public static void WriteBoolean(Boolean value, byte[] buffer, int offset = 0) => buffer[offset] = value ? (byte)1 : (byte)0;

    public static Boolean ReadBoolean(byte[] buffer, int offset = 0) => buffer[offset] == 1;

    #endregion

    #region Byte

    public static void WriteByte(Byte value, byte[] buffer, int offset = 0) => buffer[offset] = value;

    public static Byte ReadByte(byte[] buffer, int offset = 0) => buffer[offset];

    #endregion

    #region SByte

    public static void WriteSByte(SByte value, byte[] buffer, int offset = 0) => buffer[offset] = (Byte)value;

    public static SByte ReadSByte(byte[] buffer, int offset = 0) => (sbyte)buffer[offset];

    #endregion

    #region Char

    public void WriteChar(Char value, byte[] buffer, int offset = 0) => _union.WriteChar(value, buffer, offset);

    public static Char ReadChar(byte[] buffer, int offset = 0) => BitConverter.ToChar(buffer, offset);

    #endregion

    #region Decimal
    public void WriteDecimal(Decimal value, byte[] buffer, int offset = 0) => _union.WriteDecimal(value, buffer, offset);

    public Decimal ReadDecimal(byte[] buffer, int offset = 0) => _union.ReadDecimal(buffer, offset);

    #endregion

    
    #region Double

    public void WriteDouble(Double value, byte[] buffer, int offset = 0) => _union.WriteDouble(value, buffer, offset);

    public static Double ReadDouble(byte[] buffer, int offset = 0) => BitConverter.ToDouble(buffer, offset);

    #endregion

    #region Single

    public void WriteSingle(Single value, byte[] buffer, int offset = 0) => _union.WriteSingle(value, buffer, offset);

    public static Single ReadSingle(byte[] buffer, int offset = 0) => BitConverter.ToSingle(buffer, offset);

    #endregion

    #region Int32

    public void WriteInt32(Int32 value, byte[] buffer, int offset = 0) => _union.WriteInt32(value, buffer, offset);

    public static Int32 ReadInt32(byte[] buffer, int offset = 0) => BitConverter.ToInt32(buffer, offset);

    #endregion

    #region UInt32

    public void WriteUInt32(UInt32 value, byte[] buffer, int offset = 0) => _union.WriteUInt32(value, buffer, offset);

    public static UInt32 ReadUInt32(byte[] buffer, int offset = 0) => BitConverter.ToUInt32(buffer, offset);

    #endregion

    #region Int64

    public void WriteInt64(Int64 value, byte[] buffer, int offset = 0) => _union.WriteInt64(value, buffer, offset);

    public static Int64 ReadInt64(byte[] buffer, int offset = 0) => BitConverter.ToInt64(buffer, offset);

    #endregion

    #region UInt64

    public void WriteUInt64(UInt64 value, byte[] buffer, int offset = 0) => _union.WriteUInt64(value, buffer, offset);

    public static UInt64 ReadUInt64(byte[] buffer, int offset = 0) => BitConverter.ToUInt64(buffer, offset);

    #endregion

    
    #region Int16

    public void WriteInt16(Int16 value, byte[] buffer, int offset = 0) => _union.WriteInt16(value, buffer, offset);

    public static Int16 ReadInt16(byte[] buffer, int offset = 0) => BitConverter.ToInt16(buffer, offset);

    #endregion

    
    #region UInt16

    public void WriteUInt16(UInt16 value, byte[] buffer, int offset = 0) => _union.WriteUInt16(value, buffer, offset);

    public static UInt16 ReadUInt16(byte[] buffer, int offset = 0) => BitConverter.ToUInt16(buffer, offset);

    #endregion

}

