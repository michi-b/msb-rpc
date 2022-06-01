using System.Diagnostics.CodeAnalysis;

namespace MsbRps.Serialization.Primitives;

#pragma warning disable IDE0079 // Remove unnecessary suppression
[SuppressMessage("ReSharper", "BuiltInTypeReferenceStyle")]
#pragma warning restore IDE0079 // Remove unnecessary suppression
public class PrimitiveSerializer
{
    private Union _union;

    #region Int32

    public void Write(int value, byte[] buffer, int offset = 0)
    {
        _union.Int32Value = value;
        _union.Write4Bytes(buffer, offset);
    }

    public int ReadInt32(byte[] buffer, int offset = 0)
    {
        _union.Read4Bytes(buffer, offset);
        return _union.Int32Value;
    }

    #endregion

    #region Boolean

    // ReSharper disable once MemberCanBeMadeStatic.Global
    // for consistency
    public void Write(bool value, byte[] buffer, int offset = 0)
    {
        buffer[offset] = value ? (byte)1 : (byte)0;
    }

    public static void WriteStatic(bool value, byte[] buffer, int offset = 0)
    {
        buffer[offset] = value ? (byte)1 : (byte)0;
    }

    // ReSharper disable once MemberCanBeMadeStatic.Global
    // for consistency
    public bool ReadBoolean(byte[] buffer, int offset = 0) => buffer[offset] == 1;
    public static bool ReadBooleanStatic(byte[] buffer, int offset = 0) => buffer[offset] == 1;

    #endregion
}