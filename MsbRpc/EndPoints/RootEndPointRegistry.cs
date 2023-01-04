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
    private readonly Dictionary<int, Entry> _entriesByThreadId = new(); // key is managed thread id
    private readonly ILogger<RootEndPointRegistry<TEndPoint, TProcedure, TImplementation>> _logger;
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

            int port = endPoint.Port;

            if (_entriesByThreadId.ContainsKey(port))
            {
                throw new InvalidOperationException($"Port {port} is already registered");
            }

            var thread = new Thread(() => RunEndPoint(endPoint)) { Name = EndPointTypename };
            _entriesByThreadId.Add(thread.ManagedThreadId, new Entry(endPoint, thread));
            LogEndPointRegistered(_logger, EndPointTypename, thread.ManagedThreadId);
            LogConnectionCount(_logger, _entriesByThreadId.Count);
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

            return _entriesByThreadId.ToArray();
        }
    }

    public void Dispose()
    {
        if (!_isDisposed)
        {
            lock (this)
            {
                _isDisposed = true;

                KeyValuePair<int, Entry>[] entries = _entriesByThreadId.ToArray();

                foreach (KeyValuePair<int, Entry> entry in entries)
                {
                    entry.Value.EndPoint.Dispose();
                }

                foreach (KeyValuePair<int, Entry> entry in entries)
                {
                    entry.Value.Thread.Join();
                    LogEndPointDeregisteredOnDisposal(_logger, EndPointTypename, entry.Key);
                }

                _entriesByThreadId.Clear();
                LogConnectionCount(_logger, 0);
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
                LogEndPointThrewException(_logger, socketException);
            }
        }
        catch (Exception exception)
        {
            LogEndPointThrewException(_logger, exception);
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
                        _entriesByThreadId.Remove(Thread.CurrentThread.ManagedThreadId);
                        LogEndPointDeregistered(_logger, EndPointTypename);
                        LogConnectionCount(_logger, _entriesByThreadId.Count);
                    }
                }
            }
        }
    }

    [LoggerMessage
    (
        EventId = (int)LogEventIds.ServerConnectionCountChanged,
        Level = LogLevel.Information,
        Message = "Connection count changed to {ConnectionCount}"
    )]
    private static partial void LogConnectionCount(ILogger logger, int connectionCount);

    [LoggerMessage
    (
        EventId = (int)LogEventIds.RootEndPointThrewException,
        Level = LogLevel.Error,
        Message = "An exception occured."
    )]
    private static partial void LogEndPointThrewException(ILogger logger, Exception exception);

    [LoggerMessage
    (
        EventId = (int)LogEventIds.RootEndPointRegistered,
        Level = LogLevel.Debug,
        Message = "Registered new {EndPointTypeName} with thread id {ThreadId}."
    )]
    private static partial void LogEndPointRegistered(ILogger logger, string endPointTypeName, int threadId);

    [LoggerMessage
    (
        EventId = (int)LogEventIds.RootEndPointDeregistered,
        Level = LogLevel.Debug,
        Message = "Deregistered {EndPointTypeName}."
    )]
    private static partial void LogEndPointDeregistered(ILogger logger, string endPointTypeName);

    [LoggerMessage
    (
        EventId = (int)LogEventIds.RootEndPointDeregisteredOnRegistryDisposal,
        Level = LogLevel.Debug,
        Message = "Deregistered {EndPointTypeName} on with thread id {ThreadId} due to disposal."
    )]
    private static partial void LogEndPointDeregisteredOnDisposal(ILogger logger, string endPointTypeName, int threadId);
}