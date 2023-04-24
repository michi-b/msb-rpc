using Microsoft.Extensions.Logging;
using MsbRpc.Configuration;
using MsbRpc.EndPoints;
using MsbRpc.Messaging;

namespace MsbRpc.Servers.Generic;

public abstract class Server<TServer, TEndPoint> : Server
    where TServer : Server<TServer, TEndPoint>
    where TEndPoint : IInboundEndPoint
{
    private readonly string _endpointName;
    private readonly InboundEndPointRegistry _endPointRegistry;
    private readonly ILoggerFactory _loggerFactory;
    private readonly string _serverName;

    public InboundEndPointRegistryEntry[] EndPoints => _endPointRegistry.EndPoints;

    protected Server(ILoggerFactory loggerFactory) : base(CreateServerConfiguration(loggerFactory, typeof(TServer).Name))
    {
        _endPointRegistry = new InboundEndPointRegistry(CreateInboundEndpointRegistryConfiguration(loggerFactory));
        _loggerFactory = loggerFactory;
        _serverName = typeof(TServer).Name;
        _endpointName = typeof(TEndPoint).Name;
    }

    protected abstract IInboundEndPoint CreateEndPoint(Messenger messenger, InboundEndPointConfiguration endPointConfiguration);

    private static ServerConfiguration CreateServerConfiguration(ILoggerFactory loggerFactory, string serverName)
        => new()
        {
            LoggerFactory = loggerFactory,
            LoggingName = serverName,
            ThreadName = serverName
        };

    private static InboundEndPointConfiguration CreateInboundEndPointConfiguration(ILoggerFactory loggerFactory)
        => new()
        {
            LoggerFactory = loggerFactory,
            LoggingName = nameof(_endpointName)
        };

    private InboundEndpointRegistryConfiguration CreateInboundEndpointRegistryConfiguration(ILoggerFactory loggerFactory)
        => new()
        {
            LoggerFactory = loggerFactory,
            LoggingName = _serverName + "EndpointRegistry"
        };

    protected override void Accept(Messenger messenger)
    {
        _endPointRegistry.Start(CreateEndPoint(messenger, CreateInboundEndPointConfiguration(_loggerFactory)));
    }

    protected override void DisposeManagedResources()
    {
        base.DisposeManagedResources();
        _endPointRegistry.Dispose();
    }
}