#region

using MsbRpc.Configuration;
using MsbRpc.EndPoints.Interfaces;
using MsbRpc.Messaging;

#endregion

namespace MsbRpc.Servers;

/// <summary>
///     extends server with registration of endpoints, and disposes them on disposal, also joining their threads
/// </summary>
public abstract class InboundEndPointServer : Server
{
    private readonly InboundEndPointRegistry _endPointRegistry;

    public InboundEndPointRegistryEntry[] EndPoints => _endPointRegistry.EndPoints;

    protected InboundEndPointServer(ref ServerConfiguration configuration) : base(ref configuration)
    {
        InboundEndpointRegistryConfiguration endPointRegistryConfiguration = configuration.InboundEndPointRegistryConfiguration;
        _endPointRegistry = new InboundEndPointRegistry(ref endPointRegistryConfiguration);
    }

    public override void AcceptUnIdentified(Messenger messenger)
    {
        _endPointRegistry.Run(CreateEndPoint(messenger));
    }

    protected abstract IInboundEndPoint CreateEndPoint(Messenger messenger);

    protected override void DisposeManagedResources()
    {
        base.DisposeManagedResources();
        _endPointRegistry.Dispose();
    }
}