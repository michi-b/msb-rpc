#region

using JetBrains.Annotations;

#endregion

namespace MsbRpc.Configuration.Builders.Interfaces;

[PublicAPI]
public interface IServerConfigurationBuilder : IConfigurationWithLoggerFactoryBuilder
{
    IInboundEndPointRegistryConfigurationBuilder EndPointRegistryConfiguration { get; set; }
    IInboundEndPointConfigurationBuilder EndPointConfiguration { get; set; }
    MessengerListenerConfigurationBuilder MessengerListenerConfiguration { get; set; }
}