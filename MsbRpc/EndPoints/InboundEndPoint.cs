using System;
using JetBrains.Annotations;
using Microsoft.Extensions.Logging;
using MsbRpc.Contracts;
using MsbRpc.Messaging;
using MsbRpc.Serialization.Buffers;
using MsbRpc.Utility;

namespace MsbRpc.EndPoints;

[PublicAPI(Messages.ForUseInGeneratedCode)]
public abstract partial class InboundEndPoint<TEndPoint, TProcedure, TImplementation> : EndPoint<TEndPoint, TProcedure>
    where TEndPoint : InboundEndPoint<TEndPoint, TProcedure, TImplementation>
    where TImplementation : IRpcContract
    where TProcedure : Enum
{
    public readonly TImplementation Implementation;

    protected InboundEndPoint
    (
        Messenger messenger,
        TImplementation implementation,
        // ReSharper disable once ContextualLoggerProblem
        ILogger<TEndPoint> logger,
        int initialBufferSize = BufferUtility.DefaultInitialSize
    )
        : base
        (
            messenger,
            logger,
            initialBufferSize
        )
        => Implementation = implementation;

    public void Listen()
    {
        LogStartedListening(Logger);
        try
        {
            ListenReturnCode listenReturnCode = Messenger.Listen(Buffer, ReceiveMessage);
            if (RanToCompletion)
            {
                LogRanToCompletion(Logger, listenReturnCode.GetName());
            }
            else
            {
                LogStoppedListeningWithoutRunningToCompletion(Logger, listenReturnCode.GetName());
            }
        }
        finally
        {
            Dispose();
        }
    }

    public override void Dispose()
    {
        base.Dispose();
        if (Implementation is IDisposable disposableImplementation)
        {
            disposableImplementation.Dispose();
        }
    }

    private bool ReceiveMessage(Message message)
    {
        Request request = new(message);
        TProcedure procedure = GetProcedure(request.ProcedureId);
        LogReceivedCall(Logger, GetName(procedure), request.Length);
        Response response = Execute(procedure, request);
        RanToCompletion = response.RanToCompletion;
        Messenger.Send(new Message(response));
        return RanToCompletion;
    }

    [LoggerMessage
    (
        EventId = (int)LogEventIds.InboundEndPointStoppedListeningWithoutRunningToCompletion,
        Level = LogLevel.Warning,
        Message = "Stopped listening without running to completion with return code {ListenReturnCodeName}."
    )]
    private static partial void LogStoppedListeningWithoutRunningToCompletion(ILogger logger, string listenReturnCodeName);

    protected Message GetResultMessageBuffer(int count) => Buffer.GetMessage(count);

    [LoggerMessage
    (
        EventId = (int)LogEventIds.InboundEndPointStartedListening,
        Level = LogLevel.Information,
        Message = "Started listening."
    )]
    private static partial void LogStartedListening(ILogger logger);

    [LoggerMessage
    (
        EventId = (int)LogEventIds.InboundEndPointRanToCompletion,
        Level = LogLevel.Information,
        Message = "Ran to completion with return code {ListenReturnCodeName}."
    )]
    private static partial void LogRanToCompletion(ILogger logger, string listenReturnCodeName);

    [LoggerMessage
    (
        EventId = (int)LogEventIds.InboundEndPointReceivedCall,
        Level = LogLevel.Trace,
        Message = "Received a call to {ProcedureName} with {ArgumentsByteCount} argument bytes."
    )]
    private static partial void LogReceivedCall(ILogger logger, string procedureName, int argumentsByteCount);

    protected abstract Response Execute(TProcedure procedure, Request request);
}