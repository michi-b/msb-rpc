using Microsoft.Extensions.Logging;
using MsbRpc.Logging;

namespace MsbRpc.Configuration;

public class InboundEndPointConfiguration : EndPointConfiguration
{
    public LogConfiguration LogArgumentDeserializationException;
    public LogConfiguration LogExceptionTransmissionException;

    /// <summary>
    ///     Prefix for log messages for easier identification
    /// </summary>
    public string LoggingName = "InboundEndPoint";

    public LogConfiguration LogProcedureExecutionException;
    public LogConfiguration LogRanToCompletion;
    public LogConfiguration LogReceivedAnyRequest;
    public LogConfiguration LogResponseSerializationException;
    public LogConfiguration LogStartedListening;
    public LogConfiguration LogStoppedListeningWithoutRunningToCompletion;

    public InboundEndPointConfiguration()
    {
        LogStoppedListeningWithoutRunningToCompletion
            = new LogConfiguration(LogEventIds.InboundEndPointStoppedListeningWithoutRunningToCompletion, LogLevel.Error);
        LogStartedListening = new LogConfiguration(LogEventIds.InboundEndPointStartedListening, LogLevel.Information);
        LogRanToCompletion = new LogConfiguration(LogEventIds.InboundEndPointRanToCompletion, LogLevel.Information);
        LogReceivedAnyRequest = new LogConfiguration(LogEventIds.InboundEndPointReceivedAnyRequest, LogLevel.Trace);
        LogArgumentDeserializationException = new LogConfiguration(LogEventIds.InboundEndPointArgumentDeserializationException, LogLevel.Error);
        LogProcedureExecutionException = new LogConfiguration(LogEventIds.InboundEndPointProcedureExecutionException, LogLevel.Error);
        LogResponseSerializationException = new LogConfiguration(LogEventIds.InboundEndPointResponseSerializationException, LogLevel.Error);
        LogExceptionTransmissionException = new LogConfiguration(LogEventIds.InboundEndPointExceptionTransmissionException, LogLevel.Critical);
        LoggerFactory = null;
    }
}