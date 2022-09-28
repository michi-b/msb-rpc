using JetBrains.Annotations;

namespace MsbRpc.Buffer;

public static class Memory
{
    private static readonly byte[] EmptyBytes = Array.Empty<byte>();
    
    [PublicAPI] public static readonly ArraySegment<byte> Empty = new(EmptyBytes, 0, 0);
}