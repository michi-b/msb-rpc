namespace MsbRpc.Exceptions;

public readonly struct RpcExceptionHandlingInstructions
{
    public readonly RpcExceptionContinuation Continuation { get; }
    public readonly RpcExceptionTransmissionOptions TransmissionOptions { get; }
    public readonly bool Log;

    private RpcExceptionHandlingInstructions(RpcExceptionContinuation continuation, RpcExceptionTransmissionOptions transmissionOptions, bool log)
    {
        Continuation = continuation;
        TransmissionOptions = transmissionOptions;
        Log = log;
    }

    public static readonly RpcExceptionHandlingInstructions Default = new(RpcExceptionContinuation.Dispose, RpcExceptionTransmissionOptions.None, true);

    public RpcExceptionHandlingInstructions WithContinuation(RpcExceptionContinuation continuation) => new(continuation, TransmissionOptions, Log);

    public RpcExceptionHandlingInstructions WithTransmissionOptions(RpcExceptionTransmissionOptions exceptionTransmission)
        => new(Continuation, exceptionTransmission, Log);

    public RpcExceptionHandlingInstructions Logging(bool log) => new(Continuation, TransmissionOptions, log);
}