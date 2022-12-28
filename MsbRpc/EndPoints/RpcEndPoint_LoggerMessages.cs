using System;
using Microsoft.Extensions.Logging;

namespace MsbRpc.EndPoints;

public abstract partial class RpcEndPoint<TInboundProcedure, TOutboundProcedure> where TInboundProcedure : Enum
    where TOutboundProcedure : Enum
{
    [LoggerMessage
    (
        EventId = (int)LogEventIds.RpcEndPointReceivedCall,
        Level = LogLevel.Trace,
        Message = "{endPointTypeName} received a call to {procedureName} with {argumentsByteCount} argument bytes"
    )]
    partial void LogReceivedCall(string endPointTypeName, string procedureName, int argumentsByteCount);

    [LoggerMessage
    (
        EventId = (int)LogEventIds.RpcEndPointSentCall,
        Level = LogLevel.Trace,
        Message = "{endPointTypeName} sent a request to {procedureName} with {argumentsByteCount} argument bytes"
    )]
    partial void LogSentCall(string endPointTypeName, string procedureName, int argumentsByteCount);
}