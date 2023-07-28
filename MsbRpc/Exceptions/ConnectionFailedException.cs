using System;
using MsbRpc.Servers.Listeners.Connections;

namespace MsbRpc.Exceptions;

public class ConnectionFailedException : Exception
{
    public ConnectionFailedException(ConnectionResult connectionResult)
        : base(GetMessage(connectionResult)) { }

    private static string GetMessage(ConnectionResult connectionResult)
    {
        return connectionResult switch
        {
            ConnectionResult.Okay => throw new ArgumentException("The connection result is okay.", nameof(connectionResult)),
            ConnectionResult.InvalidId => "The connection request was rejected because the connection ID is invalid.",
            ConnectionResult.Error => "The connection request was rejected because of an error.",
            _ => throw new ArgumentOutOfRangeException(nameof(connectionResult), connectionResult, null)
        };
    }
}