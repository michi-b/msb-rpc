#region

using System;
using System.Text;

#endregion

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

    public override string ToString()
    {
        StringBuilder stringBuilder = new(200);
        stringBuilder.Append(Log ? "log the exception, " : "Do not log the exception, ");
        stringBuilder.Append(TransmissionOptions.GetString());
        stringBuilder.Append
        (
            Continuation switch
            {
                RpcExceptionContinuation.Continue => ", then continue",
                RpcExceptionContinuation.Dispose => ", then dispose the endpoint",
                RpcExceptionContinuation.MarkRanToCompletion => ", then mark the endpoint as ran to completion",
                _ => throw new ArgumentOutOfRangeException()
            }
        );
        return stringBuilder.ToString();
    }
}