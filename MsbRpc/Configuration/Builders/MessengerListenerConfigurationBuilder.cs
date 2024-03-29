﻿#region

using Microsoft.Extensions.Logging;
using MsbRpc.Configuration.Builders.Abstract;
using MsbRpc.Logging;
using MsbRpc.Servers.Listener;

#endregion

namespace MsbRpc.Configuration.Builders;

public class MessengerListenerConfigurationBuilder : ConfigurationWithLoggerFactoryBuilder<MessengerListenerConfiguration>
{
    public int ListenBacklogSize { get; set; } = 100;

    public LogConfigurationBuilder LogAcceptedNewUnIdentifiedConnection { get; set; } =
        new(LogEventIds.MessengerListenerAcceptedNewUnIdentifiedConnection, LogLevel.Trace);

    public LogConfigurationBuilder LogCompletedIdentifiedConnectionTask { get; set; } =
        new(LogEventIds.MessengerListenerCompletedIdentifiedConnectionTask, LogLevel.Trace);

    public LogConfigurationBuilder LogAcceptedNewIdentifiedConnection { get; set; } =
        new(LogEventIds.MessengerListenerAcceptedNewIdentifiedConnection, LogLevel.Trace);

    public LogConfigurationBuilder LogDeclinedNewConnectionDuringDisposal { get; set; } =
        new(LogEventIds.MessengerListenerDeclinedNewConnectionDuringDisposal, LogLevel.Warning);

    public LogConfigurationBuilder LogDeclinedNewConnectionDueToException { get; set; } =
        new(LogEventIds.MessengerListenerDeclinedNewConnectionDueToException, LogLevel.Error);

    public bool LogExceptionWhenLoggingStoppedListeningDueToDisposal { get; set; }
    public string LoggingName { get; set; } = nameof(MessengerListener);
    public LogConfigurationBuilder LogStartedListening { get; set; } = new(LogEventIds.MessengerListenerStartedListening);
    public LogConfigurationBuilder LogStoppedListeningDueToDisposal { get; set; } = new(LogEventIds.MessengerListenerStoppedListeningDueToDisposal);
    public LogConfigurationBuilder LogStoppedListeningDueToException { get; set; } = new(LogEventIds.MessengerListenerStoppedListeningDueToException, LogLevel.Error);
    public LogConfigurationBuilder LogWasCreated { get; set; } = new(LogEventIds.MessengerListenerWasCreated);

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

    public override MessengerListenerConfiguration Build() => new(this);
}