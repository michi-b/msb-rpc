using JetBrains.Annotations;
using Microsoft.Extensions.Logging;
using MsbRpc.Configuration.Builders.Abstract;
using MsbRpc.Configuration.Builders.Interfaces;
using MsbRpc.Configuration.Interfaces;
using MsbRpc.Logging;

namespace MsbRpc.Configuration.Builders;

[PublicAPI]
public abstract class ServerConfigurationBuilder<TConfiguration>
    : ConfigurationWithLoggerFactoryBuilder<TConfiguration>, IServerConfigurationBuilder
    where TConfiguration : IConfiguration
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
}