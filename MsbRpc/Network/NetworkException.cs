using System;

namespace MsbRpc.Network;

public class NetworkException : Exception
{
    public NetworkException(string message) : base(message) { }
}