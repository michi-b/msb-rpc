using JetBrains.Annotations;
using Microsoft.Extensions.Logging;
using MsbRpc.Logging;

namespace MsbRpc.Configuration.Builders;

[PublicAPI]
public class InboundEndPointConfigurationBuilder : EndPointConfigurationBuilder<InboundEndPointConfiguration>
{
    public string LoggingName { get; set; } = "InboundEndPoint";
    public LogConfiguration LogStartedListening { get; set; } = new(LogEventIds.InboundEndPointStartedListening, LogLevel.Information);
    public LogConfiguration LogReceivedAnyRequest { get; set; } = new(LogEventIds.InboundEndPointReceivedAnyRequest, LogLevel.Trace);
    public LogConfiguration LogArgumentDeserializationException { get; set; } = new(LogEventIds.InboundEndPointArgumentDeserializationException, LogLevel.Error);
    public LogConfiguration LogProcedureExecutionException { get; set; } = new(LogEventIds.InboundEndPointProcedureExecutionException, LogLevel.Error);
    public LogConfiguration LogResponseSerializationException { get; set; } = new(LogEventIds.InboundEndPointResponseSerializationException, LogLevel.Error);
    public LogConfiguration LogExceptionTransmissionException { get; set; } = new(LogEventIds.InboundEndPointExceptionTransmissionException, LogLevel.Critical);
    public LogConfiguration LogRanToCompletion { get; set; } = new(LogEventIds.InboundEndPointRanToCompletion, LogLevel.Information);

    public LogConfiguration LogStoppedListeningWithoutRunningToCompletion { get; set; } =
        new(LogEventIds.InboundEndPointStoppedListeningWithoutRunningToCompletion, LogLevel.Error);

    public override InboundEndPointConfiguration Build()
        => new
        (
            InitialBufferSize,
            LoggerFactory,
            LoggingName,
            LogStartedListening,
            LogReceivedAnyRequest,
            LogArgumentDeserializationException,
            LogExceptionTransmissionException,
            LogProcedureExecutionException,
            LogResponseSerializationException,
            LogRanToCompletion,
            LogStoppedListeningWithoutRunningToCompletion
        );
}