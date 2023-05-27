using JetBrains.Annotations;
using Microsoft.Extensions.Logging;
using MsbRpc.Configuration.Builders.Abstract;
using MsbRpc.Logging;

namespace MsbRpc.Configuration.Builders;

[PublicAPI]
public class InboundEndPointConfigurationBuilder : EndPointConfigurationBuilder<InboundEndPointConfiguration>
{
    public string LoggingName { get; set; } = "InboundEndPoint";
    public LogConfigurationBuilder LogStartedListening { get; set; } = new(LogEventIds.InboundEndPointStartedListening, LogLevel.Information);
    public LogConfigurationBuilder LogReceivedAnyRequest { get; set; } = new(LogEventIds.InboundEndPointReceivedAnyRequest, LogLevel.Trace);
    public LogConfigurationBuilder LogArgumentDeserializationException { get; set; } = new(LogEventIds.InboundEndPointArgumentDeserializationException, LogLevel.Error);
    public LogConfigurationBuilder LogProcedureExecutionException { get; set; } = new(LogEventIds.InboundEndPointProcedureExecutionException, LogLevel.Error);
    public LogConfigurationBuilder LogResponseSerializationException { get; set; } = new(LogEventIds.InboundEndPointResponseSerializationException, LogLevel.Error);
    public LogConfigurationBuilder LogExceptionTransmissionException { get; set; } = new(LogEventIds.InboundEndPointExceptionTransmissionException, LogLevel.Critical);
    public LogConfigurationBuilder LogRanToCompletion { get; set; } = new(LogEventIds.InboundEndPointRanToCompletion, LogLevel.Information);

    public LogConfigurationBuilder LogStoppedListeningWithoutRunningToCompletion { get; set; } =
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