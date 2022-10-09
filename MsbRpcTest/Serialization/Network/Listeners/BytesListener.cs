using System.Net.Sockets;

namespace MsbRpcTest.Serialization.Network.Listeners;

public class BytesListener
{
    private readonly Socket _socket;

    public BytesListener(Socket socket) => _socket = socket;

    public async Task<byte[]> Listen(CancellationToken cancellationToken, int bufferSize = 1024)
    {
        var bytes = new List<byte>(bufferSize);
        byte[] buffer = new byte[bufferSize];
        var bufferSegment = new ArraySegment<byte>(buffer);

        int receivedCount;
        while ((receivedCount = await _socket.ReceiveAsync(bufferSegment, SocketFlags.None)) > 0)
        {
            // ReSharper disable once HeapView.BoxingAllocation
            // yeah, don't care, this is testing only
            bytes.AddRange(bufferSegment[..receivedCount]);
        }

        return bytes.ToArray();
    }
}