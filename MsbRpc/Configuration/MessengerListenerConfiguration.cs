#region

using Microsoft.Extensions.Logging;
using MsbRpc.Configuration.Builders;

#endregion

namespace MsbRpc.Configuration;

public readonly struct MessengerListenerConfiguration
{
    public readonly ILoggerFactory? LoggerFactory;
    public readonly LogConfiguration LogAcceptedNewUnIdentifiedConnection;
    public readonly LogConfiguration LogCompletedIdentifiedConnectionTask;
    public readonly LogConfiguration LogAcceptedNewIdentifiedConnection;
    public readonly LogConfiguration LogDeclinedNewConnectionDuringDisposal;
    public readonly LogConfiguration LogDeclinedNewConnectionDueToException;
    public readonly LogConfiguration LogStoppedListeningDueToDisposal;
    public readonly bool LogExceptionThatStoppedListeningWhileDisposed;
    public readonly LogConfiguration LogStoppedListeningDueToException;
    public readonly LogConfiguration LogStartedListening;
    public readonly LogConfiguration LogWasCreated;

    public readonly int ListenBacklogSize;
    public readonly string LoggingName;
    public readonly int Port;
    public readonly string ThreadName;

    public MessengerListenerConfiguration(MessengerListenerConfigurator builder)
    {
        LoggerFactory = builder.LoggerFactory;
        ListenBacklogSize = builder.ListenBacklogSize;
        LogAcceptedNewUnIdentifiedConnection = builder.LogAcceptedNewUnIdentifiedConnection;
        LogCompletedIdentifiedConnectionTask = builder.LogCompletedIdentifiedConnectionTask;
        LogAcceptedNewIdentifiedConnection = builder.LogAcceptedNewIdentifiedConnection;
        LogDeclinedNewConnectionDuringDisposal = builder.LogDeclinedNewConnectionDuringDisposal;
        LogExceptionThatStoppedListeningWhileDisposed = builder.LogExceptionThatStoppedListeningWhileDisposed;
        LoggingName = builder.LoggingName;
        LogStartedListening = builder.LogStartedListening;
        LogStoppedListeningDueToDisposal = builder.LogStoppedListeningDueToDisposal;
        LogDeclinedNewConnectionDueToException = builder.LogDeclinedNewConnectionDueToException;
        LogStoppedListeningDueToException = builder.LogStoppedListeningDueToException;
        LogWasCreated = builder.LogWasCreated;
        Port = builder.Port;
        ThreadName = builder.ThreadName;
    }
}