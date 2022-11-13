// ReSharper disable BuiltInTypeReferenceStyle

using System.Diagnostics;
using JetBrains.Annotations;

namespace MsbRpc.Serialization.ByteArraySegment;

public static partial class ByteArraySegmentExtensions
{
    [PublicAPI]
    public static ArraySegment<byte> GetSubSegment(this ArraySegment<byte> target, int offset) => target.GetSubSegment(offset, target.Count - offset);

    [PublicAPI]
    public static ArraySegment<byte> GetSubSegment(this ArraySegment<byte> target, int offset, int count)
    {
        Debug.Assert(offset > 0);
        Debug.Assert(count > 0);
        Debug.Assert(target.Array != null);
        target.AssertEndsAtOrBefore(offset, count);
        return new ArraySegment<byte>(target.Array!, target.Offset + offset, count);
    }

    public static string CreateContentString(this ArraySegment<byte> target)
    {
        byte[] array = target.Array!;
        return array.CreateContentString(target.Offset, target.Count);
    }
}