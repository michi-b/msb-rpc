using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using JetBrains.Annotations;
using Microsoft.Extensions.Logging;
using MsbRpc.Messaging;
using MsbRpc.Serialization.Buffers;
using MsbRpc.Serialization.Primitives;
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
    
    protected BufferReader SendRequest(TProcedure procedure, ArraySegment<byte> request)
    {
        int argumentByteCount = request.Count;

        ArraySegment<byte> requestWithProcedureId = request.Array == BufferUtility.Empty.Array
            ? Buffer.Get(PrimitiveSerializer.IntSize)
            //GetRequestWriter makes sure to leave space for the procedure id in front of the buffer new ArraySegment<byte>
            : new ArraySegment<byte>
            (
                request.Array!,
                0,
                argumentByteCount + PrimitiveSerializer.IntSize
            );

        requestWithProcedureId.WriteInt(GetProcedureId(procedure));

        Messenger.SendMessage(requestWithProcedureId);

        LogSentCall(Logger, TypeName, GetName(procedure), argumentByteCount);

        ReceiveMessageResult result = Messenger.ReceiveMessage(Buffer);

        return result.ReturnCode switch
        {
            ReceiveMessageReturnCode.Success => new BufferReader(result.Message),
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
