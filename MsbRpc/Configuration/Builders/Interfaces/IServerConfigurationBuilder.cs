using JetBrains.Annotations;

namespace MsbRpc.Configuration.Builders.Interfaces;

[PublicAPI]
public interface IServerConfigurationBuilder : IConfigurationWithLoggerFactoryBuilder
{
    IInboundEndPointRegistryConfigurationBuilder EndPointRegistryConfiguration { get; set; }
    IInboundEndPointConfigurationBuilder EndPointConfiguration { get; set; }
    ConnectionListenerConfigurationBuilder ConnectionListenerConfiguration { get; set; }
}