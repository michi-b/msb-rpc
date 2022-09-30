using JetBrains.Annotations;

namespace MsbRpc.Serialization;

public static class Memory
{
    [PublicAPI] public static readonly byte[] Empty = Array.Empty<byte>();
    [PublicAPI] public static readonly ArraySegment<byte> EmptySegment = new(Empty, 0, 0);
}