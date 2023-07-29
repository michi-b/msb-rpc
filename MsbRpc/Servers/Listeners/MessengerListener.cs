#region

using System;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;
using JetBrains.Annotations;
using Microsoft.Extensions.Logging;
using MsbRpc.Configuration;
using MsbRpc.Disposable;
using MsbRpc.Extensions;
using MsbRpc.Messaging;
using MsbRpc.Network;
using MsbRpc.Servers.Listeners.Connections;
using MsbRpc.Sockets;

#endregion

namespace MsbRpc.Servers.Listeners;

public class MessengerListener : ConcurrentDisposable
{
    protected readonly MessengerListenerConfiguration Configuration;

    protected readonly ILogger? Logger;

    private bool _isListening;

    /// <summary>
    ///     The endpoint this listener is listening on.
    ///     Modifying it after starting listening has no effect.
    /// </summary>
    [PublicAPI]
    public IPEndPoint EndPoint { get; set; }

    /// <summary>
    ///     The thread this listener is running on, or null if it is not running
    /// </summary>
    [PublicAPI]
    public Thread? ListenThread { get; private set; }

    /// <summary>
    ///     The socket this listener is listening on, or null if it is not listening
    /// </summary>
    [PublicAPI]
    public Socket? ListenSocket { get; private set; }

    public MessengerListener(MessengerListenerConfiguration configuration)
    {
        Configuration = configuration;
        Logger = configuration.LoggerFactory?.CreateLogger<MessengerListener>();
        EndPoint = new IPEndPoint(NetworkUtility.GetLocalHost(), Configuration.Port);
        LogWasCreated();
        ListenThread = null;
    }

    public void StartListen(IConnectionReceiver connectionReceiver)
    {
        if (_isListening)
        {
            throw new InvalidOperationException($"{nameof(MessengerListener)} is already listening.");
        }

        ListenSocket = new Socket(IPAddress.Any.AddressFamily, SocketType.Stream, ProtocolType.Tcp);

        ListenSocket.Bind(EndPoint);

        string threadName = $"{Configuration.ThreadName}:{EndPoint.Port}";

        Thread StartListenThread()
        {
            Thread listenThread = new(() => Listen(ListenSocket, connectionReceiver)) { Name = threadName };
            listenThread.Start();
            return listenThread;
        }

        ListenThread = ExecuteIfNotDisposed(StartListenThread);
        _isListening = true;
    }

    protected override void DisposeManagedResources()
    {
        ListenSocket?.Dispose();
    }

    private void Listen(Socket socket, IConnectionReceiver connectionReceiver)
    {
        try
        {
            socket.Listen(Configuration.ListenBacklogSize);

            LogStartedListening();

            while (!IsDisposed)
            {
                Socket newConnectionSocket = socket.Accept();

                void AcceptUnsafe()
                {
                    Accept(newConnectionSocket, connectionReceiver);
                }

                void Decline()
                {
                    newConnectionSocket.Dispose();
                    LogDeclinedNewConnectionDuringDisposal();
                }

                ExecuteIfNotDisposed(AcceptUnsafe, alternativeAction: Decline, throwObjectDisposedExceptionOtherwise: false);
            }
        }
        catch (Exception exception)
        {
            switch (exception)
            {
                case SocketException { SocketErrorCode: SocketError.Interrupted }:
                case ObjectDisposedException:
                    if (IsDisposed)
                    {
                        LogStoppedListeningDueToDisposal(exception);
                    }
                    else
                    {
                        LogStoppedListeningDueToException(exception);
                        Dispose();
                        throw;
                    }

                    return;
                default:
                    LogStoppedListeningDueToException(exception);
                    Dispose();
                    throw;
            }
        }
        finally
        {
            _isListening = false;
            ListenThread = null;
            ListenSocket?.Dispose();
            ListenSocket = null;
        }
    }

    private async void Accept(Socket socket, IConnectionReceiver connectionReceiver)
    {
        try
        {
            Messenger messenger = new(new RpcSocket(socket));
            bool intercepted = await Accept(messenger);
            if (!intercepted)
            {
                connectionReceiver.Accept(messenger);
                LogAcceptedNewUnIdentifiedConnection();
            }
        }
        catch (Exception e)
        {
            LogDeclinedNewConnectionDueToException(e);
        }
    }

