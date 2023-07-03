using Microsoft.Extensions.Logging;
using MsbRpc.Configuration.Builders;

namespace MsbRpc.Configuration;

public readonly struct ConnectionListenerConfiguration
{
    public readonly ILoggerFactory? LoggerFactory;

    public readonly LogConfiguration LogAcceptedNewUnIdentifiedConnection;

    public readonly LogConfiguration LogAcceptedNewIdentifiedConnection;

    public readonly LogConfiguration LogDeclinedNewConnectionDuringDisposal;

    public readonly LogConfiguration LogDeclinedNewConnectionDueToException;
    public readonly LogConfiguration LogStoppedListeningDueToDisposal;

    public readonly LogConfiguration LogStoppedListeningDueToException;

    public readonly LogConfiguration LogStartedListening;

    public readonly LogConfiguration LogWasCreatedWithEphemeralPort;

    public readonly LogConfiguration LogWasCreatedWithSpecifiedPort;

    public readonly bool LogExceptionWhenLoggingStoppedListeningDueToDisposal;

    public readonly int ListenBacklogSize;

    public readonly string LoggingName;
    public readonly int Port;

    public readonly string ThreadName;

    public ConnectionListenerConfiguration(ConnectionListenerConfigurationBuilder builder)
    {
        LoggerFactory = builder.LoggerFactory;
        ListenBacklogSize = builder.ListenBacklogSize;
        LogAcceptedNewUnIdentifiedConnection = builder.LogAcceptedNewUnIdentifiedConnection;
        LogAcceptedNewIdentifiedConnection = builder.LogAcceptedNewIdentifiedConnection;
        LogDeclinedNewConnectionDuringDisposal = builder.LogDeclinedNewConnectionDuringDisposal;
        LogExceptionWhenLoggingStoppedListeningDueToDisposal = builder.LogExceptionWhenLoggingStoppedListeningDueToDisposal;
        LoggingName = builder.LoggingName;
        LogStartedListening = builder.LogStartedListening;
        LogStoppedListeningDueToDisposal = builder.LogStoppedListeningDueToDisposal;
        LogDeclinedNewConnectionDueToException = builder.LogDeclinedNewConnectionDueToException;
        LogStoppedListeningDueToException = builder.LogStoppedListeningDueToException;
        LogWasCreatedWithEphemeralPort = builder.LogWasCreatedWithEphemeralPort;
        LogWasCreatedWithSpecifiedPort = builder.LogWasCreatedWithSpecifiedPort;
        Port = builder.Port;
        ThreadName = builder.ThreadName;
    }
}