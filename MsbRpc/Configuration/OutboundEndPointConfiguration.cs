using Microsoft.Extensions.Logging;
using MsbRpc.Logging;

namespace MsbRpc.Configuration;

public class OutboundEndPointConfiguration : EndPointConfiguration
{
    public LogConfiguration LogExceptionTransmissionException;
    public LogConfiguration LogRemoteRpcException;
    public LogConfiguration LogSentAnyRequest;

    public OutboundEndPointConfiguration()
    {
        LogSentAnyRequest = new LogConfiguration(LogEventIds.OutboundEndPointSentAnyRequest, LogLevel.Trace);
        LogRemoteRpcException = new LogConfiguration(LogEventIds.OutboundEndPointRemoteRpcException, LogLevel.Error);
        LogExceptionTransmissionException = new LogConfiguration(LogEventIds.OutboundEndPointExceptionTransmissionException, LogLevel.Critical);
    }
}