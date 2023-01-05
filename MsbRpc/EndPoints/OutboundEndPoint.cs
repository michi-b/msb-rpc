﻿using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using JetBrains.Annotations;
using Microsoft.Extensions.Logging;
using MsbRpc.Messaging;
using MsbRpc.Serialization.Buffers;
using MsbRpc.Utility;

namespace MsbRpc.EndPoints;

[PublicAPI(Messages.ForUseInGeneratedCode)]
public abstract partial class OutboundEndPoint<TEndPoint, TProcedure> : EndPoint<TEndPoint, TProcedure>
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

    private static RpcRequestException<TProcedure> GetConnectionClosedException(TProcedure procedure)
        => new(procedure, "Connection closed while waiting for the response.");

    private static RpcRequestException<TProcedure> GetConnectionDisposedException(TProcedure procedure)
        => new(procedure, "Connection disposed while waiting for the response.");

    protected async ValueTask<Response> SendRequestAsync(Request request, CancellationToken cancellationToken)
    {
        TProcedure procedure = GetProcedure(request.ProcedureId);

        await Messenger.SendAsync(new Message(request), cancellationToken);

        LogSentCall(Logger, GetName(procedure), request.Buffer.Count);

        ReceiveResult result = await Messenger.ReceiveMessageAsync(Buffer, cancellationToken);

        switch (result.ReturnCode)
        {
            case ReceiveReturnCode.Success:
                Response response = new(result.Message);
                if (response.RanToCompletion)
                {
                    RanToCompletion = true;
                    Dispose();
                }
                return response;
            case ReceiveReturnCode.ConnectionClosed:
                throw GetConnectionClosedException(procedure);
            case ReceiveReturnCode.ConnectionDisposed:
                throw GetConnectionDisposedException(procedure);
            default:
                throw new ArgumentOutOfRangeException();
        }
    }

    protected Message SendRequest(Request request)
    {
        Messenger.Send(new Message(request));

        TProcedure procedure = GetProcedure(request.ProcedureId);

        LogSentCall(Logger, GetName(procedure), request.Buffer.Count);

        ReceiveResult result = Messenger.ReceiveMessage(Buffer);

        return result.ReturnCode switch
        {
            ReceiveReturnCode.Success => result.Message,
            ReceiveReturnCode.ConnectionClosed => throw GetConnectionClosedException(procedure),
            ReceiveReturnCode.ConnectionDisposed => throw GetConnectionDisposedException(procedure),
            _ => throw new ArgumentOutOfRangeException()
        };
    }

    protected abstract int GetId(TProcedure value);

    [LoggerMessage
    (
        EventId = (int)LogEventIds.OutboundEndPointSentRequest,
        Level = LogLevel.Trace,
        Message = "Sent a request to {procedureName} with {argumentsByteCount} argument bytes"
    )]
    private static partial void LogSentCall(ILogger<TEndPoint> logger, string procedureName, int argumentsByteCount);
}