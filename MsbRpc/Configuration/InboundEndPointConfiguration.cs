using MsbRpc.Configuration.Builders.Interfaces;
using MsbRpc.Configuration.Interfaces;

namespace MsbRpc.Configuration;

public class InboundEndPointConfiguration : EndPointConfiguration, IInboundEndPointConfiguration
{
    /// <inheritdoc cref="IInboundEndPointConfiguration.LoggingName" />
    /// >
    public string LoggingName { get; }

    public LogConfiguration LogStartedListening { get; }
    public LogConfiguration LogReceivedAnyRequest { get; }
    public LogConfiguration LogArgumentDeserializationException { get; }
    public LogConfiguration LogProcedureExecutionException { get; }
    public LogConfiguration LogResponseSerializationException { get; }
    public LogConfiguration LogExceptionTransmissionException { get; }
    public LogConfiguration LogRanToCompletion { get; }
    public LogConfiguration LogStoppedListeningWithoutRunningToCompletion { get; }

    public InboundEndPointConfiguration(IInboundEndPointConfigurationBuilder builder) : base(builder)
    {
        LoggingName = builder.LoggingName;
        LogStartedListening = builder.LogStartedListening;
        LogReceivedAnyRequest = builder.LogReceivedAnyRequest;
        LogArgumentDeserializationException = builder.LogArgumentDeserializationException;
        LogExceptionTransmissionException = builder.LogExceptionTransmissionException;
        LogProcedureExecutionException = builder.LogProcedureExecutionException;
        LogResponseSerializationException = builder.LogResponseSerializationException;
        LogRanToCompletion = builder.LogRanToCompletion;
        LogStoppedListeningWithoutRunningToCompletion = builder.LogStoppedListeningWithoutRunningToCompletion;
    }
}