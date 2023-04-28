﻿using JetBrains.Annotations;
using Microsoft.Extensions.Logging;
using MsbRpc.Logging;
using MsbRpc.Servers;

namespace MsbRpc.Configuration;

[PublicAPI]
public class ServerConfiguration : Configuration
{
    public const int DefaultListenBacklogSize = 100;
    public int ListenBacklogSize = DefaultListenBacklogSize;
    public LogConfiguration LogAcceptedNewConnection;
    public LogConfiguration LogDeclinedNewConnectionDuringDisposal;
    public bool LogExceptionWhenLoggingStoppedListeningDueToDisposal;

    /// <summary>
    ///     Prefix for log messages for easier identification
    /// </summary>
    public string LoggingName = nameof(Server);

    public LogConfiguration LogStartedListening;
    public LogConfiguration LogStoppedListeningDueToDisposal;
    public LogConfiguration LogStoppedListeningDueToException;
    public LogConfiguration LogWasCreatedWithEphemeralPort;
    public LogConfiguration LogWasCreatedWithSpecifiedPort;

    /// <summary>
    ///     The port to listen on for new connections. If 0, the OS will assign an ephemeral port.
    /// </summary>
    public int Port;

    /// <summary>
    ///     Thread name for easier identification, will have the port number appended to it
    /// </summary>
    public string ThreadName = nameof(Server);

    public ServerConfiguration()
    {
        LogStoppedListeningDueToException = new LogConfiguration(LogEventIds.ServerStoppedListeningDueToException, LogLevel.Error);
        LogStartedListening = new LogConfiguration(LogEventIds.ServerStartedListening, LogLevel.Information);
        LogStoppedListeningDueToDisposal = new LogConfiguration(LogEventIds.ServerStoppedListeningDueToDisposal, LogLevel.Information);
        LogExceptionWhenLoggingStoppedListeningDueToDisposal = false;
        LogWasCreatedWithEphemeralPort = new LogConfiguration(LogEventIds.ServerWasCreatedWithEphemeralPort, LogLevel.Information);
        LogWasCreatedWithSpecifiedPort = new LogConfiguration(LogEventIds.ServerWasCreatedWithSpecifiedPort, LogLevel.Information);
        LogAcceptedNewConnection = new LogConfiguration(LogEventIds.ServerAcceptedNewConnection, LogLevel.Trace);
        LogDeclinedNewConnectionDuringDisposal = new LogConfiguration(LogEventIds.ServerDeclinedNewConnectionDuringDisposal, LogLevel.Warning);
    }
}