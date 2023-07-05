using System;
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
    private readonly ConnectionTaskRegistry _connectionTasks = new();
    private readonly RpcBuffer _initialConnectionMessageBuffer = new(ConnectionRequest.MessageMaxSize);
    private readonly ILogger? _logger;
    private readonly MessengerListenerConfiguration _serverConfiguration;
    private readonly Socket _socket;
    private readonly IConnectionReceiver _unIdentifiedConnectionReceiver;

    [PublicAPI] public IPEndPoint EndPoint { get; }
    [PublicAPI] public Thread Thread { get; private set; }

    private MessengerListener(Socket socket, MessengerListenerConfiguration configuration, IConnectionReceiver unIdentifiedConnectionReceiver)
    {
        _socket = socket;
        _serverConfiguration = configuration;
        _unIdentifiedConnectionReceiver = unIdentifiedConnectionReceiver;

        EndPoint = (IPEndPoint)socket.LocalEndPoint;

        _logger = configuration.LoggerFactory?.CreateLogger<MessengerListener>();

        LogWasCreated();

        // thread will always be set by public static "Run" method, which is the only non-private way to construct this class
        Thread = null!;
    }

    public static MessengerListener Start(MessengerListenerConfiguration configuration, IConnectionReceiver unIdentifiedConnectionReceiver)
    {
        var socket = new Socket(IPAddress.Any.AddressFamily, SocketType.Stream, ProtocolType.Tcp);

        socket.Bind(new IPEndPoint(NetworkUtility.GetLocalHost(), configuration.Port));

        MessengerListener listener = new(socket, configuration, unIdentifiedConnectionReceiver);

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

    /// <summary>
    ///     schedules awaiting an identified connection that can be received by calling <see cref="Await(int, int)" />
    /// </summary>
    /// <returns>
    ///     the id of the scheduled connection that can be used to receive the connection by calling
    ///     <see cref="Await(int, int)" />
    /// </returns>
    public int ScheduleIdentifiedConnection() => _connectionTasks.Schedule();

    /// <summary>
    ///     awaits an identified connection task with the given id that can be retrieved by calling
    ///     <see cref="ScheduleIdentifiedConnection" />
    /// </summary>
    /// <param name="identifiedConnectionId">
    ///     the id of the connection task that was scheduled by calling <see cref="ScheduleIdentifiedConnection" />
    /// </param>
    /// <param name="millisecondsTimeout">
    ///     the timeout in milliseconds after which the method will throw a <see cref="TimeoutException" />
    /// </param>
    /// <exception cref="TimeoutException"></exception>
    /// <returns></returns>
    public Messenger Await(int identifiedConnectionId, int millisecondsTimeout = ConnectionTask.DefaultMillisecondsTimeout)
    {
        Messenger messenger = _connectionTasks.Await(identifiedConnectionId, millisecondsTimeout);
        LogAcceptedNewIdentifiedConnection(identifiedConnectionId);
        return messenger;
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

            ConnectionRequest connectionRequest = await messenger.ReceiveConnectionRequestAsync(_initialConnectionMessageBuffer);

            switch (connectionRequest.ConnectionRequestType)
            {
                case ConnectionRequestType.UnIdentified:
                    _unIdentifiedConnectionReceiver.AcceptUnIdentified(messenger);
                    LogAcceptedNewUnIdentifiedConnection();
                    break;
                case ConnectionRequestType.Identified:
                    if (connectionRequest.Id != null)
                    {
                        try
                        {
                            _connectionTasks.Complete(connectionRequest.Id.Value, messenger);
                            LogCompletedIdentifiedConnectionTask(connectionRequest.Id.Value);
                        }
                        catch (Exception e)
                        {
                            throw new InvalidIdentifiedConnectionRequestException
                            (
                                connectionRequest,
                                $"registered listen tasks do not contain an entry for the id {connectionRequest.Id}",
                                e
                            );
                        }
                    }
                    else
                    {
                        throw new InvalidIdentifiedConnectionRequestException(connectionRequest, "connection message is marked to be identified but has no ID");
                    }

                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(ConnectionRequestType));
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

    private void LogCompletedIdentifiedConnectionTask(int id)
    {
        if (_logger != null)
        {
            LogConfiguration configuration = _serverConfiguration.LogCompletedIdentifiedConnectionTask;
            if (_logger.GetIsEnabled(configuration))
            {
                _logger.Log
                (
                    configuration.Level,
                    configuration.Id,
                    "{LoggingName} completed identified connection task with id {ConnectionId}",
                    _serverConfiguration.LoggingName,
                    id
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