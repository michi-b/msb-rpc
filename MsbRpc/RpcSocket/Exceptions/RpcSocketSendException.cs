namespace MsbRpc.RpcSocket.Exceptions;

public class RpcSocketSendException : RpcSocketException
{
    public RpcSocketSendException(IRpcSocket socket, int expectedBytesSent, int actualBytesSent)
        : base
        (
            socket,
            $"Expected to send {expectedBytesSent} bytes, but sent {actualBytesSent} bytes."
            + SocketHasBeenDisposedMessage
        ) { }

    public RpcSocketSendException(IRpcSocket socket, Exception innerException)
        : base
        (
            socket,
            $"Failed to send data due to exception: {innerException.GetType().Name}: {innerException.Message}"
            + SocketHasBeenDisposedMessage,
            innerException
        ) { }
}