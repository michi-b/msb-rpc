// ReSharper disable BuiltInTypeReferenceStyle

using System.Diagnostics;
using JetBrains.Annotations;
using MsbRpc.Serialization.Primitives;

namespace MsbRpc.Serialization;

public static class ByteArraySegmentExtensions
{
    public static Boolean ReadBoolean(this ArraySegment<byte> target, int offset = 0)
    {
        target.AssertContains<byte>(offset);
        return target.Array!.ReadBoolean(target.Offset + offset);
    }

    public static void WriteBoolean(this ArraySegment<byte> target, bool value, int offset = 0)
    {
        target.AssertContains<byte>(offset);
        target.Array!.WriteBoolean(value, offset);
    }

    [AssertionMethod, Conditional("DEBUG")]
    private static void AssertContains<TPrimitive>(this ArraySegment<byte> target, int offset) where TPrimitive : struct
    {
        target.AssertHasArray();
        target.AssertDoesNotEndBefore<TPrimitive>(offset);
    }

    [AssertionMethod, Conditional("DEBUG")]
    private static void AssertDoesNotEndBefore<TPrimitive>(this ArraySegment<byte> target, int offset) where TPrimitive : struct
    {
        if (offset + PrimitiveSerializer.GetSizeOf<TPrimitive>() > target.Count)
        {
            throw new ArgumentOutOfRangeException(nameof(offset));
        }
    }
    
    [AssertionMethod, Conditional("DEBUG")]
    private static void AssertHasArray(this ArraySegment<byte> target)
    {
        if (target.Array == null)
        {
            throw new ArgumentNullException(nameof(target) + ".Array");
        }
    }
}
