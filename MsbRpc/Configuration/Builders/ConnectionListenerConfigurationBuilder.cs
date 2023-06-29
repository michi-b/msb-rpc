using Microsoft.Extensions.Logging;
using MsbRpc.Configuration.Builders.Abstract;
using MsbRpc.Logging;

namespace MsbRpc.Configuration.Builders;

public class ConnectionListenerConfigurationBuilder : ConfigurationWithLoggerFactoryBuilder<ConnectionListenerConfiguration>
{
    public int ListenBacklogSize { get; set; } = 100;
    public LogConfigurationBuilder LogAcceptedNewUnIdentifiedConnection { get; set; } = new(LogEventIds.ServerAcceptedNewUnIdentifiedConnection, LogLevel.Trace);
    public LogConfigurationBuilder LogAcceptedNewIdentifiedConnection { get; set; } = new(LogEventIds.ServerAcceptedNewUnIdentifiedConnection, LogLevel.Trace);
    public LogConfigurationBuilder LogDeclinedNewConnectionDuringDisposal { get; set; } = new(LogEventIds.ServerDeclinedNewConnectionDuringDisposal, LogLevel.Warning);
    public LogConfigurationBuilder LogDeclinedNewConnectionDueToException { get; set; } = new(LogEventIds.ServerDeclinedNewConnectionDueToException, LogLevel.Error);
    public bool LogExceptionWhenLoggingStoppedListeningDueToDisposal { get; set; }
    public string LoggingName { get; set; } = "Server";
    public LogConfigurationBuilder LogStartedListening { get; set; } = new(LogEventIds.ServerStartedListening);
    public LogConfigurationBuilder LogStoppedListeningDueToDisposal { get; set; } = new(LogEventIds.ServerStoppedListeningDueToDisposal);
    public LogConfigurationBuilder LogStoppedListeningDueToException { get; set; } = new(LogEventIds.ServerStoppedListeningDueToException, LogLevel.Error);
    public LogConfigurationBuilder LogWasCreatedWithEphemeralPort { get; set; } = new(LogEventIds.ServerWasCreatedWithEphemeralPort);
    public LogConfigurationBuilder LogWasCreatedWithSpecifiedPort { get; set; } = new(LogEventIds.ServerWasCreatedWithSpecifiedPort);
    public override ConnectionListenerConfiguration Build() => new(this);
    
    // ReSharper disable once RedundantDefaultMemberInitializer
    // 0 means "ephemeral port"
    public int Port { get; set; } = 0;
    public string ThreadName { get; set; } = "Server";
}