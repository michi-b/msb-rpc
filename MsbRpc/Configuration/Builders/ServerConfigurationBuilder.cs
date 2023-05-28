using JetBrains.Annotations;
using Microsoft.Extensions.Logging;
using MsbRpc.Configuration.Builders.Abstract;
using MsbRpc.Configuration.Builders.Interfaces;
using MsbRpc.Logging;

namespace MsbRpc.Configuration.Builders;

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
}

[PublicAPI]
public class ServerConfigurationBuilder : ConfigurationWithLoggerFactoryBuilder<ServerConfiguration>, IServerConfigurationBuilder
{
    public int ListenBacklogSize { get; set; } = 100;
    public LogConfigurationBuilder LogAcceptedNewConnection { get; set; } = new(LogEventIds.ServerAcceptedNewConnection, LogLevel.Trace);
    public LogConfigurationBuilder LogDeclinedNewConnectionDuringDisposal { get; set; } = new(LogEventIds.ServerDeclinedNewConnectionDuringDisposal, LogLevel.Warning);
    public bool LogExceptionWhenLoggingStoppedListeningDueToDisposal { get; set; }
    public string LoggingName { get; set; } = "Server";
    public LogConfigurationBuilder LogStartedListening { get; set; } = new(LogEventIds.ServerStartedListening);
    public LogConfigurationBuilder LogStoppedListeningDueToDisposal { get; set; } = new(LogEventIds.ServerStoppedListeningDueToDisposal);
    public LogConfigurationBuilder LogStoppedListeningDueToException { get; set; } = new(LogEventIds.ServerStoppedListeningDueToException, LogLevel.Error);
    public LogConfigurationBuilder LogWasCreatedWithEphemeralPort { get; set; } = new(LogEventIds.ServerWasCreatedWithEphemeralPort);
    public LogConfigurationBuilder LogWasCreatedWithSpecifiedPort { get; set; } = new(LogEventIds.ServerWasCreatedWithSpecifiedPort);

    // ReSharper disable once RedundantDefaultMemberInitializer
    // 0 means "ephemeral port"
    public int Port { get; set; } = 0;
    public string ThreadName { get; set; } = "Server";
    public override ServerConfiguration Build() => new(this);
}