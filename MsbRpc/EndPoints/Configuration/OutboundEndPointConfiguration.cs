using Microsoft.Extensions.Logging;

namespace MsbRpc.EndPoints.Configuration;

public abstract class OutboundEndPointConfiguration : EndPointConfiguration
{
    public LogConfiguration LogSentCall;

    protected OutboundEndPointConfiguration() => LogSentCall = new LogConfiguration(LogEventIds.OutboundEndPointSentRequest, LogLevel.Trace);
}