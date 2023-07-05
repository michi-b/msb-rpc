using System;

namespace MsbRpc.Servers.Listener;

public class InvalidIdentifiedConnectionRequestException : Exception
{
    public InvalidIdentifiedConnectionRequestException(ConnectionRequest connectionRequest, string message)
        : base($"{message}; the connection message is: {connectionRequest}") { }

    public InvalidIdentifiedConnectionRequestException(ConnectionRequest connectionRequest, string message, Exception innerException)
        : base($"{message}; the connection message is: {connectionRequest}", innerException) { }
}