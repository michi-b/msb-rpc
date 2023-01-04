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
    where TEndPoint : InboundEndPoint<TProcedure, TImplementation>
    where TProcedure : Enum
{
    private static readonly string EndPointTypename = typeof(TEndPoint).Name;
    private readonly Dictionary<int, Entry> _entries = new();
    private readonly ILogger<RootEndPointRegistry<TEndPoint, TProcedure, TImplementation>> _logger;
    private bool _isDisposed;

    public RootEndPointRegistry(ILoggerFactory loggerFactory)
        => _logger = loggerFactory.TryCreateLogger<RootEndPointRegistry<TEndPoint, TProcedure, TImplementation>>();

    public void AddAndStart(TEndPoint endPoint)
    {
        lock (this)
        {
            if (_isDisposed)
            {
                throw new ObjectDisposedException(EndPointTypename);
            }

            int port = endPoint.Port;

            if (_entries.ContainsKey(port))
            {
                throw new InvalidOperationException($"Port {port} is already registered");
            }

            var thread = new Thread(() => RunEndPoint(endPoint)) { Name = $"{EndPointTypename}:{port}" };
            _entries.Add(port, new Entry(endPoint, thread));
            LogEndPointRegistered(_logger, EndPointTypename, port);
            LogConnectionCount(_logger, _entries.Count);
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

            return _entries.ToArray();
        }
    }

    public void Dispose()
    {
        if (!_isDisposed)
        {
            lock (this)
            {
                _isDisposed = true;

                KeyValuePair<int, Entry>[] entries = _entries.ToArray();

                foreach (KeyValuePair<int, Entry> entry in entries)
                {
                    entry.Value.EndPoint.Dispose();
                }

                foreach (KeyValuePair<int, Entry> entry in entries)
                {
                    entry.Value.Thread.Join();
                    LogEndPointDeregisteredOnDisposal(_logger, EndPointTypename, entry.Key);
                }

                _entries.Clear();
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
        int port = endPoint.Port;
        try
        {
            endPoint.Listen();
        }
        catch (SocketException socketException)
        {
            if (!_isDisposed || socketException.SocketErrorCode != SocketError.Interrupted)
            {
                LogEndPointThrewException(_logger, port, socketException);
            }
        }
        catch (Exception exception)
        {
            LogEndPointThrewException(_logger, port, exception);
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
                        //can't join the thread because this is the current thread
                        //_entries[port].Thread.Join();
                        _entries.Remove(port);
                        LogEndPointDeregistered(_logger, EndPointTypename, port);
                        LogConnectionCount(_logger, _entries.Count);
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
    private static partial void LogEndPointThrewException(ILogger logger, int port, Exception exception);

    [LoggerMessage
    (
        EventId = (int)LogEventIds.RootEndPointDeregistered,
        Level = LogLevel.Debug,
        Message = "Deregistered {EndPointTypeName} on port {Port}."
    )]
    private static partial void LogEndPointDeregistered(ILogger logger, string endPointTypeName, int port);

    [LoggerMessage
    (
        EventId = (int)LogEventIds.RootEndPointDeregisteredOnRegistryDisposal,
        Level = LogLevel.Debug,
        Message = "Deregistered {EndPointTypeName} on port {Port} due to disposal."
    )]
    private static partial void LogEndPointDeregisteredOnDisposal(ILogger logger, string endPointTypeName, int port);

    [LoggerMessage
    (
        EventId = (int)LogEventIds.RootEndPointRegistered,
        Level = LogLevel.Debug,
        Message = "Registered new {EndPointTypeName} on port {Port}."
    )]
    private static partial void LogEndPointRegistered(ILogger logger, string endPointTypeName, int port);
}