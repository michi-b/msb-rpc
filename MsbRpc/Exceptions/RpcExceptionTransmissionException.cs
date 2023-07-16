#region

using System;

#endregion

namespace MsbRpc.Exceptions;

public class RpcExceptionTransmissionException : Exception
{
    public RpcExceptionTransmissionException(Exception innerException) : base("An exception occured during RPC exception transmission.", innerException) { }
}