using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

#pragma warning disable CS0649

namespace MsbRpc.Serialization.Primitives;

public static partial class PrimitiveSerializer
{
    [SuppressMessage("ReSharper", "BuiltInTypeReferenceStyle")]
    [SuppressMessage("ReSharper", "SuggestBaseTypeForParameter")]
    [SuppressMessage("ReSharper", "PrivateFieldCanBeConvertedToLocalVariable")]
    [StructLayout(LayoutKind.Explicit)]
    private readonly struct ByteConverter4
    {
        [FieldOffset(0)] private readonly Single _singleValue;
        [FieldOffset(0)] private readonly Int32 _int32Value;
        [FieldOffset(0)] private readonly UInt32 _uint32Value;

        [FieldOffset(0)] private readonly byte _byte0;
        [FieldOffset(1)] private readonly byte _byte1;
        [FieldOffset(2)] private readonly byte _byte2;
        [FieldOffset(3)] private readonly byte _byte3;

        public ByteConverter4(Single value)
        {
            Unsafe.SkipInit(out this);
            _singleValue = value;
        }

        public ByteConverter4(Int32 value)
        {
            Unsafe.SkipInit(out this);
            _int32Value = value;
        }

        public ByteConverter4(UInt32 value)
        {
            Unsafe.SkipInit(out this);
            _uint32Value = value;
        }

        public void Write(byte[] buffer, int offset)
        {
            if (IsLittleEndian)
            {
                buffer[offset] = _byte0;
                buffer[offset + 1] = _byte1;
                buffer[offset + 2] = _byte2;
                buffer[offset + 3] = _byte3;
            }
            else
            {
                buffer[offset] = _byte3;
                buffer[offset + 1] = _byte2;
                buffer[offset + 2] = _byte1;
                buffer[offset + 3] = _byte0;
            }
        }
    }
}