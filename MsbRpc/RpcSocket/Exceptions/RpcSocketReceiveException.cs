namespace MsbRpc.RpcSocket.Exceptions;

public class RpcSocketReceiveException : RpcSocketException
{
    public RpcSocketReceiveException(IRpcSocket socket, int expectedBytesReceived, int actualBytesReceived)
        : base
        (
            socket,
            $"Expected to receive {expectedBytesReceived} bytes, but received {actualBytesReceived} bytes."
            + SocketHasBeenDisposedMessage
        ) { }

    public RpcSocketReceiveException(IRpcSocket socket, Exception innerException)
        : base
        (
            socket,
            $"Failed to send receive data due to exception: {innerException.GetType().Name}: {innerException.Message}" + SocketHasBeenDisposedMessage,
            innerException
        ) { }
}