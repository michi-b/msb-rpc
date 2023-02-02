using System;

namespace MsbRpc.Exceptions;

public static class RemoteContinuationExtensions
{
    public static string GetName(this RemoteContinuation target)
    {
        return target switch
        {
            RemoteContinuation.Undefined => nameof(RemoteContinuation.Undefined),
            RemoteContinuation.Disposed => nameof(RemoteContinuation.Disposed),
            RemoteContinuation.RanToCompletion => nameof(RemoteContinuation.RanToCompletion),
            RemoteContinuation.Continues => nameof(RemoteContinuation.Continues),
            _ => throw new ArgumentOutOfRangeException(nameof(target), target, null)
        };
    }
}