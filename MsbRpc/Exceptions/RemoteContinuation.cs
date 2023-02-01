namespace MsbRpc.Exceptions;

public enum RemoteContinuation : byte
{
    Undefined = 0,
    Disposed = 1,
    RanToCompletion = 2,
    Continues = 3
}