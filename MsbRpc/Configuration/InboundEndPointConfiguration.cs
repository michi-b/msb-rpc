using Microsoft.Extensions.Logging;
using MsbRpc.Logging;

namespace MsbRpc.Configuration;

public abstract class InboundEndPointConfiguration : EndPointConfiguration
{
    public LogConfiguration LogRanToCompletion;
    public LogConfiguration LogReceivedCall;
    public LogConfiguration LogStartedListening;
    public LogConfiguration LogStoppedListeningWithoutRunningToCompletion;
    public LogConfiguration LogArgumentDeserializationException;
    public LogConfiguration LogProcedureExecutionException;
    public LogConfiguration LogResponseSerializationException;
    public LogConfiguration LogExceptionTransmissionException;

    protected InboundEndPointConfiguration()
    {
        LogStoppedListeningWithoutRunningToCompletion
            = new LogConfiguration(LogEventIds.InboundEndPointStoppedListeningWithoutRunningToCompletion, LogLevel.Error);
        LogStartedListening = new LogConfiguration(LogEventIds.InboundEndPointStartedListening, LogLevel.Information);
        LogRanToCompletion = new LogConfiguration(LogEventIds.InboundEndPointRanToCompletion, LogLevel.Information);
        LogReceivedCall = new LogConfiguration(LogEventIds.InboundEndPointReceivedCall, LogLevel.Trace);
        LogArgumentDeserializationException = new LogConfiguration(LogEventIds.InboundEndPointArgumentDeserializationException, LogLevel.Error);
        LogProcedureExecutionException = new LogConfiguration(LogEventIds.InboundEndPointProcedureExecutionException, LogLevel.Error);
        LogResponseSerializationException = new LogConfiguration(LogEventIds.InboundEndPointResponseSerializationException, LogLevel.Error);
        LogExceptionTransmissionException = new LogConfiguration(LogEventIds.InboundEndPointExceptionTransmissionException, LogLevel.Critical);
        LoggerFactory = null;
    }
}