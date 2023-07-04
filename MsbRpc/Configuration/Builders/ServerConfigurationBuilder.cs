using JetBrains.Annotations;
using MsbRpc.Configuration.Builders.Abstract;
using MsbRpc.Configuration.Builders.Interfaces;

namespace MsbRpc.Configuration.Builders;

[PublicAPI]
public class ServerConfigurationBuilder : ConfigurationWithLoggerFactoryBuilder<ServerConfiguration>, IServerConfigurationBuilder
{
    public MessengerListenerConfigurationBuilder MessengerListenerConfiguration { get; set; } = new();
    public IInboundEndPointRegistryConfigurationBuilder EndPointRegistryConfiguration { get; set; } = new InboundEndPointRegistryConfigurationBuilder();
    public IInboundEndPointConfigurationBuilder EndPointConfiguration { get; set; } = new InboundEndPointConfigurationBuilder();

    // sets both the name and the logging name of the connection listener with added "ConnectionListener" suffix
    public string Name
    {
        set => MessengerListenerConfiguration.Name = value + "ConnectionListener";
    }

    public override ServerConfiguration Build() => new(this);
}