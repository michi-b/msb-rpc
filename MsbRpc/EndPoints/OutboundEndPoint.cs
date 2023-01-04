using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using JetBrains.Annotations;
using Microsoft.Extensions.Logging;
using MsbRpc.Messaging;
using MsbRpc.Serialization.Buffers;
using MsbRpc.Utility;

namespace MsbRpc.EndPoints;

[PublicAPI(Messages.ForUseInGeneratedCode)]
public abstract partial class OutboundEndPoint<TEndPoint, TProcedure> : OneWayEndPoint<TEndPoint, TProcedure> 
    where TEndPoint : OutboundEndPoint<TEndPoint, TProcedure>
    where TProcedure : Enum
{
    protected OutboundEndPoint
    (
        Messenger messenger,
        // ReSharper disable once ContextualLoggerProblem
        ILogger<TEndPoint> logger,
        int initialBufferSize = BufferUtility.DefaultInitialSize
    )
        : base(messenger, logger, initialBufferSize)
    {
        Debug.Assert(Enum.GetUnderlyingType(typeof(TProcedure)) == typeof(int));
    }

    protected async ValueTask<Message> SendRequestAsync(Request request, CancellationToken cancellationToken)
    {
        TProcedure procedure = GetProcedure(request.ProcedureId);
        
        await Messenger.SendAsync(new Message(request), cancellationToken);
        
        LogSentCall(Logger, TypeName, GetName(procedure), request.Buffer.Count);

        ReceiveResult result = await Messenger.ReceiveMessageAsync(Buffer, cancellationToken);

        return result.ReturnCode switch
        {
            ReceiveReturnCode.Success => result.Message,
            ReceiveReturnCode.ConnectionClosed => throw new RpcRequestException<TProcedure>
                (procedure, "connection closed while waiting for the response"),
            _ => throw new ArgumentOutOfRangeException()
        };
    }
    
    protected Message SendRequest(Request request)
    {
        Messenger.Send(new Message(request));

        TProcedure procedure = GetProcedure(request.ProcedureId);
        
        LogSentCall(Logger, TypeName, GetName(procedure), request.Buffer.Count);

        ReceiveResult result = Messenger.ReceiveMessage(Buffer);

        return result.ReturnCode switch
        {
            ReceiveReturnCode.Success => result.Message,
            ReceiveReturnCode.ConnectionClosed => throw new RpcRequestException<TProcedure>
                (procedure, "connection closed while waiting for the response"),
            _ => throw new ArgumentOutOfRangeException()
        };
    }

    private static int GetProcedureId(TProcedure value) => Unsafe.As<TProcedure, int>(ref value);
    
    [LoggerMessage
    (
        EventId = (int)LogEventIds.OutboundEndPointRequestedCall,
        Level = LogLevel.Trace,
        Message = "{endPointTypeName} sent a request to {procedureName} with {argumentsByteCount} argument bytes"
    )]
    private static partial void LogSentCall(ILogger<TEndPoint> logger, string endPointTypeName, string procedureName, int argumentsByteCount);
}
