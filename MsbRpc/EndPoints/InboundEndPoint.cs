using System;
using JetBrains.Annotations;
using Microsoft.Extensions.Logging;
using MsbRpc.Messaging;
using MsbRpc.Serialization.Buffers;
using MsbRpc.Utility;

namespace MsbRpc.EndPoints;

[PublicAPI(Messages.ForUseInGeneratedCode)]
public abstract partial class InboundEndPoint<TProcedure, TImplementation> : OneWayEndPoint<TProcedure> where TProcedure : Enum
{
    public readonly TImplementation Implementation;
    
    protected InboundEndPoint
    (
        Messenger messenger,
        TImplementation implementation,
        ILogger logger,
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
        LogStartedListening(Logger, TypeName, Port);
        try
        {
            ListenReturnCode listenReturnCode = Messenger.Listen(Buffer, ReceiveMessage);
            LogStoppedListening(Logger, TypeName, Port, listenReturnCode.GetName());
        }
        finally
        {
            Dispose();
        }
    }

    private bool ReceiveMessage(Message message)
    {
        Request request = new(message);
        TProcedure procedure = GetProcedure(request.ProcedureId);
        LogReceivedCall(Logger, TypeName, GetName(procedure), Port, request.Length);
        Message response = Execute(procedure, request);
        Messenger.Send(response);
        return GetClosesCommunication(procedure);
    }

    public override void Dispose()
    {
        base.Dispose();
        if (Implementation is IDisposable disposableImplementation)
        {
            disposableImplementation.Dispose();
        }
    }
    
    protected Message GetResultMessageBuffer(int count) => Buffer.GetMessage(count);

    [LoggerMessage
    (
        EventId = (int)LogEventIds.InboundEndPointStartedListening,
        Level = LogLevel.Debug,
        Message = "{EndPointTypeName} started listening on port {Port}"
    )]
    private static partial void LogStartedListening(ILogger logger, string endPointTypeName, int port);
    
    [LoggerMessage
    (
        EventId = (int)LogEventIds.InboundEndPointReceivedCall,
        Level = LogLevel.Trace,
        Message = "{EndPointTypeName} received a call to {ProcedureName} on port {Port} with {ArgumentsByteCount} argument bytes"
    )]
    private static partial void LogReceivedCall(ILogger logger, string endPointTypeName, string procedureName, int port, int argumentsByteCount);
    
    [LoggerMessage
    (
        EventId = (int)LogEventIds.InboundEndPointStoppedListening,
        Level = LogLevel.Debug,
        Message = "{EndPointTypeName} stopped listening with on port {Port} with listen return code {ListenReturnCodeName}"
    )]
    private static partial void LogStoppedListening(ILogger logger, string endPointTypeName, int port, string listenReturnCodeName);

    protected abstract Message Execute(TProcedure procedure, Request request);
}