namespace MsbRpc.Exceptions;

public enum RpcExceptionContinuation : byte
{
    Dispose,
    MarkRanToCompletion,
    Continue
}