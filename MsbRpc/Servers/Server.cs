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

public abstract class Server : ConcurrentDisposable
{
    private readonly Socket _listenSocket;

    private readonly ILogger<Server>? _logger;
    private readonly string _threadName;

    public readonly int Port;

    private Thread? _listenThread;

    [PublicAPI] protected ServerConfiguration Configuration { get; }

    protected Server(ServerConfiguration configuration)
    {
        Configuration = configuration;
        _logger = configuration.LoggerFactory?.CreateLogger<Server>();

        IPAddress localHost = Dns.GetHostEntry("localhost").AddressList[0];
        _listenSocket = new Socket(localHost.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
        _listenSocket.Bind(new IPEndPoint(localHost, configuration.Port));

        var listenEndPoint = _listenSocket.LocalEndPoint as IPEndPoint;
        Port = configuration.Port == 0 ? listenEndPoint!.Port : configuration.Port;
        _threadName = $"{Configuration.ThreadName}:{Port}";
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
            _listenThread = new Thread(() => Run(Configuration.ListenBacklogSize)) { Name = _threadName };
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

                void AcceptUnsafe()
                {
                    Accept(new Messenger(new RpcSocket(newConnectionSocket)));
                    LogAcceptedNewConnection();
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

    private void LogStoppedListeningDueToException(Exception exception)
    {
        if (_logger != null)
        {
            LogConfiguration configuration = Configuration.LogStoppedListeningDueToException;
            if (_logger.GetIsEnabled(configuration))
            {
                _logger.Log
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

    private void LogStartedListening()
    {
        if (_logger != null)
        {
            LogConfiguration configuration = Configuration.LogStartedListening;
            if (_logger.GetIsEnabled(configuration))
            {
                _logger.Log
                (
                    configuration.Level,
                    configuration.Id,
                    "{LoggingName} started listening",
                    Configuration.LoggingName
                );
            }
        }
    }

    private void LogStoppedListeningDueToDisposal(Exception exception)
    {
        if (_logger != null)
        {
            LogConfiguration configuration = Configuration.LogStoppedListeningDueToDisposal;
            if (_logger.GetIsEnabled(configuration))
            {
                _logger.Log
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

    private void LogWasCreatedWithEphemeralPort()
    {
        if (_logger != null)
        {
            LogConfiguration configuration = Configuration.LogWasCreatedWithEphemeralPort;
            if (_logger.GetIsEnabled(configuration))
            {
                _logger.Log
                (
                    configuration.Level,
                    configuration.Id,
                    "{LoggingName} was created with ephemeral port",
                    Configuration.LoggingName
                );
            }
        }
    }

    private void LogWasCreatedWithSpecificPort()
    {
        if (_logger != null)
        {
            LogConfiguration configuration = Configuration.LogWasCreatedWithSpecifiedPort;
            if (_logger.GetIsEnabled(configuration))
            {
                _logger.Log
                (
                    configuration.Level,
                    configuration.Id,
                    "{LoggingName} was created with specified port",
                    Configuration.LoggingName
                );
            }
        }
    }

    private void LogAcceptedNewConnection()
    {
        if (_logger != null)
        {
            LogConfiguration configuration = Configuration.LogAcceptedNewConnection;
            if (_logger.GetIsEnabled(configuration))
            {
                _logger.Log
                (
                    configuration.Level,
                    configuration.Id,
                    "{LoggingName} accepted new connection",
                    Configuration.LoggingName
                );
            }
        }
    }

    private void LogDeclinedNewConnectionDuringDisposal()
    {
        if (_logger != null)
        {
            LogConfiguration configuration = Configuration.LogDeclinedNewConnectionDuringDisposal;
            if (_logger.GetIsEnabled(configuration))
            {
                _logger.Log
                (
                    configuration.Level,
                    configuration.Id,
                    "{LoggingName} immediately disposed new connection because this server is disposed",
                    Configuration.LoggingName
                );
            }
        }
    }
}