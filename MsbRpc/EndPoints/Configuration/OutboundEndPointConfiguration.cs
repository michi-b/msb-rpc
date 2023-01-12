using Microsoft.Extensions.Logging;

namespace MsbRpc.EndPoints.Configuration;

public abstract class OutboundEndPointConfiguration : EndPointConfiguration
{
    public LogConfiguration LogSentCall { get; set; }

    protected OutboundEndPointConfiguration()
    {
        LogSentCall = new LogConfiguration(LogLevel.Trace);
    }
}