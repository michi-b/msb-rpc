#region

using System;

#endregion

namespace MsbRpc.Servers.Listener;

public class InvalidIdentifiedConnectionRequestException<TId> : Exception where TId : struct
{
    public InvalidIdentifiedConnectionRequestException(ConnectionRequest<TId> connectionRequest, string message)
        : base($"{message}; the connection message is: {connectionRequest}") { }

    public InvalidIdentifiedConnectionRequestException(ConnectionRequest<TId> connectionRequest, string message, Exception innerException)
        : base($"{message}; the connection message is: {connectionRequest}", innerException) { }
}