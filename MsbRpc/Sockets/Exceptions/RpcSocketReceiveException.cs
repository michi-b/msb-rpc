using System;

namespace MsbRpc.Sockets.Exceptions;

public class RpcSocketReceiveException : RpcSocketException
{
    public RpcSocketReceiveException(RpcSocket socket, int expectedBytesReceived, int actualBytesReceived)
        : base
        (
            socket,
            $"Expected to receive {expectedBytesReceived} bytes, but received {actualBytesReceived} bytes."
            + SocketHasBeenDisposedMessage
        ) { }

    public RpcSocketReceiveException(RpcSocket socket, Exception innerException)
        : base
        (
            socket,
            $"Failed to receive data due to exception: {innerException.GetType().Name}: {innerException.Message}" + SocketHasBeenDisposedMessage,
            innerException
        ) { }
}