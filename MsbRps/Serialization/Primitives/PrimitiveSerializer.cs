using System.Diagnostics.CodeAnalysis;

namespace MsbRps.Serialization.Primitives;

#pragma warning disable IDE0079 // Remove unnecessary suppression
[SuppressMessage("ReSharper", "BuiltInTypeReferenceStyle")]
#pragma warning restore IDE0079 // Remove unnecessary suppression
public static class PrimitiveSerializer
{
    private static Union _union;

    #region Boolean

    public static void WriteBoolean(Boolean value, byte[] buffer, int offset = 0)
    {
        buffer[offset] = value ? (byte)1 : (byte)0;
    }

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

    public static void WriteChar(Char value, byte[] buffer, int offset = 0)
    {
        _union.WriteChar(value, buffer, offset);
    }

    public static Char ReadChar(byte[] buffer, int offset = 0) => BitConverter.ToChar(buffer, offset);

    #endregion

    #region Decimal
    public static void WriteDecimal(Decimal value, byte[] buffer, int offset = 0)
    {
        _union.WriteDecimal(value, buffer, offset);
    }

    public static Decimal ReadDecimal(byte[] buffer, int offset = 0) => _union.ReadDecimal(buffer, offset);

    #endregion

    #region Int32

    public static void WriteInt32(Int32 value, byte[] buffer, int offset = 0)
    {
        _union.WriteInt32(value, buffer, offset);
    }

    public static int ReadInt32(byte[] buffer, Int32 offset = 0) => BitConverter.ToInt32(buffer, offset);

    #endregion
}