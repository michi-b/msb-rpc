using Microsoft.Extensions.Logging;
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

    public InboundEndPointConfiguration
    (
        int initialBufferSize,
        ILoggerFactory? loggerFactory,
        string loggingName,
        LogConfiguration logStartedListening,
        LogConfiguration logReceivedAnyRequest,
        LogConfiguration logArgumentDeserializationException,
        LogConfiguration logExceptionTransmissionException,
        LogConfiguration logProcedureExecutionException,
        LogConfiguration logResponseSerializationException,
        LogConfiguration logRanToCompletion,
        LogConfiguration logStoppedListeningWithoutRunningToCompletion
    )
        : base(initialBufferSize, loggerFactory)
    {
        LoggingName = loggingName;
        LogStartedListening = logStartedListening;
        LogReceivedAnyRequest = logReceivedAnyRequest;
        LogArgumentDeserializationException = logArgumentDeserializationException;
        LogExceptionTransmissionException = logExceptionTransmissionException;
        LogProcedureExecutionException = logProcedureExecutionException;
        LogResponseSerializationException = logResponseSerializationException;
        LogRanToCompletion = logRanToCompletion;
        LogStoppedListeningWithoutRunningToCompletion = logStoppedListeningWithoutRunningToCompletion;
    }
}