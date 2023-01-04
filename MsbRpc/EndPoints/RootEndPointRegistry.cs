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
    private readonly string _serverName;
    private bool _isDisposed;

    public RootEndPointRegistry(string serverName, ILoggerFactory loggerFactory)
    {
        _serverName = serverName;
        _logger = loggerFactory.TryCreateLogger<RootEndPointRegistry<TEndPoint, TProcedure, TImplementation>>();
    }

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

            var thread = new Thread(() => RunEndPoint(endPoint));
            _entries.Add(port, new Entry(endPoint, thread));
            LogEndPointRegistered(_logger, _serverName, EndPointTypename, port);
            LogConnectionCount(_logger, _serverName, _entries.Count);
            thread.Start();
        }
    }

    [PublicAPI]
    public Entry[] CreateDump()
    {
        lock (this)
        {
            if (_isDisposed)
            {
                throw new ObjectDisposedException(EndPointTypename);
            }

            return _entries.Values.ToArray();
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
                    LogEndPointDeregisteredOnDisposal(_logger, _serverName, EndPointTypename, entry.Key);
                }

                _entries.Clear();
                LogConnectionCount(_logger, _serverName, 0);
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
                LogEndPointThrewException(_logger, EndPointTypename, port, socketException);
            }
        }
        catch (Exception exception)
        {
            LogEndPointThrewException(_logger, EndPointTypename, port, exception);
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
                        _entries[port].Thread.Join();
                        _entries.Remove(port);
                        LogEndPointDeregistered(_logger, _serverName, EndPointTypename, port);
                        LogConnectionCount(_logger, _serverName, _entries.Count);
                    }
                }
            }
        }
    }

    [LoggerMessage
    (
        EventId = (int)LogEventIds.RootEndPointRegistryConnectionCountChanged,
        Level = LogLevel.Information,
        Message = "{ServerName} with port {Port} now has {ConnectionCount} connection."
    )]
    private static partial void LogConnectionCount(ILogger logger, string serverName, int connectionCount);

    [LoggerMessage
    (
        EventId = (int)LogEventIds.RootEndPointThrewException,
        Level = LogLevel.Error,
        Message = "{EndPointTypeName} with port {Port} threw an exception."
    )]
    private static partial void LogEndPointThrewException(ILogger logger, string endPointTypeName, int port, Exception exception);

    [LoggerMessage
    (
        EventId = (int)LogEventIds.RootEndPointDeregistered,
        Level = LogLevel.Debug,
        Message = "{ServerName} deregistered {EndPointTypeName} with port {Port}."
    )]
    private static partial void LogEndPointDeregistered(ILogger logger, string serverName, string endPointTypeName, int port);

    [LoggerMessage
    (
        EventId = (int)LogEventIds.RootEndPointDeregisteredOnRegistryDisposal,
        Level = LogLevel.Debug,
        Message = "{ServerName} deregistered {EndPointTypeName} with port {Port} on endpoint registry disposal."
    )]
    private static partial void LogEndPointDeregisteredOnDisposal(ILogger logger, string serverName, string endPointTypeName, int port);

    [LoggerMessage
    (
        EventId = (int)LogEventIds.RootEndPointRegistered,
        Level = LogLevel.Debug,
        Message = "{ServerName} registered {EndPointTypeName} with port {Port}."
    )]
    private static partial void LogEndPointRegistered(ILogger logger, string serverName, string endPointTypeName, int port);
}