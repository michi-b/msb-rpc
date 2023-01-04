using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using JetBrains.Annotations;
using Microsoft.Extensions.Logging;
using MsbRpc.Messaging;
using MsbRpc.Sockets;

namespace MsbRpc.EndPoints;

public abstract partial class Server<TEndPoint, TProcedure, TImplementation> : IDisposable
    where TEndPoint : InboundEndPoint<TEndPoint, TProcedure, TImplementation>
    where TProcedure : Enum
{
    [PublicAPI] public readonly int Port;

    protected readonly Func<TImplementation> GetImplementation;
    protected readonly ILogger Logger;
    protected readonly ILoggerFactory LoggerFactory;
    protected readonly string ThreadName;

    private const int DefaultListenBacklogSize = 100;
    private readonly RootEndPointRegistry<TEndPoint, TProcedure, TImplementation> _connections;
    private readonly Socket _listenSocket;
    private bool _isDisposed;
    private Thread? _listenThread;

    protected Server(Func<TImplementation> getImplementation, ILoggerFactory loggerFactory, ILogger logger, int port = 0)
    {
        GetImplementation = getImplementation;
        LoggerFactory = loggerFactory;
        Logger = logger;

        IPAddress localHost = Dns.GetHostEntry("localhost").AddressList[0];
        _listenSocket = new Socket(localHost.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
        _listenSocket.Bind(new IPEndPoint(localHost, port));
        
        var listenEndPoint = _listenSocket.LocalEndPoint as IPEndPoint;
        Port = port == 0 ? listenEndPoint!.Port : port;
        ThreadName = $"{GetType().Name}:{Port}";
        if (port == 0)
        {
            LogWasCreatedWithEphemeralPort(Logger, ThreadName);
        }
        else
        {
            Port = port;
            LogWasCreatedWithSpecifiedPort(Logger, ThreadName);
        }
        _connections = new RootEndPointRegistry<TEndPoint, TProcedure, TImplementation>(loggerFactory);
    }

    [PublicAPI]
    public Thread Start(int backlog = DefaultListenBacklogSize)
    {
        lock (this)
        {
            if (_isDisposed)
            {
                throw new ObjectDisposedException(ThreadName);
            }

            if (_listenThread != null)
            {
                throw new InvalidOperationException($"{ThreadName} is already started.");
            }

            _listenThread = new Thread(() => Run(backlog))
            {
                Name = ThreadName,
            };
            _listenThread.Start();
            return _listenThread;
        }
    }

    public KeyValuePair<int, RootEndPointRegistry<TEndPoint, TProcedure, TImplementation>.Entry>[] CreateConnectionDump()
    {
        lock (this)
        {
            if (_isDisposed)
            {
                throw new ObjectDisposedException(ThreadName);
            }

            return _connections.CreateDump();
        }
    }

    private void Run(int backlog = DefaultListenBacklogSize)
    {
        try
        {
            _listenSocket.Listen(backlog);
            LogStartedListening(Logger);
            while (true)
            {
                Socket newConnectionSocket = _listenSocket.Accept();
                var newConnectionEndPoint = (IPEndPoint)newConnectionSocket.LocalEndPoint;
                LogAcceptedNewConnection(Logger, newConnectionEndPoint.Port);
                lock (this)
                {
                    if (_isDisposed)
                    {
                        newConnectionSocket.Dispose();
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
                LogStoppedListeningDueToDisposal(Logger);
            }
            else
            {
                LogStoppedListeningDueToException(Logger, socketException);
                throw;
            }
        }
        catch (Exception exception)
        {
            LogStoppedListeningDueToException(Logger, exception);
            throw;
        }
        finally
        {
            Dispose();
        }
    }

    protected abstract TEndPoint CreateEndPoint(Messenger messenger);

    public void Dispose()
    {
        if (!_isDisposed)
        {
            lock (this)
            {
                _isDisposed = true;
                _listenSocket.Dispose();
                _connections.Dispose();
                _listenThread?.Join();
            }
        }
    }

    [LoggerMessage
    (
        EventId = (int)LogEventIds.ServerWasCreatedWithEphemeralPort,
        Level = LogLevel.Debug,
        Message = "{ServerType} was created on ephemeral port."
    )]
    private static partial void LogWasCreatedWithEphemeralPort(ILogger logger, string serverType);

    [LoggerMessage
    (
        EventId = (int)LogEventIds.ServerWasCreatedWithSpecifiedPort,
        Level = LogLevel.Debug,
        Message = "{ServerType} was created on specified port."
    )]
    private static partial void LogWasCreatedWithSpecifiedPort(ILogger logger, string serverType);

    [LoggerMessage
    (
        EventId = (int)LogEventIds.ServerStartedListening,
        Level = LogLevel.Information,
        Message = "Started listening."
    )]
    private static partial void LogStartedListening(ILogger logger);

    [LoggerMessage
    (
        EventId = (int)LogEventIds.ServerStoppedListeningDueToDisposal,
        Level = LogLevel.Information,
        Message = "Stopped listening due to disposal."
    )]
    private static partial void LogStoppedListeningDueToDisposal(ILogger logger);

    [LoggerMessage
    (
        EventId = (int)LogEventIds.ServerStoppedListeningDueToException,
        Level = LogLevel.Error,
        Message = "Stopped listening due to an exception."
    )]
    private static partial void LogStoppedListeningDueToException(ILogger logger, Exception exception);

    [LoggerMessage
    (
        EventId = (int)LogEventIds.ServerAcceptedNewConnection,
        Level = LogLevel.Trace,
        Message = "Accepted new connection on port {Port}."
    )]
    private static partial void LogAcceptedNewConnection(ILogger logger, int port);
}