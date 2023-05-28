using JetBrains.Annotations;
using MsbRpc.Configuration.Builders.Interfaces;
using MsbRpc.Configuration.Interfaces;

namespace MsbRpc.Configuration;

[PublicAPI]
public class ServerConfiguration : ConfigurationWithLoggerFactory, IServerConfiguration
{
    public int ListenBacklogSize { get; }

    public LogConfiguration LogAcceptedNewConnection { get; }

    public LogConfiguration LogDeclinedNewConnectionDuringDisposal { get; }

    public bool LogExceptionWhenLoggingStoppedListeningDueToDisposal { get; }

    public string LoggingName { get; }

    public LogConfiguration LogStartedListening { get; }

    public LogConfiguration LogStoppedListeningDueToDisposal { get; }

    public LogConfiguration LogStoppedListeningDueToException { get; }

    public LogConfiguration LogWasCreatedWithEphemeralPort { get; }

    public LogConfiguration LogWasCreatedWithSpecifiedPort { get; }

    public int Port { get; }

    public string ThreadName { get; }

    public IInboundEndPointConfiguration InboundEndPointConfiguration { get; }

    public IInboundEndpointRegistryConfiguration InboundEndPointRegistryConfiguration { get; }

    public ServerConfiguration(IServerConfigurationBuilder builder) : base(builder)
    {
        ListenBacklogSize = builder.ListenBacklogSize;
        LogAcceptedNewConnection = builder.LogAcceptedNewConnection;
        LogDeclinedNewConnectionDuringDisposal = builder.LogDeclinedNewConnectionDuringDisposal;
        LogExceptionWhenLoggingStoppedListeningDueToDisposal = builder.LogExceptionWhenLoggingStoppedListeningDueToDisposal;
        LoggingName = builder.LoggingName;
        LogStartedListening = builder.LogStartedListening;
        LogStoppedListeningDueToDisposal = builder.LogStoppedListeningDueToDisposal;
        LogStoppedListeningDueToException = builder.LogStoppedListeningDueToException;
        LogWasCreatedWithEphemeralPort = builder.LogWasCreatedWithEphemeralPort;
        LogWasCreatedWithSpecifiedPort = builder.LogWasCreatedWithSpecifiedPort;
        Port = builder.Port;
        ThreadName = builder.ThreadName;
        InboundEndPointConfiguration = new InboundEndPointConfiguration(builder.InboundEndPointConfiguration);
        InboundEndPointRegistryConfiguration = new InboundEndpointRegistryConfiguration(builder.InboundEndPointRegistryConfiguration);
    }
}