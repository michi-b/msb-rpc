using Microsoft.Extensions.Logging;
using MsbRpc.Configuration.Builders.Abstract;
using MsbRpc.Logging;

namespace MsbRpc.Configuration.Builders;

public class ConnectionListenerConfigurationBuilder : ConfigurationWithLoggerFactoryBuilder<ConnectionListenerConfiguration>
{
    public int ListenBacklogSize { get; set; } = 100;

    public LogConfigurationBuilder LogAcceptedNewUnIdentifiedConnection { get; set; } =
        new(LogEventIds.ConnectionListenerAcceptedNewUnIdentifiedConnection, LogLevel.Trace);

    public LogConfigurationBuilder LogAcceptedNewIdentifiedConnection { get; set; } =
        new(LogEventIds.ConnectionListenerAcceptedNewIdentifiedConnection, LogLevel.Trace);

    public LogConfigurationBuilder LogDeclinedNewConnectionDuringDisposal { get; set; } =
        new(LogEventIds.ConnectionListenerDeclinedNewConnectionDuringDisposal, LogLevel.Warning);

    public LogConfigurationBuilder LogDeclinedNewConnectionDueToException { get; set; } =
        new(LogEventIds.ConnectionListenerDeclinedNewConnectionDueToException, LogLevel.Error);

    public bool LogExceptionWhenLoggingStoppedListeningDueToDisposal { get; set; }
    public string LoggingName { get; set; } = "Server";
    public LogConfigurationBuilder LogStartedListening { get; set; } = new(LogEventIds.ConnectionListenerStartedListening);
    public LogConfigurationBuilder LogStoppedListeningDueToDisposal { get; set; } = new(LogEventIds.ConnectionListenerStoppedListeningDueToDisposal);
    public LogConfigurationBuilder LogStoppedListeningDueToException { get; set; } = new(LogEventIds.ConnectionListenerStoppedListeningDueToException, LogLevel.Error);
    public LogConfigurationBuilder LogWasCreatedWithEphemeralPort { get; set; } = new(LogEventIds.ConnectionListenerWasCreatedWithEphemeralPort);
    public LogConfigurationBuilder LogWasCreatedWithSpecifiedPort { get; set; } = new(LogEventIds.ConnectionListenerWasCreatedWithSpecifiedPort);

    // ReSharper disable once RedundantDefaultMemberInitializer
    // 0 means "ephemeral port"
    public int Port { get; set; } = 0;
    public string ThreadName { get; set; } = "Server";

    // sets both the name and the logging name
    public string Name
    {
        set
        {
            LoggingName = value;
            ThreadName = value;
        }
    }

    public override ConnectionListenerConfiguration Build() => new(this);
}