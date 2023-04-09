using MsbRpc.Configuration;
using MsbRpc.Exceptions;
using MsbRpc.Messaging;
using MsbRpc.Servers;
using MsbRpc.Test.Generator.Incrementer.ToGenerate;

namespace MsbRpc.Test.Generator.Incrementer;

public class IncrementerServer : Server
{
    private readonly InboundEndPointConfiguration _endPointConfiguration;
    private readonly InboundEndPointRegistry _endPointRegistry;
    private readonly RpcExceptionTransmissionOptions _exceptionTransmissionOptions;

    public InboundEndPointRegistryEntry[] EndPoints => _endPointRegistry.EndPoints;

    public IncrementerServer(ServerConfiguration configuration, RpcExceptionTransmissionOptions exceptionTransmissionOptions) : base(configuration)
    {
        _exceptionTransmissionOptions = exceptionTransmissionOptions;

        var endPointRegistryConfiguration = new InboundEndpointRegistryConfiguration { LoggerFactory = configuration.LoggerFactory };

        _endPointConfiguration = new InboundEndPointConfiguration();
        _endPointConfiguration.LoggerFactory = configuration.LoggerFactory;

        _endPointRegistry = new InboundEndPointRegistry(endPointRegistryConfiguration);
    }

    protected override void Accept(Messenger messenger)
    {
        _endPointRegistry.Run(new IncrementerServerEndPoint(messenger, new Incrementer(_exceptionTransmissionOptions), _endPointConfiguration));
    }

    protected override void Dispose(bool disposing)
    {
        if (disposing)
        {
            _endPointRegistry.Dispose();
        }

        base.Dispose(disposing);
    }
}