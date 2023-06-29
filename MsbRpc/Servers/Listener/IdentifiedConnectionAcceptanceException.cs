using System;

namespace MsbRpc.Servers.Listener;

public class IdentifiedConnectionAcceptanceException : Exception
{
    public IdentifiedConnectionAcceptanceException(InitialConnectionMessage connectionMessage, string message)
        : base($"{message}; the connection message is: {connectionMessage}") { }
}