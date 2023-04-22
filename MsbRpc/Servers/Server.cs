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
using MsbRpc.Sockets;

namespace MsbRpc.Servers;

public abstract class Server : SelfLockingDisposable
{
    private readonly ServerConfiguration _configuration;
    private readonly Socket _listenSocket;

    private readonly ILogger<Server>? _logger;
    private readonly string _threadName;

    public readonly int Port;

    private Thread? _listenThread;

    protected Server(ServerConfiguration configuration)
    {
        _configuration = configuration;
        _logger = configuration.LoggerFactory?.CreateLogger<Server>();

        IPAddress localHost = Dns.GetHostEntry("localhost").AddressList[0];
        _listenSocket = new Socket(localHost.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
        _listenSocket.Bind(new IPEndPoint(localHost, configuration.Port));

        var listenEndPoint = _listenSocket.LocalEndPoint as IPEndPoint;
        Port = configuration.Port == 0 ? listenEndPoint!.Port : configuration.Port;
        _threadName = $"{_configuration.ThreadName}:{Port}";
        if (Port == 0)
        {
            LogWasCreatedWithEphemeralPort();
        }
        else
        {
            LogWasCreatedWithSpecificPort();
        }
    }

    [PublicAPI]
    public Thread Start()
    {
        if (_listenThread != null)
        {
            throw new InvalidOperationException($"{_threadName} is already started.");
        }

        Thread StartUnsafe()
        {
            _listenThread = new Thread(() => Run(_configuration.ListenBacklogSize)) { Name = _threadName };
            _listenThread.Start();
            return _listenThread;
        }

        return ExecuteIfNotDisposed(StartUnsafe);
    }

    protected override void DisposeManagedResources()
    {
        _listenSocket.Dispose();

        if (_listenThread != null)
        {
            _listenThread.Join();
            _listenThread = null;
        }

        base.DisposeManagedResources();
    }

    protected abstract void Accept(Messenger messenger);

    private void Run(int backlog)
    {
        try
        {
            _listenSocket.Listen(backlog);
            LogStartedListening();
            while (!IsDisposed)
            {
                Socket newConnectionSocket = _listenSocket.Accept();
                LogAcceptedNewConnection();

                //imagine this object is disposed while we are waiting for the lock
                //since disposal also locks this object and since disposal waits for this thread to finish, it will never release the lock
                bool earlyExitDueToDisposal = false;
                while (!Monitor.TryEnter(this, 1))
                {
                    if (IsDisposed)
                    {
                        try
                        {
                            newConnectionSocket.Dispose();
                            LogDisposedNewConnectionAfterDisposal();
                            break;
                        }
                        finally
                        {
                            earlyExitDueToDisposal = true;
                        }
                    }
                }

                //if we get here, we either have the lock or IsDisposed is true
                if (!earlyExitDueToDisposal)
                {
                    try
                    {
                        Accept(new Messenger(new RpcSocket(newConnectionSocket)));
                    }
                    finally
                    {
                        Monitor.Exit(this);
                    }
                }
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

    private void LogStoppedListeningDueToException(Exception exception)
    {
        if (_logger != null)
        {
            LogConfiguration configuration = _configuration.LogStoppedListeningDueToException;
            if (_logger.GetIsEnabled(configuration))
            {
                _logger.Log
                (
                    configuration.Level,
                    configuration.Id,
                    exception,
                    "{LoggingName} stopped listening due to an exception",
                    _configuration.LoggingName
                );
            }
        }
    }

    private void LogStartedListening()
    {
        if (_logger != null)
        {
            LogConfiguration configuration = _configuration.LogStartedListening;
            if (_logger.GetIsEnabled(configuration))
            {
                _logger.Log
                (
                    configuration.Level,
                    configuration.Id,
                    "{LoggingName} started listening",
                    _configuration.LoggingName
                );
            }
        }
    }

    private void LogStoppedListeningDueToDisposal(Exception exception)
    {
        if (_logger != null)
        {
            LogConfiguration configuration = _configuration.LogStoppedListeningDueToDisposal;
            if (_logger.GetIsEnabled(configuration))
            {
                _logger.Log
                (
                    configuration.Level,
                    configuration.Id,
                    exception,
                    "{LoggingName} stopped listening due to disposal",
                    _configuration.LoggingName
                );
            }
        }
    }

    private void LogWasCreatedWithEphemeralPort()
    {
        if (_logger != null)
        {
            LogConfiguration configuration = _configuration.LogWasCreatedWithEphemeralPort;
            if (_logger.GetIsEnabled(configuration))
            {
                _logger.Log
                (
                    configuration.Level,
                    configuration.Id,
                    "{LoggingName} was created with ephemeral port",
                    _configuration.LoggingName
                );
            }
        }
    }

    private void LogWasCreatedWithSpecificPort()
    {
        if (_logger != null)
        {
            LogConfiguration configuration = _configuration.LogWasCreatedWithSpecifiedPort;
            if (_logger.GetIsEnabled(configuration))
            {
                _logger.Log
                (
                    configuration.Level,
                    configuration.Id,
                    "{LoggingName} was created with specified port",
                    _configuration.LoggingName
                );
            }
        }
    }

    private void LogAcceptedNewConnection()
    {
        if (_logger != null)
        {
            LogConfiguration configuration = _configuration.LogAcceptedNewConnection;
            if (_logger.GetIsEnabled(configuration))
            {
                _logger.Log
                (
                    configuration.Level,
                    configuration.Id,
                    "{LoggingName} accepted new connection",
                    _configuration.LoggingName
                );
            }
        }
    }

    private void LogDisposedNewConnectionAfterDisposal()
    {
        if (_logger != null)
        {
            LogConfiguration configuration = _configuration.LogDisposedNewConnectionAfterDisposal;
            if (_logger.GetIsEnabled(configuration))
            {
                _logger.Log
                (
                    configuration.Level,
                    configuration.Id,
                    "{LoggingName} immediately disposed new connection because this server is disposed",
                    _configuration.LoggingName
                );
            }
        }
    }
}