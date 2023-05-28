using JetBrains.Annotations;
using Microsoft.Extensions.Logging;
using MsbRpc.Configuration.Builders.Interfaces;

namespace MsbRpc.Configuration;

[PublicAPI]
public readonly struct ServerConfiguration
{
    public readonly ILoggerFactory? LoggerFactory;

    public readonly int ListenBacklogSize;

    public readonly LogConfiguration LogAcceptedNewConnection;

    public readonly LogConfiguration LogDeclinedNewConnectionDuringDisposal;

    public readonly bool LogExceptionWhenLoggingStoppedListeningDueToDisposal;

    public readonly string LoggingName;

    public readonly LogConfiguration LogStartedListening;

    public readonly LogConfiguration LogStoppedListeningDueToDisposal;

    public readonly LogConfiguration LogStoppedListeningDueToException;

    public readonly LogConfiguration LogWasCreatedWithEphemeralPort;

    public readonly LogConfiguration LogWasCreatedWithSpecifiedPort;

    public readonly int Port;

    public readonly string ThreadName;

    public readonly InboundEndPointConfiguration InboundEndPointConfiguration;

    public readonly InboundEndpointRegistryConfiguration InboundEndPointRegistryConfiguration;

    public ServerConfiguration(IServerConfigurationBuilder builder)
    {
        LoggerFactory = builder.LoggerFactory;
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
        InboundEndPointConfiguration = new InboundEndPointConfiguration(builder.EndPointConfiguration);
        InboundEndPointRegistryConfiguration = new InboundEndpointRegistryConfiguration(builder.EndPointRegistryConfiguration);
    }
}