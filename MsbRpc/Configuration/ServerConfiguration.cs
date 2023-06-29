using JetBrains.Annotations;
using MsbRpc.Configuration.Builders.Interfaces;

namespace MsbRpc.Configuration;

[PublicAPI]
public readonly struct ServerConfiguration
{
    public readonly ConnectionListenerConfiguration ConnectionListenerConfiguration;

    public readonly InboundEndPointConfiguration InboundEndPointConfiguration;

    public readonly InboundEndpointRegistryConfiguration InboundEndPointRegistryConfiguration;

    public ServerConfiguration(IServerConfigurationBuilder builder)
    {
        InboundEndPointConfiguration = new InboundEndPointConfiguration(builder.EndPointConfiguration);
        InboundEndPointRegistryConfiguration = new InboundEndpointRegistryConfiguration(builder.EndPointRegistryConfiguration);
        ConnectionListenerConfiguration = new ConnectionListenerConfiguration(builder.ConnectionListenerConfiguration);
    }
}