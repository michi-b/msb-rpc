#region

using System;
using MsbRpc.Servers.Listeners.Connections.Generic;

#endregion

namespace MsbRpc.Servers.Listeners;

public class InvalidIdentifiedConnectionRequestException<TId> : Exception where TId : struct
{
    public InvalidIdentifiedConnectionRequestException(ConnectionRequest<TId> connectionRequest, string message)
        : base($"{message}; the connection message is: {connectionRequest}") { }

    public InvalidIdentifiedConnectionRequestException(ConnectionRequest<TId> connectionRequest, string message, Exception innerException)
        : base($"{message}; the connection message is: {connectionRequest}", innerException) { }
}