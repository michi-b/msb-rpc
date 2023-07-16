#region

using MsbRpc.Configuration;
using MsbRpc.EndPoints.Interfaces;
using MsbRpc.Messaging;

#endregion

namespace MsbRpc.Servers.Generic;

/// <summary>
///     extends server with registration of endpoints, and disposes them on disposal, also joining their threads
/// </summary>
public abstract class RegistryServer : Server
{
    private readonly InboundEndPointRegistry _endPointRegistry;

    public InboundEndPointRegistryEntry[] EndPoints => _endPointRegistry.EndPoints;

    protected RegistryServer(ref ServerConfiguration configuration) : base(ref configuration)
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