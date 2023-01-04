using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Threading;
using JetBrains.Annotations;
using Microsoft.Extensions.Logging;
using MsbRpc.Utility;

namespace MsbRpc.EndPoints;

public partial class RootEndPointRegistry<TEndPoint, TProcedure, TImplementation> : IDisposable
    where TEndPoint : InboundEndPoint<TEndPoint, TProcedure, TImplementation>
    where TProcedure : Enum
{
    private static readonly string EndPointTypename = typeof(TEndPoint).Name;
    private readonly Dictionary<int, Entry> _connections = new(); // key is managed thread id
    private readonly ILogger<RootEndPointRegistry<TEndPoint, TProcedure, TImplementation>> _logger;
    private int _connectionCount;
    private bool _isDisposed;

    public RootEndPointRegistry(ILoggerFactory loggerFactory)
        => _logger = loggerFactory.CreateLoggerOptional<RootEndPointRegistry<TEndPoint, TProcedure, TImplementation>>();

    public void AddAndStart(TEndPoint endPoint)
    {
        lock (this)
        {
            if (_isDisposed)
            {
                throw new ObjectDisposedException(EndPointTypename);
            }

            var thread = new Thread(() => RunEndPoint(endPoint)) { Name = EndPointTypename };

            _connections.Add(thread.ManagedThreadId, new Entry(endPoint, thread));

            LogEndPointRegistered(_logger, ++_connectionCount);
            thread.Start();
        }
    }

    [PublicAPI]
    public KeyValuePair<int, Entry>[] CreateDump()
    {
        lock (this)
        {
            if (_isDisposed)
            {
                throw new ObjectDisposedException(EndPointTypename);
            }

            return _connections.ToArray();
        }
    }

    public void Dispose()
    {
        if (!_isDisposed)
        {
            lock (this)
            {
                _isDisposed = true;

                foreach (KeyValuePair<int, Entry> entry in _connections)
                {
                    entry.Value.EndPoint.Dispose();
                    entry.Value.Thread.Join();
                    LogEndPointDeregisteredOnDisposal(_logger, EndPointTypename, entry.Key, --_connectionCount);
                }

                _connections.Clear();
            }
        }
    }

    public readonly struct Entry
    {
        public readonly Thread Thread;
        public readonly TEndPoint EndPoint;

        public Entry(TEndPoint endPoint, Thread thread)
        {
            EndPoint = endPoint;
            Thread = thread;
        }
    }

    private void RunEndPoint(TEndPoint endPoint)
    {
        try
        {
            endPoint.Listen();
        }
        catch (SocketException socketException)
        {
            if (!_isDisposed || socketException.SocketErrorCode != SocketError.Interrupted)
            {
                LogException(_logger, socketException);
            }
        }
        catch (Exception exception)
        {
            LogException(_logger, exception);
        }
        finally
        {
            if (!_isDisposed)
            {
                lock (this)
                {
                    if (!_isDisposed)
                    {
                        endPoint.Dispose();
                        _connections.Remove(Thread.CurrentThread.ManagedThreadId);
                        LogEndPointDeregistered(_logger, --_connectionCount);
                    }
                }
            }
        }
    }

    [LoggerMessage
    (
        EventId = (int)LogEventIds.RootEndPointThrewException,
        Level = LogLevel.Error,
        Message = "An exception occured."
    )]
    private static partial void LogException(ILogger logger, Exception exception);

    [LoggerMessage
    (
        EventId = (int)LogEventIds.RootEndPointRegistered,
        Level = LogLevel.Debug,
        Message = "Registered new endpoint. Connection count is {ConnectionCount}."
    )]
    private static partial void LogEndPointRegistered(ILogger logger, int connectionCount);

    [LoggerMessage
    (
        EventId = (int)LogEventIds.RootEndPointDeregistered,
        Level = LogLevel.Debug,
        Message = "Deregistered endpoint. Connection count is {ConnectionCount}."
    )]
    private static partial void LogEndPointDeregistered(ILogger logger, int connectionCount);

    [LoggerMessage
    (
        EventId = (int)LogEventIds.RootEndPointDeregisteredOnRegistryDisposal,
        Level = LogLevel.Debug,
        Message = "Deregistered {EndPointTypeName} on with thread id {ThreadId} due to disposal. Connection count is {ConnectionCount}."
    )]
    private static partial void LogEndPointDeregisteredOnDisposal(ILogger logger, string endPointTypeName, int threadId, int connectionCount);
}