using JetBrains.Annotations;

namespace MsbRpc.Configuration.Builders.Interfaces;

[PublicAPI]
public interface IInboundEndPointConfigurationBuilder : IEndPointConfigurationBuilder
{
    string LoggingName { get; set; }
    LogConfigurationBuilder LogStartedListening { get; set; }
    LogConfigurationBuilder LogReceivedAnyRequest { get; set; }
    LogConfigurationBuilder LogArgumentDeserializationException { get; set; }
    LogConfigurationBuilder LogProcedureExecutionException { get; set; }
    LogConfigurationBuilder LogResponseSerializationException { get; set; }
    LogConfigurationBuilder LogExceptionTransmissionException { get; set; }
    LogConfigurationBuilder LogRanToCompletion { get; set; }
    LogConfigurationBuilder LogStoppedListeningWithoutRunningToCompletion { get; set; }
}