using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using JetBrains.Annotations;
using Microsoft.Extensions.Logging;
using MsbRpc.Contracts;
using MsbRpc.EndPoints.Configuration;
using MsbRpc.Extensions;
using MsbRpc.Messaging;
using MsbRpc.Sockets;

namespace MsbRpc.EndPoints;

public abstract class Server<TServer, TEndPoint, TProcedure, TImplementation> : IDisposable
    where TServer : Server<TServer, TEndPoint, TProcedure, TImplementation>
    where TEndPoint : InboundEndPoint<TEndPoint, TProcedure, TImplementation>
    where TImplementation : IRpcContract
    where TProcedure : Enum
{
    private const int DefaultListenBacklogSize = 100;
    private readonly ServerConfiguration _configuration;
    private readonly ServerEndPointRegistry<TEndPoint, TProcedure, TImplementation> _connections;
    private readonly Socket _listenSocket;

    private readonly ILogger<TServer>? _logger;
    private readonly string _threadName;

    public readonly int Port;

    private bool _isDisposed;
    private Thread? _listenThread;

    protected Server(ServerConfiguration configuration)
    {
        _configuration = configuration;
        _logger = configuration.LoggerFactory?.CreateLogger<TServer>();

        IPAddress localHost = Dns.GetHostEntry("localhost").AddressList[0];
        _listenSocket = new Socket(localHost.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
        _listenSocket.Bind(new IPEndPoint(localHost, configuration.Port));

        var listenEndPoint = _listenSocket.LocalEndPoint as IPEndPoint;
        Port = configuration.Port == 0 ? listenEndPoint!.Port : configuration.Port;
        _threadName = $"{GetType().Name}:{Port}";
        if (Port == 0)
        {
            LogWasCreatedWithEphemeralPort();
        }
        else
        {
            LogWasCreatedWithSpecificPort();
        }

        _connections = new ServerEndPointRegistry<TEndPoint, TProcedure, TImplementation>(configuration);
    }

    [PublicAPI]
    public Thread Start()
    {
        lock (this)
        {
            if (_isDisposed)
            {
                throw new ObjectDisposedException(_threadName);
            }

            if (_listenThread != null)
            {
                throw new InvalidOperationException($"{_threadName} is already started.");
            }

            _listenThread = new Thread(() => Run(_configuration.ListenBacklogSize)) { Name = _threadName };
            _listenThread.Start();
            return _listenThread;
        }
    }

    public KeyValuePair<int, ServerEndPointRegistry<TEndPoint, TProcedure, TImplementation>.Entry>[] CreateConnectionDump()
    {
        lock (this)
        {
            if (_isDisposed)
            {
                throw new ObjectDisposedException(_threadName);
            }

            return _connections.CreateDump();
        }
    }

    public void Dispose()
    {
        if (!_isDisposed)
        {
            lock (this)
            {
                _isDisposed = true;
                _listenSocket.Dispose();
                _connections.Dispose();
            }

            _listenThread?.Join();
        }
    }

    private void Run(int backlog = DefaultListenBacklogSize)
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

                    TEndPoint endPoint = CreateEndPoint(new Messenger(new RpcSocket(newConnectionSocket)));
                    _connections.AddAndStart(endPoint);
                }
            }
        }
        catch (SocketException socketException)
        {
            if (_isDisposed && socketException.SocketErrorCode == SocketError.Interrupted)
            {
                LogStoppedListeningDueToDisposal();
            }
            else
            {
                LogStoppedListeningDueToException(socketException);
                throw;
            }
        }
        catch (Exception exception)
        {
            LogStoppedListeningDueToException(exception);
            throw;
        }
        finally
        {
            Dispose();
        }
    }

    protected abstract TEndPoint CreateEndPoint(Messenger messenger);

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

    private void LogStoppedListeningDueToDisposal()
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
                    nameof(TServer)
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
                    nameof(TServer)
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