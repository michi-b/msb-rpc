#nullable enable

#region

using System;
using JetBrains.Annotations;

#endregion

namespace MsbRpc.Sockets.Exceptions;

public class RpcSocketException : Exception
{
    protected const string SocketHasBeenDisposedMessage = " Consequentially, the Socket has been disposed.";

    [PublicAPI] public RpcSocket Socket { get; }

    protected RpcSocketException(RpcSocket socket, string message, Exception? innerException = null)
        : base(message, innerException)
        => Socket = socket;
}