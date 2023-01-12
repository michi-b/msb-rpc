﻿using Microsoft.Extensions.Logging;

namespace MsbRpc.EndPoints.Configuration;

public abstract class InboundEndPointConfiguration : EndPointConfiguration
{
    public LogConfiguration LogStoppedListeningWithoutRunningToCompletion;
    public LogConfiguration LogStartedListening;
    public LogConfiguration LogRanToCompletion;
    public LogConfiguration LogReceivedCall;
    
    protected InboundEndPointConfiguration()
    {
        LogStoppedListeningWithoutRunningToCompletion 
            = new LogConfiguration(LogEventIds.InboundEndPointStoppedListeningWithoutRunningToCompletion, LogLevel.Error);
        LogStartedListening = new LogConfiguration(LogEventIds.InboundEndPointStartedListening, LogLevel.Information);
        LogRanToCompletion = new LogConfiguration(LogEventIds.InboundEndPointRanToCompletion, LogLevel.Information);
        LogReceivedCall = new LogConfiguration(LogEventIds.InboundEndPointReceivedCall, LogLevel.Trace);
        LoggerFactory = null;
    }
}