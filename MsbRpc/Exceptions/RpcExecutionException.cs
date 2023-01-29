using System;
using MsbRpc.Contracts;

namespace MsbRpc.Exceptions;

public class RpcExecutionException<TProcedure> : System.Exception where TProcedure : Enum
{
    public Exception OriginalException { get; }
    public TProcedure Procedure { get; }
    public RpcExecutionStage Stage { get; }

    public RpcExecutionException(Exception originalException, TProcedure procedure, RpcExecutionStage stage)
    {
        OriginalException = originalException;
        Procedure = procedure;
        Stage = stage;
    }
}