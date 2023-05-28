using JetBrains.Annotations;

namespace MsbRpc.Configuration.Interfaces;

[PublicAPI]
public interface IServerConfiguration : IConfigurationWithLoggerFactory
{
    public int ListenBacklogSize { get; }
    public LogConfiguration LogAcceptedNewConnection { get; }

    public LogConfiguration LogDeclinedNewConnectionDuringDisposal { get; }
    public bool LogExceptionWhenLoggingStoppedListeningDueToDisposal { get; }

    /// <summary>
    ///     Prefix for log messages for easier identification
    /// </summary>
    public string LoggingName { get; }

    public LogConfiguration LogStartedListening { get; }
    public LogConfiguration LogStoppedListeningDueToDisposal { get; }
    public LogConfiguration LogStoppedListeningDueToException { get; }
    public LogConfiguration LogWasCreatedWithEphemeralPort { get; }
    public LogConfiguration LogWasCreatedWithSpecifiedPort { get; }

    /// <summary>
    ///     The port to listen on for new connections. If 0, the OS will assign an ephemeral port.
    /// </summary>
    public int Port { get; }

    /// <summary>
    ///     Thread name for easier identification, will have the port number appended to it
    /// </summary>
    public string ThreadName { get; }

    public IInboundEndPointConfiguration InboundEndPointConfiguration { get; }

    public IInboundEndpointRegistryConfiguration InboundEndPointRegistryConfiguration { get; }
}