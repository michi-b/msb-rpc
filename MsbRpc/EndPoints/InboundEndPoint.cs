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
    protected readonly TImplementation Implementation;
    
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

    public ListenReturnCode Listen() => Messenger.Listen(Buffer, ReceiveMessage);

    private bool ReceiveMessage(Message message)
    {
        Request request = new(message);
        TProcedure procedure = GetProcedure(request.ProcedureId);
        LogReceivedCall(Logger, TypeName, GetName(procedure), request.Length);
        Message response = Execute(procedure, request);
        Messenger.Send(response);
        return GetClosesCommunication(procedure);
    }

    protected Message GetResultMessageBuffer(int count) => Buffer.GetMessage(count);

    [LoggerMessage
    (
        EventId = (int)LogEventIds.RpcEndPointReceivedCall,
        Level = LogLevel.Trace,
        Message = "{endPointTypeName} received a call to {procedureName} with {argumentsByteCount} argument bytes"
    )]
    private static partial void LogReceivedCall(ILogger logger, string endPointTypeName, string procedureName, int argumentsByteCount);

    protected abstract Message Execute(TProcedure procedure, Request request);
}