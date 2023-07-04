using System;
using System.Collections.Concurrent;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using JetBrains.Annotations;
using Microsoft.Extensions.Logging;
using MsbRpc.Configuration;
using MsbRpc.Disposable;
using MsbRpc.Extensions;
using MsbRpc.Messaging;
using MsbRpc.Network;
using MsbRpc.Serialization.Buffers;
using MsbRpc.Sockets;

namespace MsbRpc.Servers.Listener;

public class MessengerListener : ConcurrentDisposable
{
    private readonly RpcBuffer _initialConnectionMessageBuffer = new(InitialConnectionMessage.MessageMaxSize);
    private readonly ConcurrentDictionary<int, MessengerListenTask> _listenTasks = new();
    private readonly ILogger? _logger;
    private readonly MessengerListenerConfiguration _serverConfiguration;
    private readonly Socket _socket;
    private readonly IMessengerReceiver _unIdentifiedMessengerReceiver;

    [PublicAPI] public IPEndPoint EndPoint { get; }
    [PublicAPI] public Thread Thread { get; private set; }

    private MessengerListener(Socket socket, MessengerListenerConfiguration configuration, IMessengerReceiver unIdentifiedMessengerReceiver)
    {
        _socket = socket;
        _serverConfiguration = configuration;
        _unIdentifiedMessengerReceiver = unIdentifiedMessengerReceiver;

        EndPoint = (IPEndPoint)socket.LocalEndPoint;

        _logger = configuration.LoggerFactory?.CreateLogger<MessengerListener>();

        LogWasCreated();

        // thread will always be set by public static "Run" method, which is the only non-private way to construct this class
        Thread = null!;
    }

    public static MessengerListener Start(MessengerListenerConfiguration configuration, IMessengerReceiver unIdentifiedMessengerReceiver)
    {
        var socket = new Socket(IPAddress.Any.AddressFamily, SocketType.Stream, ProtocolType.Tcp);

        socket.Bind(new IPEndPoint(NetworkUtility.GetLocalHost(), configuration.Port));

        MessengerListener listener = new(socket, configuration, unIdentifiedMessengerReceiver);

        string threadName = $"{listener._serverConfiguration.ThreadName}:{listener.EndPoint.Port}";

        Thread StartListenThread()
        {
            Thread listenThread = new(() => listener.Start()) { Name = threadName };
            listenThread.Start();
            return listenThread;
        }

        listener.Thread = listener.ExecuteIfNotDisposed(StartListenThread);

        return listener;
    }

    protected override void DisposeManagedResources()
    {
        _socket.Dispose();
    }

    private void Start()
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
                    Accept(newConnectionSocket);
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

            InitialConnectionMessage connectionMessage = await messenger.ReceiveInitialConnectionMessageAsync(_initialConnectionMessageBuffer);

            switch (connectionMessage.MessengerType)
            {
                case MessengerType.UnIdentified:
                    _unIdentifiedMessengerReceiver.AcceptUnIdentified(messenger);
                    LogAcceptedNewUnIdentifiedConnection();
                    break;
                case MessengerType.Identified:
                    if (connectionMessage.Id != null)
                    {
                        if (_listenTasks.TryRemove(connectionMessage.Id.Value, out MessengerListenTask listenTask))
                        {
                            listenTask.Fullfill(messenger);
                            LogAcceptedNewIdentifiedConnection(connectionMessage.Id.Value);
                        }
                        else
                        {
                            throw new IdentifiedMessengerException
                            (
                                connectionMessage,
                                $"registered listen tasks do not contain an entry for the id {connectionMessage.Id}"
                            );
                        }
                    }
                    else
                    {
                        throw new IdentifiedMessengerException(connectionMessage, "connection message is marked to be identified but has no ID");
                    }

                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(MessengerType));
            }
        }
        catch (Exception e)
        {
            messengerForExceptionHandling?.Dispose();
            LogDeclinedNewConnectionDueToException(e);
        }
    }

    private void LogWasCreated()
    {
        if (_logger != null)
        {
            LogConfiguration configuration = _serverConfiguration.LogWasCreated;
            if (_logger.GetIsEnabled(configuration))
            {
                _logger.Log
                (
                    configuration.Level,
                    configuration.Id,
                    "{LoggingName} was created at {EndPoint}",
                    _serverConfiguration.LoggingName,
                    EndPoint
                );
            }
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
                    "{LoggingName} started listening with a backlog of {BacklogSize}",
                    _serverConfiguration.LoggingName,
                    _serverConfiguration.ListenBacklogSize
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
                    "{LoggingName} accepted new unidentified messenger",
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
                    "{LoggingName} accepted new identified messenger with ID {ConnectionId}",
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
                    "{LoggingName} immediately disposed new connection because it is disposed",
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
                    "{LoggingName} immediately disposed new connection due to exception",
                    _serverConfiguration.LoggingName
                );
            }
        }
    }
}