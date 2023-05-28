using MsbRpc.Configuration.Interfaces.Generic;
using MsbRpc.Contracts;
using MsbRpc.EndPoints;
using MsbRpc.Messaging;

namespace MsbRpc.Servers.Generic;

public abstract class Server<TContract> : Server where TContract : IRpcContract
{
    private readonly IServerConfiguration<TContract> _configuration;
    private readonly InboundEndPointRegistry _endPointRegistry;

    public InboundEndPointRegistryEntry[] EndPoints => _endPointRegistry.EndPoints;

    protected Server(IServerConfiguration<TContract> configuration) : base(configuration)
    {
        _configuration = configuration;

        //todo: fix this
        _endPointRegistry = null!;
        //new InboundEndPointRegistry(CreateInboundEndpointRegistryConfiguration(loggerFactory));
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