namespace MsbRpc.Configuration.Interfaces;

public interface IInboundEndPointConfiguration : IEndPointConfiguration
{
    /// <summary>
    ///     Prefix for log messages for easier identification
    /// </summary>
    string LoggingName { get; }

    LogConfiguration LogArgumentDeserializationException { get; }
    LogConfiguration LogExceptionTransmissionException { get; }
    LogConfiguration LogProcedureExecutionException { get; }
    LogConfiguration LogRanToCompletion { get; }
    LogConfiguration LogReceivedAnyRequest { get; }
    LogConfiguration LogResponseSerializationException { get; }
    LogConfiguration LogStartedListening { get; }
    LogConfiguration LogStoppedListeningWithoutRunningToCompletion { get; }
}