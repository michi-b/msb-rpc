using System.Diagnostics.CodeAnalysis;
using System.Runtime.InteropServices;

namespace MsbRps.Serialization;

[SuppressMessage("ReSharper", "BuiltInTypeReferenceStyle")]
public static class PrimitiveSerialization
{
    [StructLayout(LayoutKind.Explicit)]
    private struct FourBytes
    {
        [FieldOffset(0)] public Int32 Int32Value;

        [FieldOffset(0)] public Byte Byte0;
        [FieldOffset(1)] public Byte Byte1;
        [FieldOffset(2)] public Byte Byte2;
        [FieldOffset(3)] public Byte Byte3;

        public void SetBytes(byte[] buffer, int offset)
        {
            Byte0 = buffer[offset];
            Byte1 = buffer[offset + 1];
            Byte2 = buffer[offset + 2];
            Byte3 = buffer[offset + 3];
        }

        public void GetBytes(byte[] buffer, int offset)
        {
            buffer[offset] = Byte0;
            buffer[offset + 1] = Byte1;
            buffer[offset + 2] = Byte2;
            buffer[offset + 3] = Byte3;
        }
    }

    private static FourBytes _fourBytes;

    public static void Write(this byte[] buffer, Int32 value, int offset = 0)
    {
        _fourBytes.Int32Value = value;
        _fourBytes.GetBytes(buffer, offset);
    }

    public static Int32 ReadInt32(this byte[] buffer, int offset = 0)
    {
        _fourBytes.SetBytes(buffer, offset);
        return _fourBytes.Int32Value;
    }

    public static void Write(this byte[] buffer, Boolean value, int offset = 0)
    {
        buffer[offset] = value ? (byte)1 : (byte)0;
    }

    public static bool ReadBoolean(this byte[] buffer, int offset = 0)
    {
        return buffer[offset] == 1;
    }
}