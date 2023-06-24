using MsbRpc.Configuration;
using MsbRpc.EndPoints.Interfaces;
using MsbRpc.Messaging;

namespace MsbRpc.Servers.Generic;

/// <summary>
///     extends server with registration of endpoints, and disposes them on disposal, also joining their threads
/// </summary>
public abstract class RegistryServer : Server
{
    private readonly InboundEndPointRegistry _endPointRegistry;

    private uint _lastGivenEndPointId = int.MaxValue;

    public InboundEndPointRegistryEntry[] EndPoints => _endPointRegistry.EndPoints;

    protected RegistryServer(ref ServerConfiguration configuration) : base(ref configuration)
    {
        InboundEndpointRegistryConfiguration endPointRegistryConfiguration = configuration.InboundEndPointRegistryConfiguration;
        _endPointRegistry = new InboundEndPointRegistry(ref endPointRegistryConfiguration);
    }

    public override void AcceptUnIdentified(Messenger messenger)
    {
        //first "invalid" integral value after int.MaxValue
        const uint int32EndValue = (uint)int.MaxValue + 1;

        _lastGivenEndPointId++;
        _lastGivenEndPointId %= int32EndValue;

        _endPointRegistry.Start(CreateEndPoint(messenger, (int)_lastGivenEndPointId));
    }

    protected abstract IInboundEndPoint CreateEndPoint(Messenger messenger, int id);

    protected override void DisposeManagedResources()
    {
        base.DisposeManagedResources();
        _endPointRegistry.Dispose();
    }
}