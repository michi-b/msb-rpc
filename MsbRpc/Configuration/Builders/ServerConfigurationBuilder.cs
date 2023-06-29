﻿using JetBrains.Annotations;
using MsbRpc.Configuration.Builders.Abstract;
using MsbRpc.Configuration.Builders.Interfaces;

namespace MsbRpc.Configuration.Builders;

[PublicAPI]
public class ServerConfigurationBuilder : ConfigurationWithLoggerFactoryBuilder<ServerConfiguration>, IServerConfigurationBuilder
{
    public ConnectionListenerConfigurationBuilder ConnectionListenerConfiguration { get; set; } = new();
    public IInboundEndPointRegistryConfigurationBuilder EndPointRegistryConfiguration { get; set; } = new InboundEndPointRegistryConfigurationBuilder();
    public IInboundEndPointConfigurationBuilder EndPointConfiguration { get; set; } = new InboundEndPointConfigurationBuilder();
    public override ServerConfiguration Build() => new(this);
}