    /// <returns>whether the messenger was accepted non-unidentified (unidentified messenger acceptance was intercepted)</returns>
    protected virtual Task<bool> Accept(Messenger messenger) => Task.FromResult(false);

    private void LogWasCreated()
    {
        if (Logger != null)
        {
            LogConfiguration configuration = Configuration.LogWasCreated;
            if (Logger.GetIsEnabled(configuration))
            {
                Logger.Log
                (
                    configuration.Level,
                    configuration.Id,
                    "{LoggingName} was created at {EndPoint}",
                    Configuration.LoggingName,
                    EndPoint
                );
            }
        }
    }

    private void LogStartedListening()
    {
        if (Logger != null)
        {
            LogConfiguration configuration = Configuration.LogStartedListening;
            if (Logger.GetIsEnabled(configuration))
            {
                Logger.Log
                (
                    configuration.Level,
                    configuration.Id,
                    "{LoggingName} started listening with a backlog of {BacklogSize}",
                    Configuration.LoggingName,
                    Configuration.ListenBacklogSize
                );
            }
        }
    }

    private void LogStoppedListeningDueToException(Exception exception)
    {
        if (Logger != null)
        {
            LogConfiguration configuration = Configuration.LogStoppedListeningDueToException;
            if (Logger.GetIsEnabled(configuration))
            {
                Logger.Log
                (
                    configuration.Level,
                    configuration.Id,
                    exception,
                    "{LoggingName} stopped listening due to an exception",
                    Configuration.LoggingName
                );
            }
        }
    }

    private void LogStoppedListeningDueToDisposal(Exception exception)
    {
        if (Logger != null)
        {
            LogConfiguration configuration = Configuration.LogStoppedListeningDueToDisposal;
            if (Logger.GetIsEnabled(configuration))
            {
                Logger.Log
                (
                    configuration.Level,
                    configuration.Id,
                    Configuration.LogExceptionWhenLoggingStoppedListeningDueToDisposal ? exception : null,
                    "{LoggingName} stopped listening due to disposal",
                    Configuration.LoggingName
                );
            }
        }
    }

    private void LogAcceptedNewUnIdentifiedConnection()
    {
        if (Logger != null)
        {
            LogConfiguration configuration = Configuration.LogAcceptedNewUnIdentifiedConnection;
            if (Logger.GetIsEnabled(configuration))
            {
                Logger.Log
                (
                    configuration.Level,
                    configuration.Id,
                    "{LoggingName} accepted new unidentified messenger",
                    Configuration.LoggingName
                );
            }
        }
    }

    internal void LogAcceptedNewIdentifiedConnection(int id)
    {
        if (Logger != null)
        {
            LogConfiguration configuration = Configuration.LogAcceptedNewIdentifiedConnection;
            if (Logger.GetIsEnabled(configuration))
            {
                Logger.Log
                (
                    configuration.Level,
                    configuration.Id,
                    "{LoggingName} accepted new identified messenger with ID {ConnectionId}",
                    Configuration.LoggingName,
                    id
                );
            }
        }
    }

    private void LogDeclinedNewConnectionDuringDisposal()
    {
        if (Logger != null)
        {
            LogConfiguration configuration = Configuration.LogDeclinedNewConnectionDuringDisposal;
            if (Logger.GetIsEnabled(configuration))
            {
                Logger.Log
                (
                    configuration.Level,
                    configuration.Id,
                    "{LoggingName} immediately disposed new connection because it is disposed",
                    Configuration.LoggingName
                );
            }
        }
    }

    private void LogDeclinedNewConnectionDueToException(Exception exception)
    {
        if (Logger != null)
        {
            LogConfiguration configuration = Configuration.LogDeclinedNewConnectionDueToException;
            if (Logger.GetIsEnabled(configuration))
            {
                Logger.Log
                (
                    configuration.Level,
                    configuration.Id,
                    exception,
                    "{LoggingName} immediately disposed new connection due to exception",
                    Configuration.LoggingName
                );
            }
        }
    }
}