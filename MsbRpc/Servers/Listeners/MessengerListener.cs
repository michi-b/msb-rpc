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

    public bool IsListening { get; private set; }

    /// <summary>
    ///     The endpoint this listener is listening on.
    ///     If the port is configured to be 0, this will be the actual endpoint the listener is listening on after it has
    ///     started listening.
    /// </summary>
    [PublicAPI]
    public IPEndPoint EndPoint { get; private set; }

    /// <summary>
    ///     The thread this listener is running on, or null if it is not running in a thread
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

    /// <summary>
    ///     listens blocking on the current thread
    /// </summary>
    [PublicAPI]
    public void Listen(IConnectionReceiver connectionReceiver)
    {
        ListenSocket = CreateAndBindListenSocket();
        Listen(ListenSocket, connectionReceiver);
        ListenThread = Thread.CurrentThread;
    }

    /// <summary>
    ///     starts listening asynchronously in a new task
    /// </summary>
    [PublicAPI]
    public async Task ListenAsync(IConnectionReceiver connectionReceiver, CancellationToken cancellationToken)
    {
        ListenSocket = CreateAndBindListenSocket();
        await ListenAsync(ListenSocket, connectionReceiver, cancellationToken);
    }

    /// <summary>
    ///     starts listening in a new thread
    /// </summary>
    [PublicAPI]
    public Thread StartListening(IConnectionReceiver connectionReceiver)
    {
        ListenSocket = CreateAndBindListenSocket();

        string threadName = $"{Configuration.ThreadName}:{EndPoint.Port}";

        Thread StartListenThread()
        {
            Thread listenThread = new(() => Listen(ListenSocket, connectionReceiver)) { Name = threadName };
            listenThread.Start();
            return listenThread;
        }

        ListenThread = ExecuteIfNotDisposed(StartListenThread);

        return ListenThread;
    }

    /// <exception cref="InvalidOperationException">operation is invalid if the listener is already listening</exception>
    /// <returns>the listen socket, which shall be assigned to <see cref="ListenSocket" /></returns>
    private Socket CreateAndBindListenSocket()
    {
        if (IsListening)
        {
            throw new InvalidOperationException($"{nameof(MessengerListener)} is already listening.");
        }

        var listenSocket = new Socket(IPAddress.Any.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
        listenSocket.Bind(EndPoint);
        EndPoint = (IPEndPoint)listenSocket.LocalEndPoint;
        ListenSocket = listenSocket;
        return listenSocket;
    }

    private async ValueTask ListenAsync(Socket socket, IConnectionReceiver connectionReceiver, CancellationToken cancellationToken)
    {
        try
        {
            using CancellationTokenRegistration cancellationTokenRegistration = cancellationToken.Register(Dispose);

            socket.Listen(Configuration.ListenBacklogSize);

            LogStartedListening();

            while (!IsDisposed)
            {
                Socket newConnectionSocket = await socket.AcceptAsync();

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
            if (HandleExceptionWhileListening(exception))
            {
                throw;
            }
        }
        finally
        {
            FinalizeListening();
        }
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
            if (HandleExceptionWhileListening(exception))
            {
                throw;
            }
        }
        finally
        {
            FinalizeListening();
        }
    }

    /// <returns>whether exception needs to be rethrown, i.e. escalated to outer scope</returns>
    private bool HandleExceptionWhileListening(Exception exception)
    {
        switch (exception)
        {
            case SocketException { SocketErrorCode: SocketError.Interrupted }:
            case SocketException { SocketErrorCode: SocketError.OperationAborted }:
            case ObjectDisposedException:
                if (IsDisposed)
                {
                    if (exception is SocketException socketException)
                    {
                        LogStoppedListeningDueToDisposal(socketException);
                    }
                    else
                    {
                        LogStoppedListeningDueToDisposal(exception);
                    }

                    return false;
                }

                LogStoppedListeningDueToException(exception);
                Dispose();
                return true;
            default:
                LogStoppedListeningDueToException(exception);
                Dispose();
                return true;
        }
    }

    private void FinalizeListening()
    {
        IsListening = false;
        ListenThread = null;
        ListenSocket?.Dispose();
        ListenSocket = null;
    }

    protected override void DisposeManagedResources()
    {
        ListenSocket?.Dispose();
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
                    "{LoggingName} started listening on port {Port} with a backlog of {BacklogSize}",
                    Configuration.LoggingName,
                    EndPoint.Port,
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

    private void LogStoppedListeningDueToDisposal(SocketException socketException)
    {
        if (Logger != null)
        {
            LogConfiguration configuration = Configuration.LogStoppedListeningDueToDisposal;
            if (Logger.GetIsEnabled(configuration))
            {
                SocketError socketError = socketException.SocketErrorCode;
                Logger.Log
                (
                    configuration.Level,
                    configuration.Id,
                    Configuration.LogExceptionThatStoppedListeningWhileDisposed ? socketException : null,
                    "{LoggingName} stopped listening due to disposal with socket error {SocketError} ({SocketErrorCode})",
                    Configuration.LoggingName,
                    socketError,
                    (int)socketError
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
                    Configuration.LogExceptionThatStoppedListeningWhileDisposed ? exception : null,
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