using System;

namespace MsbRpc.Servers.Listener;

public class IdentifiedMessengerException : Exception
{
    public IdentifiedMessengerException(InitialConnectionMessage connectionMessage, string message)
        : base($"{message}; the connection message is: {connectionMessage}") { }
}