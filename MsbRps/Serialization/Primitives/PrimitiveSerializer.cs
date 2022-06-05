using System.Diagnostics.CodeAnalysis;

namespace MsbRps.Serialization.Primitives;

#pragma warning disable IDE0079 // Remove unnecessary suppression
[SuppressMessage("ReSharper", "BuiltInTypeReferenceStyle")]
#pragma warning restore IDE0079 // Remove unnecessary suppression
public static class PrimitiveSerializer
{
    private static Union _union;

    #region Boolean

    public static void Write(Boolean value, byte[] buffer, int offset = 0)
    {
        buffer[offset] = value ? (byte)1 : (byte)0;
    }

    public static Boolean ReadBoolean(byte[] buffer, int offset = 0) => buffer[offset] == 1;

    #endregion

    #region Byte

    public static void Write(Byte value, byte[] buffer, int offset = 0) => buffer[offset] = value;

    public static Byte ReadByte(byte[] buffer, int offset = 0) => buffer[offset];

    #endregion

    #region SByte

    public static void Write(SByte value, byte[] buffer, int offset = 0) => buffer[offset] = (Byte)value;

    public static SByte ReadSByte(byte[] buffer, int offset = 0) => (sbyte)buffer[offset];

    #endregion

    #region Int32

    public static void Write(Int32 value, byte[] buffer, int offset = 0)
    {
        _union.ConvertInt32(value, buffer, offset);
    }

    public static int ReadInt32(byte[] buffer, Int32 offset = 0) => BitConverter.ToInt32(buffer, offset);

    #endregion
}