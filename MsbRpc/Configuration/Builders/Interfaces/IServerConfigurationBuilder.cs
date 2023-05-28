using JetBrains.Annotations;

namespace MsbRpc.Configuration.Builders.Interfaces;

[PublicAPI]
public interface IServerConfigurationBuilder : IConfigurationWithLoggerFactoryBuilder
{
    int ListenBacklogSize { get; set; }
    LogConfigurationBuilder LogAcceptedNewConnection { get; set; }
    LogConfigurationBuilder LogDeclinedNewConnectionDuringDisposal { get; set; }
    bool LogExceptionWhenLoggingStoppedListeningDueToDisposal { get; set; }
    string LoggingName { get; set; }
    LogConfigurationBuilder LogStartedListening { get; set; }
    LogConfigurationBuilder LogStoppedListeningDueToDisposal { get; set; }
    LogConfigurationBuilder LogStoppedListeningDueToException { get; set; }
    LogConfigurationBuilder LogWasCreatedWithEphemeralPort { get; set; }
    LogConfigurationBuilder LogWasCreatedWithSpecifiedPort { get; set; }
    int Port { get; set; }
    string ThreadName { get; set; }
    IInboundEndPointRegistryConfigurationBuilder InboundEndPointRegistryConfiguration { get; set; }
    IInboundEndPointConfigurationBuilder InboundEndPointConfiguration { get; set; }
}