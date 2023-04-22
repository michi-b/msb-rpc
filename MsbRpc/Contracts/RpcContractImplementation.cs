using System;
using JetBrains.Annotations;
using MsbRpc.Exceptions;

namespace MsbRpc.Contracts;

[PublicAPI]
public abstract class RpcContractImplementation : Disposable.Disposable, IRpcContract
{
    public bool RanToCompletion { get; private set; }

    public virtual RpcExceptionHandlingInstructions HandleException
        (ref Exception exception, int procedureId, RpcExecutionStage executionStage)
        => RpcExceptionHandlingInstructions.Default;

    protected void MarkRanToCompletion() => RanToCompletion = true;
}