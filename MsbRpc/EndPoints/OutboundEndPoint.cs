using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using JetBrains.Annotations;
using Microsoft.Extensions.Logging;
using MsbRpc.Messaging;
using MsbRpc.Serialization.Buffers;
using MsbRpc.Utility;

namespace MsbRpc.EndPoints;

[PublicAPI(Messages.ForUseInGeneratedCode)]
public abstract partial class OutboundEndPoint<TProcedure> : OneWayEndPoint<TProcedure> where TProcedure : Enum
{
    protected OutboundEndPoint
    (
        Messenger messenger,
        ILogger logger,
        int initialBufferSize = BufferUtility.DefaultInitialSize
    )
        : base(messenger, logger, initialBufferSize)
    {
        Debug.Assert(Enum.GetUnderlyingType(typeof(TProcedure)) == typeof(int));
    }
    
    protected Message SendRequest(Request request)
    {
        Messenger.SendMessage(new Message(request));

        TProcedure procedure = GetProcedure(request.ProcedureId);
        
        LogSentCall(Logger, TypeName, GetName(procedure), request.Buffer.Count);

        ReceiveMessageResult result = Messenger.ReceiveMessage(Buffer);

        return result.ReturnCode switch
        {
            ReceiveMessageReturnCode.Success => result.Message,
            ReceiveMessageReturnCode.ConnectionClosed => throw new RpcRequestException<TProcedure>
                (procedure, "connection closed while waiting for the response"),
            _ => throw new ArgumentOutOfRangeException()
        };
    }

    private static int GetProcedureId(TProcedure value) => Unsafe.As<TProcedure, int>(ref value);
    
    [LoggerMessage
    (
        EventId = (int)LogEventIds.RpcEndPointSentCall,
        Level = LogLevel.Trace,
        Message = "{endPointTypeName} sent a request to {procedureName} with {argumentsByteCount} argument bytes"
    )]
    partial void LogSentCall(ILogger logger, string endPointTypeName, string procedureName, int argumentsByteCount);
}
