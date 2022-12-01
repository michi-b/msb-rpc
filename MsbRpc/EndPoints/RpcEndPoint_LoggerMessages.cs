using Microsoft.Extensions.Logging;

namespace MsbRpc.EndPoints;

public abstract partial class RpcEndPoint<TInboundProcedure, TOutboundProcedure> where TInboundProcedure : Enum
    where TOutboundProcedure : Enum
{
    [LoggerMessage
    (
        EventId = (int)LogEventIds.RpcEndPointReceivedCall,
        Level = LogLevel.Debug,
        Message = "{endPointTypeName} received a call to {procedureName} with {argumentsByteCount} argument bytes"
    )]
    partial void LogReceivedCall(string endPointTypeName, string procedureName, int argumentsByteCount);
}