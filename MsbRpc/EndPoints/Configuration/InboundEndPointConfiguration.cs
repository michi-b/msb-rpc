using System;
using Microsoft.Extensions.Logging;
using MsbRpc.Contracts;

namespace MsbRpc.EndPoints;

public class InboundEndPointConfiguration : EndPointConfiguration
{
    public LogConfiguration LogStoppedListeningWithoutRunningToCompletion { get; set; }
    public LogConfiguration LogStartedListening { get; set; }
    public LogConfiguration LogRanToCompletion { get; set; }
    public LogConfiguration LogReceivedCall { get; set; }
    
    public InboundEndPointConfiguration()
    {
        LogStoppedListeningWithoutRunningToCompletion = new LogConfiguration(true, LogLevel.Error);
        LogStartedListening = new LogConfiguration(true, LogLevel.Information);
        LogRanToCompletion = new LogConfiguration(true, LogLevel.Information);
        LogReceivedCall = new LogConfiguration(true, LogLevel.Trace);
        LoggerFactory = null;
    }
}