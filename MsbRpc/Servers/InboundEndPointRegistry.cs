using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Threading;
using JetBrains.Annotations;
using Microsoft.Extensions.Logging;
using MsbRpc.Configuration;
using MsbRpc.Configuration.Interfaces;
using MsbRpc.Disposable;
using MsbRpc.EndPoints;
using MsbRpc.Extensions;

namespace MsbRpc.Servers;

[PublicAPI]
public class InboundEndPointRegistry : ConcurrentDisposable
{
    private readonly IInboundEndpointRegistryConfiguration _configuration;
    private readonly Dictionary<int, InboundEndPointRegistryEntry> _endPoints = new(); // key is managed thread id
    private readonly ILogger<InboundEndPointRegistry>? _logger;
    private int _connectionCount;

    [PublicAPI]
    public InboundEndPointRegistryEntry[] EndPoints
    {
        get { return ExecuteIfNotDisposed(() => _endPoints.Values.ToArray()); }
    }

    protected virtual string Name => nameof(InboundEndPointRegistry);

    [PublicAPI]
    public InboundEndPointRegistry(IInboundEndpointRegistryConfiguration configuration)
    {
        _configuration = configuration;
        _logger = _configuration.LoggerFactory?.CreateLogger<InboundEndPointRegistry>();
    }

    [PublicAPI]
    public void Start(IInboundEndPoint endPoint)
    {
        void RunUnsafe()
        {
            var thread = new Thread(() => RunEndPoint(endPoint)) { Name = endPoint.GetType().Name };
            int threadId = thread.ManagedThreadId;
            _endPoints.Add(threadId, new InboundEndPointRegistryEntry(endPoint, thread));

            LogRegisteredEndpoint(threadId, ++_connectionCount);
            thread.Start();
        }

        ExecuteIfNotDisposed(RunUnsafe);
    }

    protected override void DisposeManagedResources()
    {
        foreach (KeyValuePair<int, InboundEndPointRegistryEntry> connection in _endPoints)
        {
            InboundEndPointRegistryEntry entry = connection.Value;
            entry.EndPoint.Dispose();
            entry.Thread.Join();
            LogDeregisteredEndPointOnDisposal(connection.Key, --_connectionCount);
        }

        _endPoints.Clear();

        base.DisposeManagedResources();
    }

    private void RunEndPoint(IInboundEndPoint endPoint)
    {
        try
        {
            endPoint.Listen();
        }
        catch (SocketException socketException)
        {
            if (!IsDisposed || socketException.SocketErrorCode != SocketError.Interrupted)
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
            void DisposeEndPoint()
            {
                endPoint.Dispose();
                _endPoints.Remove(Thread.CurrentThread.ManagedThreadId);
                LogDeregisteredEndPoint(--_connectionCount);
            }

            ExecuteIfNotDisposed(DisposeEndPoint, false);
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
                    "{LoggingName} caught an endpoint exception",
                    _configuration.LoggingName
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
                    "{LoggingName} registered new endpoint with thread id {TargetThreadId}. Connection count is {ConnectionCount}",
                    _configuration.LoggingName,
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
                    "{LoggingName} deregistered endpoint. Connection count is {ConnectionCount}",
                    _configuration.LoggingName,
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
                    "{LoggingName} deregistered endpoint with thread id {TargetThreadId} on disposal. Connection count is {ConnectionCount}",
                    _configuration.LoggingName,
                    threadId,
                    connectionCount
                );
            }
        }
    }
}