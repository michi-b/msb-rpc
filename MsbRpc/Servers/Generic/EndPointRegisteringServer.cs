using MsbRpc.Configuration;
using MsbRpc.EndPoints;
using MsbRpc.Messaging;

namespace MsbRpc.Servers.Generic;

public abstract class EndPointRegisteringServer : Server
{
    private readonly InboundEndPointRegistry _endPointRegistry;

    public InboundEndPointRegistryEntry[] EndPoints => _endPointRegistry.EndPoints;

    protected EndPointRegisteringServer(ref ServerConfiguration configuration) : base(ref configuration)
    {
        InboundEndpointRegistryConfiguration endPointRegistryConfiguration = configuration.InboundEndPointRegistryConfiguration;
        _endPointRegistry = new InboundEndPointRegistry(ref endPointRegistryConfiguration);
    }

    protected abstract IInboundEndPoint CreateEndPoint(Messenger messenger);

    protected override void Accept(Messenger messenger)
    {
        _endPointRegistry.Start(CreateEndPoint(messenger));
    }

    protected override void DisposeManagedResources()
    {
        base.DisposeManagedResources();
        _endPointRegistry.Dispose();
    }
}