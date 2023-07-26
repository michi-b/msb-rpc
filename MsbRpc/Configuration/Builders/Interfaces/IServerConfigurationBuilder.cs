#region

using JetBrains.Annotations;

#endregion

namespace MsbRpc.Configuration.Builders.Interfaces;

[PublicAPI]
public interface IServerConfigurationBuilder : IConfigurationWithLoggerFactoryBuilder
{
    public IInboundEndPointConfigurationBuilder EndPointConfiguration { get; set; }
}