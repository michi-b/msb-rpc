#region

using Microsoft.Extensions.Logging;
using MsbRpc.Configuration.Builders.Abstract;
using MsbRpc.Configuration.Builders.Interfaces;
using MsbRpc.Logging;
using MsbRpc.Servers;

#endregion

namespace MsbRpc.Configuration.Builders;

public class InboundEndPointRegistryConfigurationBuilder : ConfigurationWithLoggerFactoryBuilder<InboundEndpointRegistryConfiguration>,
    IInboundEndPointRegistryConfigurationBuilder
{
    public string LoggingName { get; set; } = nameof(InboundEndPointRegistry);
    public LogConfigurationBuilder LogRegisteredEndpoint { get; set; } = new(LogEventIds.ServerEndPointDeregistered, LogLevel.Debug);
    public LogConfigurationBuilder LogEndpointThrewException { get; set; } = new(LogEventIds.ServerEndPointThrewException, LogLevel.Error);
    public LogConfigurationBuilder LogDeregisteredEndpoint { get; set; } = new(LogEventIds.ServerEndPointDeregistered, LogLevel.Debug);
    public LogConfigurationBuilder LogDeregisteredEndpointOnDisposal { get; set; } = new(LogEventIds.ServerEndPointDeregisteredOnRegistryDisposal, LogLevel.Warning);

    public override InboundEndpointRegistryConfiguration Build() => new(this);
}