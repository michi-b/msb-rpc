using System;
using System.Collections.Concurrent;
using System.Net.Sockets;
using System.Threading;
using JetBrains.Annotations;
using Microsoft.Extensions.Logging;
using MsbRpc.Configuration;
using MsbRpc.Disposable;
using MsbRpc.Extensions;
using MsbRpc.Messaging;
using MsbRpc.Sockets;

namespace MsbRpc.Servers.Listener;

public class ConnectionListener : ConcurrentDisposable
{
    private readonly ConcurrentDictionary<int, ListenTask> _listenTasks = new();

    private readonly ILogger? _logger;
    private readonly ServerConfiguration _serverConfiguration;
    private readonly Socket _socket;
    private readonly IUnIdentifiedConnectionReceiver _unIdentifiedConnectionReceiver;

    [PublicAPI] public Thread Thread { get; private set; }

    private ConnectionListener(Socket socket, ServerConfiguration configuration, IUnIdentifiedConnectionReceiver unIdentifiedConnectionReceiver, ILogger? logger)
    {
        _logger = logger;
        _socket = socket;
        _serverConfiguration = configuration;
        _unIdentifiedConnectionReceiver = unIdentifiedConnectionReceiver;

        // thread will always be set by public static "Run" method, which is the only non-private way to construct this class
        Thread = null!;
    }

    public static ConnectionListener Run(Socket socket, ServerConfiguration configuration, string threadName, IUnIdentifiedConnectionReceiver receiver, ILogger? logger)
    {
        ConnectionListener listener = new(socket, configuration, receiver, logger);

        Thread Start()
        {
            Thread listenThread = new(() => listener.Run()) { Name = threadName };
            listenThread.Start();
            return listenThread;
        }

        listener.Thread = listener.ExecuteIfNotDisposed(Start);

        return listener;
    }

    private void Run()
    {
        try
        {
            _socket.Listen(_serverConfiguration.ListenBacklogSize);
            LogStartedListening();
            while (!IsDisposed)
            {
                Socket newConnectionSocket = _socket.Accept();

                void AcceptUnsafe()
                {
                    Accept(_socket);
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
    }

    private async void Accept(Socket socket)
    {
        Messenger? messengerForExceptionHandling = null;
        try
        {
            Messenger messenger = new(new RpcSocket(socket));

            messengerForExceptionHandling = messenger;

            int timeOut = 10000; //TODO: control timeout via configuration

            CancellationToken cancellationToken = new CancellationTokenSource(timeOut).Token;
            cancellationToken.Register(() => messenger.Dispose());

            InitialConnectionMessage connectionMessage = await messenger.ReceiveInitialConnectionMessageAsync();

            switch (connectionMessage.ConnectionType)
            {
                case ConnectionType.UnIdentified:
                    _unIdentifiedConnectionReceiver.AcceptUnIdentified(messenger);
                    LogAcceptedNewUnIdentifiedConnection();
                    break;
                case ConnectionType.Identified:
                    if (connectionMessage.Id != null)
                    {
                        if (_listenTasks.TryRemove(connectionMessage.Id.Value, out ListenTask listenTask))
                        {
                            listenTask.Fullfill(messenger);
                            LogAcceptedNewIdentifiedConnection(connectionMessage.Id.Value);
                        }
                        else
                        {
                            throw new IdentifiedConnectionAcceptanceException
                            (
                                connectionMessage,
                                $"registered listen tasks do not contain an entry for the id {connectionMessage.Id}"
                            );
                        }
                    }
                    else
                    {
                        throw new IdentifiedConnectionAcceptanceException(connectionMessage, "connection message is marked to be identified but has no ID");
                    }

                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(ConnectionType));
            }
        }
        catch (Exception e)
        {
            messengerForExceptionHandling?.Dispose();
            LogDeclinedNewConnectionDueToException(e);
        }
    }

    private void LogStartedListening()
    {
        if (_logger != null)
        {
            LogConfiguration configuration = _serverConfiguration.LogStartedListening;
            if (_logger.GetIsEnabled(configuration))
            {
                _logger.Log
                (
                    configuration.Level,
                    configuration.Id,
                    "{LoggingName} started listening",
                    _serverConfiguration.LoggingName
                );
            }
        }
    }

    private void LogStoppedListeningDueToException(Exception exception)
    {
        if (_logger != null)
        {
            LogConfiguration configuration = _serverConfiguration.LogStoppedListeningDueToException;
            if (_logger.GetIsEnabled(configuration))
            {
                _logger.Log
                (
                    configuration.Level,
                    configuration.Id,
                    exception,
                    "{LoggingName} stopped listening due to an exception",
                    _serverConfiguration.LoggingName
                );
            }
        }
    }

    private void LogStoppedListeningDueToDisposal(Exception exception)
    {
        if (_logger != null)
        {
            LogConfiguration configuration = _serverConfiguration.LogStoppedListeningDueToDisposal;
            if (_logger.GetIsEnabled(configuration))
            {
                _logger.Log
                (
                    configuration.Level,
                    configuration.Id,
                    _serverConfiguration.LogExceptionWhenLoggingStoppedListeningDueToDisposal ? exception : null,
                    "{LoggingName} stopped listening due to disposal",
                    _serverConfiguration.LoggingName
                );
            }
        }
    }

    private void LogAcceptedNewUnIdentifiedConnection()
    {
        if (_logger != null)
        {
            LogConfiguration configuration = _serverConfiguration.LogAcceptedNewUnIdentifiedConnection;
            if (_logger.GetIsEnabled(configuration))
            {
                _logger.Log
                (
                    configuration.Level,
                    configuration.Id,
                    "{LoggingName} accepted new unidentified connection",
                    _serverConfiguration.LoggingName
                );
            }
        }
    }

    private void LogAcceptedNewIdentifiedConnection(int id)
    {
        if (_logger != null)
        {
            LogConfiguration configuration = _serverConfiguration.LogAcceptedNewIdentifiedConnection;
            if (_logger.GetIsEnabled(configuration))
            {
                _logger.Log
                (
                    configuration.Level,
                    configuration.Id,
                    "{LoggingName} accepted new identified connection with connection ID {ConnectionId}",
                    _serverConfiguration.LoggingName,
                    id
                );
            }
        }
    }

    private void LogDeclinedNewConnectionDuringDisposal()
    {
        if (_logger != null)
        {
            LogConfiguration configuration = _serverConfiguration.LogDeclinedNewConnectionDuringDisposal;
            if (_logger.GetIsEnabled(configuration))
            {
                _logger.Log
                (
                    configuration.Level,
                    configuration.Id,
                    "{LoggingName} immediately disposed new connection because this server is disposed",
                    _serverConfiguration.LoggingName
                );
            }
        }
    }

    private void LogDeclinedNewConnectionDueToException(Exception exception)
    {
        if (_logger != null)
        {
            LogConfiguration configuration = _serverConfiguration.LogDeclinedNewConnectionDueToException;
            if (_logger.GetIsEnabled(configuration))
            {
                _logger.Log
                (
                    configuration.Level,
                    configuration.Id,
                    exception,
                    "{LoggingName} immediately disposed new connection because this server is disposed",
                    _serverConfiguration.LoggingName
                );
            }
        }
    }
}