using System;
using System.Net;
using System.Net.Sockets;
using JetBrains.Annotations;
using Microsoft.Extensions.Logging;
using MsbRpc.Configuration;
using MsbRpc.Disposable;
using MsbRpc.Extensions;
using MsbRpc.Messaging;
using MsbRpc.Servers.Listener;

namespace MsbRpc.Servers;

public abstract class Server : ConcurrentDisposable, IUnIdentifiedConnectionReceiver
{
    private readonly Socket _listenSocket;

    private readonly ILogger<Server>? _logger;

    protected readonly ServerConfiguration Configuration;

    public readonly int Port;

    private ConnectionListener? _connectionListener;

    protected Server(ref ServerConfiguration configuration)
    {
        Configuration = configuration;
        _logger = configuration.LoggerFactory?.CreateLogger<Server>();

        IPAddress localHost = Dns.GetHostEntry("localhost").AddressList[0];

        _listenSocket = new Socket(localHost.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
        _listenSocket.Bind(new IPEndPoint(localHost, configuration.Port));

        var listenEndPoint = _listenSocket.LocalEndPoint as IPEndPoint;
        Port = configuration.Port == 0 ? listenEndPoint!.Port : configuration.Port;
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
    public ConnectionListener Start()
    {
        if (_connectionListener != null)
        {
            throw new InvalidOperationException($"{_connectionListener.Thread.Name} is already started.");
        }

        string threadName = $"{Configuration.ThreadName}:{Port}";
        _connectionListener = ConnectionListener.Run(_listenSocket, Configuration, threadName, this, _logger);

        return _connectionListener;
    }

    public abstract void AcceptUnIdentified(Messenger messenger);

    protected override void DisposeManagedResources()
    {
        _listenSocket.Dispose();

        if (_connectionListener != null)
        {
            _connectionListener.Dispose();
            _connectionListener = null;
        }

        base.DisposeManagedResources();
    }

    private void LogWasCreatedWithEphemeralPort()
    {
        if (_logger != null)
        {
            LogConfiguration configuration = Configuration.LogWasCreatedWithEphemeralPort;
            if (_logger.GetIsEnabled(configuration))
            {
                _logger.Log
                (
                    configuration.Level,
                    configuration.Id,
                    "{LoggingName} was created with ephemeral port",
                    Configuration.LoggingName
                );
            }
        }
    }

    private void LogWasCreatedWithSpecificPort()
    {
        if (_logger != null)
        {
            LogConfiguration configuration = Configuration.LogWasCreatedWithSpecifiedPort;
            if (_logger.GetIsEnabled(configuration))
            {
                _logger.Log
                (
                    configuration.Level,
                    configuration.Id,
                    "{LoggingName} was created with specified port",
                    Configuration.LoggingName
                );
            }
        }
    }
}