using Microsoft.Extensions.Logging;
using MsbRpc.Configuration.Builders.Interfaces;

namespace MsbRpc.Configuration;

public readonly struct InboundEndPointConfiguration
{
    public readonly int InitialBufferSize;

    public readonly ILoggerFactory? LoggerFactory;

    /// <inheritdoc cref="IInboundEndPointConfiguration.LoggingName" />
    /// >
    public readonly string LoggingName;

    public readonly LogConfiguration LogStartedListening;
    public readonly LogConfiguration LogReceivedAnyRequest;
    public readonly LogConfiguration LogArgumentDeserializationException;
    public readonly LogConfiguration LogProcedureExecutionException;
    public readonly LogConfiguration LogResponseSerializationException;
    public readonly LogConfiguration LogExceptionTransmissionException;
    public readonly LogConfiguration LogRanToCompletion;
    public readonly LogConfiguration LogStoppedListeningWithoutRunningToCompletion;

    public InboundEndPointConfiguration(IInboundEndPointConfigurationBuilder builder)
    {
        InitialBufferSize = builder.InitialBufferSize;
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