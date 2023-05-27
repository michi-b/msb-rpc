using JetBrains.Annotations;
using Microsoft.Extensions.Logging;
using MsbRpc.Configuration.Interfaces;
using MsbRpc.Logging;

namespace MsbRpc.Configuration.Builders;

[PublicAPI]
public class ServerConfigurationBuilder : ConfigurationWithLoggerFactoryBuilder, IConfigurationBuilder<ServerConfiguration>
{
    public int ListenBacklogSize { get; set; } = 100;
    public LogConfiguration LogAcceptedNewConnection { get; set; } = new(LogEventIds.ServerAcceptedNewConnection, LogLevel.Trace);
    public LogConfiguration LogDeclinedNewConnectionDuringDisposal { get; set; } = new(LogEventIds.ServerDeclinedNewConnectionDuringDisposal, LogLevel.Warning);
    public bool LogExceptionWhenLoggingStoppedListeningDueToDisposal { get; set; }
    public string LoggingName { get; set; } = "Server";
    public LogConfiguration LogStartedListening { get; set; } = new(LogEventIds.ServerStartedListening, LogLevel.Information);
    public LogConfiguration LogStoppedListeningDueToDisposal { get; set; } = new(LogEventIds.ServerStoppedListeningDueToDisposal, LogLevel.Information);
    public LogConfiguration LogStoppedListeningDueToException { get; set; } = new(LogEventIds.ServerStoppedListeningDueToException, LogLevel.Error);
    public LogConfiguration LogWasCreatedWithEphemeralPort { get; set; } = new(LogEventIds.ServerWasCreatedWithEphemeralPort, LogLevel.Information);
    public LogConfiguration LogWasCreatedWithSpecifiedPort { get; set; } = new(LogEventIds.ServerWasCreatedWithSpecifiedPort, LogLevel.Information);

    // ReSharper disable once RedundantDefaultMemberInitializer
    // 0 means "ephemeral port"
    public int Port { get; set; } = 0;
    public string ThreadName { get; set; } = "Server";

    public new ServerConfiguration Build()
        => new
        (
            ThreadName,
            LoggingName,
            Port,
            ListenBacklogSize,
            LoggerFactory,
            LogWasCreatedWithSpecifiedPort,
            LogWasCreatedWithEphemeralPort,
            LogStartedListening,
            LogAcceptedNewConnection,
            LogDeclinedNewConnectionDuringDisposal,
            LogStoppedListeningDueToDisposal,
            LogStoppedListeningDueToException,
            LogExceptionWhenLoggingStoppedListeningDueToDisposal
        );
}