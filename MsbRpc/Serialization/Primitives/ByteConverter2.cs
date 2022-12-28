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
    private readonly struct ByteConverter2
    {
        [FieldOffset(0)] private readonly Char _charValue;
        [FieldOffset(0)] private readonly Int16 _int16Value;
        [FieldOffset(0)] private readonly UInt16 _uint16Value;
        [FieldOffset(0)] private readonly byte _byte0;
        [FieldOffset(1)] private readonly byte _byte1;

        public ByteConverter2(Char value)
        {
            Unsafe.SkipInit(out this);
            _charValue = value;
        }

        public ByteConverter2(Int16 value)
        {
            Unsafe.SkipInit(out this);
            _int16Value = value;
        }

        public ByteConverter2(UInt16 value)
        {
            Unsafe.SkipInit(out this);
            _uint16Value = value;
        }

        public void Write(byte[] buffer, int offset)
        {
            if (IsLittleEndian)
            {
                buffer[offset] = _byte0;
                buffer[offset + 1] = _byte1;
            }
            else
            {
                buffer[offset] = _byte1;
                buffer[offset + 1] = _byte0;
            }
        }
    }
}