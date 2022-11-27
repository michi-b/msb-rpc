using System.Net;
using MsbRpc.Serialization.Buffers;
using MsbRpc.Sockets;

namespace MsbRpcTest.Serialization.Network.Utility.Listeners;

public static class ByteArrayListener
{
    public static async Task<byte[]> ListenAsync
        (EndPoint ep, CancellationToken cancellationToken)
        => await ListenAsync(ep, NetworkUtility.DefaultBufferSize, cancellationToken);

    private static async Task<byte[]> ListenAsync(EndPoint ep, int bufferSize, CancellationToken cancellationToken)
    {
        ArraySegment<byte> buffer = new(new byte[bufferSize]);
        RpcSocket socket = await NetworkUtility.AcceptAsync(ep, cancellationToken);
        List<ArraySegment<byte>> receivedBuffers = new(1);
        int count;
        while ((count = await socket.ReceiveAsync(buffer, cancellationToken)) > 0)
        {
            receivedBuffers.Add(buffer.CopySubSegment(count));
        }

        int sum = receivedBuffers.Aggregate(0, (sum, current) => sum + current.Count);
        byte[] ret = new byte[sum];
        int bufferOffset = 0;
        foreach (ArraySegment<byte> currentBuffer in receivedBuffers)
        {
            int currentBufferSize = currentBuffer.Count;
            Buffer.BlockCopy(currentBuffer.Array!, currentBuffer.Offset, ret, bufferOffset, currentBufferSize);
            bufferOffset += currentBufferSize;
        }

        return ret;
    }
}