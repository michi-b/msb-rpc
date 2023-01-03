using System;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using JetBrains.Annotations;
using Microsoft.Extensions.Logging;
using MsbRpc.Messaging;
using MsbRpc.Sockets;

namespace MsbRpc.EndPoints;

public abstract partial class Server<TEndPoint, TProcedure, TImplementation> : IDisposable
    where TEndPoint : InboundEndPoint<TProcedure, TImplementation>
    where TProcedure : Enum
{
    [PublicAPI] public readonly int Port;

    protected readonly Func<TImplementation> GetImplementation;
    protected readonly ILogger Logger;
    protected readonly ILoggerFactory LoggerFactory;
    protected readonly string TypeName;

    private const int DefaultListenBacklogSize = 100;
    private readonly RootEndPointRegistry<TEndPoint, TProcedure, TImplementation> _connections;
    private readonly Socket _listenSocket;
    private bool _isDisposed;
    private Thread? _listenThread;

    protected Server(Func<TImplementation> getImplementation, ILoggerFactory loggerFactory, ILogger logger, int port = 0)
    {
        TypeName = GetType().Name;
        _connections = new RootEndPointRegistry<TEndPoint, TProcedure, TImplementation>(TypeName, loggerFactory);
        GetImplementation = getImplementation;
        LoggerFactory = loggerFactory;
        Logger = logger;

        IPAddress localHost = Dns.GetHostEntry("localhost").AddressList[0];
        _listenSocket = new Socket(localHost.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
        _listenSocket.Bind(new IPEndPoint(localHost, port));
        if (port == 0)
        {
            var listenEndPoint = (IPEndPoint)_listenSocket.LocalEndPoint;
            Port = listenEndPoint.Port;
            LogWasCreatedWithEphemeralPort(Logger, TypeName, Port);
        }
        else
        {
            Port = port;
            LogWasCreatedWithSpecifiedPort(Logger, TypeName, Port);
        }
    }

    [PublicAPI]
    public Thread Start(int backlog = DefaultListenBacklogSize)
    {
        lock (this)
        {
            if (_isDisposed)
            {
                throw new ObjectDisposedException(TypeName);
            }

            if (_listenThread != null)
            {
                throw new InvalidOperationException($"{TypeName} is already started.");
            }

            _listenThread = new Thread(() => Run(backlog));
            _listenThread.Start();
            return _listenThread;
        }
    }

    private void Run(int backlog = DefaultListenBacklogSize)
    {
        try
        {
            _listenSocket.Listen(backlog);
            LogStartedListening(Logger, TypeName, Port);
            while (true)
            {
                Socket newConnectionSocket = _listenSocket.Accept();
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
                LogStoppedListeningDueToDisposal(Logger, TypeName, Port);
            }
            else
            {
                LogStoppedListeningDueToException(Logger, TypeName, Port, socketException);
                throw;
            }
        }
        catch (Exception exception)
        {
            LogStoppedListeningDueToException(Logger, TypeName, Port, exception);
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
        Message = "{ServerType} was created with ephemeral port {Port}."
    )]
    private static partial void LogWasCreatedWithEphemeralPort(ILogger logger, string serverType, int port);

    [LoggerMessage
    (
        EventId = (int)LogEventIds.ServerWasCreatedWithSpecifiedPort,
        Level = LogLevel.Debug,
        Message = "{ServerType} was created with specified port {Port}."
    )]
    private static partial void LogWasCreatedWithSpecifiedPort(ILogger logger, string serverType, int port);

    [LoggerMessage
    (
        EventId = (int)LogEventIds.ServerStartedListening,
        Level = LogLevel.Information,
        Message = "{ServerType} started listening on port {Port}."
    )]
    private static partial void LogStartedListening(ILogger logger, string serverType, int port);

    [LoggerMessage
    (
        EventId = (int)LogEventIds.ServerStoppedListeningDueToDisposal,
        Level = LogLevel.Information,
        Message = "{ServerType} stopped listening on port {Port} due to disposal."
    )]
    private static partial void LogStoppedListeningDueToDisposal(ILogger logger, string serverType, int port);

    [LoggerMessage
    (
        EventId = (int)LogEventIds.ServerStoppedListeningDueToException,
        Level = LogLevel.Error,
        Message = "{ServerType} stopped listening on port {Port} due to an exception."
    )]
    private static partial void LogStoppedListeningDueToException(ILogger logger, string serverType, int port, Exception exception);
}