using Microsoft.Extensions.Logging;
using MsbRpc.Contracts;
using MsbRpc.Exceptions;

namespace MsbRpc.Test.Integration.LoopBack;

public class LoopBackInitiator : ILoopBackReceiver
{
    private ILogger<LoopBackInitiator> Logger { get; }

    public bool RanToCompletion => false;

    public LoopBackInitiator(ILoggerFactory loggerFactory) => Logger = loggerFactory.CreateLogger<LoopBackInitiator>();

    public void Dispose()
    {
        GC.SuppressFinalize(this);
    }

    public RpcExceptionHandlingInstructions HandleException
        (ref Exception exception, int procedureId, RpcExecutionStage executionStage)
        => RpcExceptionHandlingInstructions.Default;

    public void InitiateLoopBack(int value)
    {
        Logger.Log(LogLevel.Information, "received call to initiate loopback of {Value}", value);
    }
}