using System;

namespace MsbRpc.Exceptions;

public static class RpcExceptionContinuationExtensions
{
    public static string GetName(this RpcExceptionContinuation target)
    {
        return target switch
        {
            RpcExceptionContinuation.Dispose => nameof(RpcExceptionContinuation.Dispose),
            RpcExceptionContinuation.MarkRanToCompletion => nameof(RpcExceptionContinuation.MarkRanToCompletion),
            RpcExceptionContinuation.Continue => nameof(RpcExceptionContinuation.Continue),
            _ => throw new ArgumentOutOfRangeException(nameof(target), target, null)
        };
    }
}