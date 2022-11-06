using JetBrains.Annotations;

namespace MsbRpc.Serialization;

public static class Memory
{
    [PublicAPI] public static readonly byte[] Empty = Array.Empty<byte>();
    [PublicAPI] public static readonly ArraySegment<byte> EmptySegment = new(Empty, 0, 0);

    [PublicAPI]
    public static ArraySegment<byte> Create(int count) => new(new byte[count]);

    [PublicAPI]
    public static string CreateByteString(params byte[] bytes) => bytes.CreateContentString();
}