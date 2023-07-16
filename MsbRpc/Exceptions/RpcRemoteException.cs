#region

using System;
using JetBrains.Annotations;

#endregion

namespace MsbRpc.Exceptions;

public class RpcRemoteException : Exception
{
    [PublicAPI] public readonly RpcExceptionTransmission RemoteExceptionTransmission;

    public RpcRemoteException(RpcExceptionTransmission exceptionTransmission) => RemoteExceptionTransmission = exceptionTransmission;
}