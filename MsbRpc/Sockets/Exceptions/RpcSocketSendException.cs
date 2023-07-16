#region

using System;

#endregion

namespace MsbRpc.Sockets.Exceptions;

public class RpcSocketSendException : RpcSocketException
{
    public RpcSocketSendException(RpcSocket socket, int expectedBytesSent, int actualBytesSent)
        : base
        (
            socket,
            $"Expected to send {expectedBytesSent} bytes, but sent {actualBytesSent} bytes."
            + SocketHasBeenDisposedMessage
        ) { }

    public RpcSocketSendException(RpcSocket socket, Exception innerException)
        : base
        (
            socket,
            $"Failed to send data due to exception: {innerException.GetType().Name}: {innerException.Message}"
            + SocketHasBeenDisposedMessage,
            innerException
        ) { }
}