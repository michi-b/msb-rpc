using MsbRpc.Configuration;

namespace MsbRpc.Test.Implementations.Incrementer;

public struct IncrementerServerConfiguration
{
    public readonly ServerConfiguration ServerConfiguration { get; init; }
    public readonly InboundEndpointRegistryConfiguration EndPointRegistryConfiguration { get; init; }
    public readonly InboundEndPointConfiguration EndPointConfiguration { get; init; }

    public IncrementerServerConfiguration
        (ServerConfiguration serverConfiguration, InboundEndpointRegistryConfiguration endPointRegistryConfiguration, InboundEndPointConfiguration endPointConfiguration)
    {
        ServerConfiguration = serverConfiguration;
        EndPointConfiguration = endPointConfiguration;
        EndPointRegistryConfiguration = endPointRegistryConfiguration;
    }
}