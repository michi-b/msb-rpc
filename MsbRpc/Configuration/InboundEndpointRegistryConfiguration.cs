using MsbRpc.Configuration.Builders.Interfaces;
using MsbRpc.Configuration.Interfaces;

namespace MsbRpc.Configuration;

public class InboundEndpointRegistryConfiguration : ConfigurationWithLoggerFactory, IInboundEndpointRegistryConfiguration
{
    public string LoggingName { get; }

    public LogConfiguration LogRegisteredEndpoint { get; }

    public LogConfiguration LogEndpointThrewException { get; }

    public LogConfiguration LogDeregisteredEndpoint { get; }

    public LogConfiguration LogDeregisteredEndpointOnDisposal { get; }

    public InboundEndpointRegistryConfiguration(IInboundEndPointRegistryConfigurationBuilder builder) : base(builder)
    {
        LoggingName = builder.LoggingName;
        LogRegisteredEndpoint = builder.LogRegisteredEndpoint;
        LogEndpointThrewException = builder.LogEndpointThrewException;
        LogDeregisteredEndpoint = builder.LogDeregisteredEndpoint;
        LogDeregisteredEndpointOnDisposal = builder.LogDeregisteredEndpointOnDisposal;
    }
}