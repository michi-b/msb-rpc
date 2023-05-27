using JetBrains.Annotations;
using Microsoft.Extensions.Logging;
using MsbRpc.Configuration.Interfaces;
using MsbRpc.Logging;
using MsbRpc.Servers;

namespace MsbRpc.Configuration.Builders;

[PublicAPI]
public class InboundEndPointRegistryConfigurationBuilder
    : ConfigurationWithLoggerFactoryBuilder,
        IConfigurationBuilder<InboundEndpointRegistryConfiguration>
{
    public string LoggingName { get; set; } = nameof(InboundEndPointRegistry);
    public LogConfiguration LogRegisteredEndpoint { get; set; } = new(LogEventIds.ServerEndPointDeregistered, LogLevel.Debug);
    public LogConfiguration LogEndpointThrewException { get; set; } = new(LogEventIds.ServerEndPointThrewException, LogLevel.Error);
    public LogConfiguration LogDeregisteredEndpoint { get; set; } = new(LogEventIds.ServerEndPointDeregistered, LogLevel.Debug);
    public LogConfiguration LogDeregisteredEndpointOnDisposal { get; set; } = new(LogEventIds.ServerEndPointDeregisteredOnRegistryDisposal, LogLevel.Warning);

    public new InboundEndpointRegistryConfiguration Build()
        => new
        (
            LoggerFactory,
            LoggingName,
            LogRegisteredEndpoint,
            LogEndpointThrewException,
            LogDeregisteredEndpoint,
            LogDeregisteredEndpointOnDisposal
        );
}