namespace MsbRpc.Sockets.Exceptions;

public class RpcSocketException : Exception
{
    protected const string SocketHasBeenDisposedMessage = " Consequentially, the Socket has been disposed.";
    private IRpcSocket _socket;

    protected RpcSocketException(IRpcSocket socket, string message, Exception? innerException = null)
        : base(message, innerException)
        => _socket = socket;
}