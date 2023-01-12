using Microsoft.Extensions.Logging;

namespace MsbRpc.EndPoints.Configuration;

public abstract class InboundEndPointConfiguration : EndPointConfiguration
{
    public LogConfiguration LogStoppedListeningWithoutRunningToCompletion { get; set; }
    public LogConfiguration LogStartedListening { get; set; }
    public LogConfiguration LogRanToCompletion { get; set; }
    public LogConfiguration LogReceivedCall { get; set; }
    
    protected InboundEndPointConfiguration()
    {
        LogStoppedListeningWithoutRunningToCompletion = new LogConfiguration(LogLevel.Error);
        LogStartedListening = new LogConfiguration(LogLevel.Information);
        LogRanToCompletion = new LogConfiguration(LogLevel.Information);
        LogReceivedCall = new LogConfiguration(LogLevel.Trace);
        LoggerFactory = null;
    }
}