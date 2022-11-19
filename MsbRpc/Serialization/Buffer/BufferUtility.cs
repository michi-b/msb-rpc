using JetBrains.Annotations;

namespace MsbRpc.Serialization.Buffer;

public static class BufferUtility
{
    [PublicAPI] public static readonly ArraySegment<byte> Empty = new(MemoryUtility.Empty, 0, 0);

    [PublicAPI]
    public static ArraySegment<byte> Create(int count) => new(new byte[count]);
    
    public static ArraySegment<byte> Create(byte value) => new(new[] {value});
}