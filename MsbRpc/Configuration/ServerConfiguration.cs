using JetBrains.Annotations;
using MsbRpc.Configuration.Builders.Interfaces;

namespace MsbRpc.Configuration;

[PublicAPI]
public readonly struct ServerConfiguration
{
    public readonly MessengerListenerConfiguration MessengerListenerConfiguration;

    public readonly InboundEndPointConfiguration EndPointConfiguration;

    public readonly InboundEndpointRegistryConfiguration InboundEndPointRegistryConfiguration;

    public ServerConfiguration(IServerConfigurationBuilder builder)
    {
        EndPointConfiguration = new InboundEndPointConfiguration(builder.EndPointConfiguration);
        InboundEndPointRegistryConfiguration = new InboundEndpointRegistryConfiguration(builder.EndPointRegistryConfiguration);
        MessengerListenerConfiguration = new MessengerListenerConfiguration(builder.MessengerListenerConfiguration);
    }
}