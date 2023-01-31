using System;
using JetBrains.Annotations;

namespace MsbRpc.Exceptions;

public class RpcRemoteException : Exception
{
    [PublicAPI] public readonly RpcExceptionTransmission RemoteExceptionTransmission;

    public RpcRemoteException(RpcExceptionTransmission exceptionTransmission) => RemoteExceptionTransmission = exceptionTransmission;
}