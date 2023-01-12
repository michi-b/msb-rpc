using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Threading;
using JetBrains.Annotations;
using Microsoft.Extensions.Logging;
using MsbRpc.Contracts;
using MsbRpc.EndPoints.Configuration;
using MsbRpc.Extensions;

namespace MsbRpc.EndPoints;

public class ServerEndPointRegistry<TEndPoint, TProcedure, TImplementation> : IDisposable
    where TEndPoint : InboundEndPoint<TEndPoint, TProcedure, TImplementation>
    where TImplementation : IRpcContract
    where TProcedure : Enum
{
    private static readonly string EndPointTypename = typeof(TEndPoint).Name;
    private readonly ServerConfiguration _configuration;
    private readonly Dictionary<int, Entry> _connections = new(); // key is managed thread id
    private readonly ILogger<ServerEndPointRegistry<TEndPoint, TProcedure, TImplementation>>? _logger;
    private int _connectionCount;
    private bool _isDisposed;

    public ServerEndPointRegistry(ServerConfiguration configuration)
    {
        _configuration = configuration;
        _logger = _configuration.LoggerFactory?.CreateLogger<ServerEndPointRegistry<TEndPoint, TProcedure, TImplementation>>();
    }

    public void AddAndStart(TEndPoint endPoint)
    {
        lock (this)
        {
            if (_isDisposed)
            {
                throw new ObjectDisposedException(EndPointTypename);
            }

            var thread = new Thread(() => RunEndPoint(endPoint)) { Name = EndPointTypename };
            int threadId = thread.ManagedThreadId;
            _connections.Add(threadId, new Entry(endPoint, thread));

            LogRegisteredEndpoint(threadId, ++_connectionCount);
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

                foreach (KeyValuePair<int, Entry> connection in _connections)
                {
                    Entry connectionValue = connection.Value;
                    connectionValue.EndPoint.Dispose();
                    connectionValue.Thread.Join();
                    LogDeregisteredEndPointOnDisposal(connection.Key, --_connectionCount);
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
                LogEndpointThrewException(socketException);
            }
        }
        catch (Exception exception)
        {
            LogEndpointThrewException(exception);
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
                        LogDeregisteredEndPoint(--_connectionCount);
                    }
                }
            }
        }
    }

    private void LogEndpointThrewException(Exception exception)
    {
        if (_logger != null)
        {
            LogConfiguration configuration = _configuration.LogEndpointThrewException;
            if (_logger.GetIsEnabled(configuration))
            {
                _logger.Log
                (
                    configuration.Level,
                    configuration.Id,
                    exception,
                    "An exception occured"
                );
            }
        }
    }

    private void LogRegisteredEndpoint(int threadId, int connectionCount)
    {
        if (_logger != null)
        {
            LogConfiguration configuration = _configuration.LogRegisteredEndpoint;
            if (_logger.GetIsEnabled(configuration))
            {
                _logger.Log
                (
                    configuration.Level,
                    configuration.Id,
                    "Registered new endpoint ({EndPointTypeName}) with thread id {TargetThreadId}. Connection count is {ConnectionCount}",
                    nameof(TEndPoint),
                    threadId,
                    connectionCount
                );
            }
        }
    }

    private void LogDeregisteredEndPoint(int connectionCount)
    {
        if (_logger != null)
        {
            LogConfiguration configuration = _configuration.LogDeregisteredEndpoint;
            if (_logger.GetIsEnabled(configuration))
            {
                _logger.Log
                (
                    configuration.Level,
                    configuration.Id,
                    "Deregistered endpoint ({EndpointTypeName}). Connection count is {ConnectionCount}",
                    nameof(TEndPoint),
                    connectionCount
                );
            }
        }
    }

    private void LogDeregisteredEndPointOnDisposal(int threadId, int connectionCount)
    {
        if (_logger != null)
        {
            LogConfiguration configuration = _configuration.LogDeregisteredEndpointOnDisposal;
            if (_logger.GetIsEnabled(configuration))
            {
                _logger.Log
                (
                    configuration.Level,
                    configuration.Id,
                    "Deregistered endpoint ({EndpointTypeName}) with thread id {TargetThreadId} on disposal. Connection count is {ConnectionCount}",
                    nameof(TEndPoint),
                    threadId,
                    connectionCount
                );
            }
        }
    }
}