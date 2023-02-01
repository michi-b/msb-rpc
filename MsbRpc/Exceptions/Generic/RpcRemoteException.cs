using System;
using JetBrains.Annotations;

namespace MsbRpc.Exceptions.Generic;

public class RpcRemoteException<TProcedure> : RpcRemoteException where TProcedure : Enum
{
    [PublicAPI] public readonly TProcedure Procedure;

    public RpcRemoteException(RpcExceptionTransmission exceptionTransmission, TProcedure procedure) : base(exceptionTransmission) => Procedure = procedure;
}