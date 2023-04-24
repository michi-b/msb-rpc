using Microsoft.Extensions.Logging;
using MsbRpc.Configuration;
using MsbRpc.Exceptions;
using MsbRpc.Messaging;
using MsbRpc.Servers;
using MsbRpc.Test.Implementations.Incrementer.ToGenerate;

namespace MsbRpc.Test.Implementations.Incrementer;

public class IncrementerServer : Server
{
    private readonly InboundEndPointRegistry _endPointRegistry;

    private readonly RpcExceptionTransmissionOptions _exceptionTransmissionOptions;

    private readonly ILoggerFactory _loggerFactory;

    public InboundEndPointRegistryEntry[] EndPoints => _endPointRegistry.EndPoints;

    public IncrementerServer
    (
        ILoggerFactory loggerFactory,
        RpcExceptionTransmissionOptions exceptionTransmissionOptions = RpcExceptionTransmissionOptions.None
    ) : base(CreateServerConfiguration(loggerFactory))
    {
        _endPointRegistry = new InboundEndPointRegistry(CreateInboundEndpointRegistryConfiguration(loggerFactory));
        _loggerFactory = loggerFactory;
        _exceptionTransmissionOptions = exceptionTransmissionOptions;
    }

    private static ServerConfiguration CreateServerConfiguration(ILoggerFactory loggerFactory)
        => new()
        {
            LoggerFactory = loggerFactory,
            LoggingName = nameof(IncrementerServer),
            ThreadName = nameof(IncrementerServer)
        };

    private static InboundEndPointConfiguration CreateInboundEndPointConfiguration(ILoggerFactory loggerFactory)
        => new()
        {
            LoggerFactory = loggerFactory,
            LoggingName = nameof(IncrementerServerEndPoint)
        };

    private static InboundEndpointRegistryConfiguration CreateInboundEndpointRegistryConfiguration(ILoggerFactory loggerFactory)
        => new()
        {
            LoggerFactory = loggerFactory,
            LoggingName = nameof(IncrementerServer) + "EndpointRegistry"
        };

    protected override void Accept(Messenger messenger)
    {
        _endPointRegistry.Start
            (new IncrementerServerEndPoint(messenger, new Incrementer(_exceptionTransmissionOptions), CreateInboundEndPointConfiguration(_loggerFactory)));
    }

    protected override void DisposeManagedResources()
    {
        base.DisposeManagedResources();
        _endPointRegistry.Dispose();
    }
}