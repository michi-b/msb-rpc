using JetBrains.Annotations;

namespace MsbRpc.Sockets.Exceptions;

public class RpcSocketException : Exception
{
    protected const string SocketHasBeenDisposedMessage = " Consequentially, the Socket has been disposed.";

    [PublicAPI] public IRpcSocket Socket { get; }

    protected RpcSocketException(IRpcSocket socket, string message, Exception? innerException = null)
        : base(message, innerException)
        => Socket = socket;
}