using JetBrains.Annotations;
using Microsoft.Extensions.Logging;
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

    public ServerConfiguration
    (
        string threadName,
        string loggingName,
        int port,
        int listenBacklogSize,
        ILoggerFactory? loggerFactory,
        LogConfiguration logWasCreatedWithSpecifiedPort,
        LogConfiguration logWasCreatedWithEphemeralPort,
        LogConfiguration logStartedListening,
        LogConfiguration logAcceptedNewConnection,
        LogConfiguration logDeclinedNewConnectionDuringDisposal,
        LogConfiguration logStoppedListeningDueToDisposal,
        LogConfiguration logStoppedListeningDueToException,
        bool logExceptionWhenLoggingStoppedListeningDueToDisposal
    ) : base(loggerFactory)
    {
        ListenBacklogSize = listenBacklogSize;
        LogAcceptedNewConnection = logAcceptedNewConnection;
        LogDeclinedNewConnectionDuringDisposal = logDeclinedNewConnectionDuringDisposal;
        LogExceptionWhenLoggingStoppedListeningDueToDisposal = logExceptionWhenLoggingStoppedListeningDueToDisposal;
        LoggingName = loggingName;
        LogStartedListening = logStartedListening;
        LogStoppedListeningDueToDisposal = logStoppedListeningDueToDisposal;
        LogStoppedListeningDueToException = logStoppedListeningDueToException;
        LogWasCreatedWithEphemeralPort = logWasCreatedWithEphemeralPort;
        LogWasCreatedWithSpecifiedPort = logWasCreatedWithSpecifiedPort;
        Port = port;
        ThreadName = threadName;
    }
}