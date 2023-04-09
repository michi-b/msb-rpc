using System;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using JetBrains.Annotations;
using Microsoft.Extensions.Logging;
using MsbRpc.Configuration;
using MsbRpc.Extensions;
using MsbRpc.Messaging;
using MsbRpc.Sockets;

namespace MsbRpc.Servers;

public abstract class Server : IDisposable
{
    private readonly ServerConfiguration _configuration;
    private readonly object _disposeLock = new();
    private readonly Socket _listenSocket;

    private readonly ILogger<Server>? _logger;
    private readonly string _threadName;
    private readonly string _typeName;

    public readonly int Port;

    private bool _isDisposed;
    private Thread? _listenThread;

    protected Server(ServerConfiguration configuration)
    {
        _configuration = configuration;
        _logger = configuration.LoggerFactory?.CreateLogger<Server>();

        IPAddress localHost = Dns.GetHostEntry("localhost").AddressList[0];
        _listenSocket = new Socket(localHost.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
        _listenSocket.Bind(new IPEndPoint(localHost, configuration.Port));

        _typeName = GetType().Name;

        var listenEndPoint = _listenSocket.LocalEndPoint as IPEndPoint;
        Port = configuration.Port == 0 ? listenEndPoint!.Port : configuration.Port;
        _threadName = $"{_typeName}:{Port}";
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

        if (_isDisposed)
        {
            throw new ObjectDisposedException(_threadName);
        }

        lock (_disposeLock)
        {
            if (_isDisposed)
            {
                throw new ObjectDisposedException(_threadName);
            }

            _listenThread = new Thread(() => Run(_configuration.ListenBacklogSize)) { Name = _threadName };
            _listenThread.Start();
            return _listenThread;
        }
    }

    public void Dispose()
    {
        if (!_isDisposed)
        {
            lock (_disposeLock)
            {
                if (!_isDisposed)
                {
                    Dispose(true);
                }
            }
        }

        GC.SuppressFinalize(this);
    }

    protected virtual void Dispose(bool disposing)
    {
        //need to set this before disposing the socket, as the socket's dispose method will dispose the server in return otherwise, causing a deadlock
        _isDisposed = true;

        if (disposing)
        {
            _listenSocket.Dispose();

            if (_listenThread != null)
            {
                _listenThread.Join();
                _listenThread = null;
            }
        }
    }

    protected abstract void Accept(Messenger messenger);

    private void Run(int backlog)
    {
        try
        {
            _listenSocket.Listen(backlog);
            LogStartedListening();
            while (true)
            {
                Socket newConnectionSocket = _listenSocket.Accept();
                LogAcceptedNewConnection();
                lock (this)
                {
                    if (_isDisposed)
                    {
                        newConnectionSocket.Dispose();
                        LogDisposedNewConnectionAfterDisposal();
                        break;
                    }

                    Accept(new Messenger(new RpcSocket(newConnectionSocket)));
                }
            }
        }
        catch (Exception exception)
        {
            switch (exception)
            {
                case SocketException { SocketErrorCode: SocketError.Interrupted }:
                case ObjectDisposedException:
                    if (_isDisposed)
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
                    "Stopped listening due to an exception"
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
                    "Started listening"
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
                    "Stopped listening due to disposal"
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
                    "{ServerType} was created with ephemeral port",
                    _typeName
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
                    "{ServerType} was created with specified port",
                    _typeName
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
                    "Accepted new connection"
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
                    "Immediately disposed new connection because this server is disposed"
                );
            }
        }
    }
}