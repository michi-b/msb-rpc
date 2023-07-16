#region

using System;
using MsbRpc.Exceptions;

#endregion

namespace MsbRpc.Contracts;

public interface IRpcContract : IDisposable
{
    public bool RanToCompletion { get; }

    public RpcExceptionHandlingInstructions HandleException(ref Exception exception, int procedureId, RpcExecutionStage executionStage);
}