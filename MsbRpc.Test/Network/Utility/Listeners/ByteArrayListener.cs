using MsbRpc.Network;
using MsbRpc.Serialization.Buffers;
using MsbRpc.Sockets;

namespace MsbRpc.Test.Network.Utility.Listeners;

public static class ByteArrayListener
{
    private const int DefaultBufferSize = NetworkUtility.DefaultBufferSize;

    public static Task<byte[]> Listen(RpcSocket socket, TaskCreationOptions options = TaskCreationOptions.None)
    {
        return Task.Factory.StartNew
        (
            () => Listen(socket, DefaultBufferSize),
            CancellationToken.None,
            options,
            TaskScheduler.Default
        );
    }

    private static byte[] Listen(RpcSocket socket, int bufferSize)
    {
        ArraySegment<byte> buffer = new(new byte[bufferSize]);

        List<ArraySegment<byte>> receivedBuffers = new(1);
        int count;
        while ((count = socket.Receive(buffer)) > 0)
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