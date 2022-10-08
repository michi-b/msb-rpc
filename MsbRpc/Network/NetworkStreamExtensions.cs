using System.Net.Sockets;

namespace MsbRpc.Network;

public static class NetworkStreamExtensions
{
    public static Task WriteAsync
        (this NetworkStream target, ArraySegment<byte> bytes, CancellationToken cancellationToken) =>
        target.WriteAsync(bytes.Array!, bytes.Offset, bytes.Count, cancellationToken);

    public static Task<int> ReadAsync
        (this NetworkStream target, ArraySegment<byte> bytes, CancellationToken cancellationToken) =>
        target.ReadAsync(bytes.Array!, bytes.Offset, bytes.Count, cancellationToken);
}