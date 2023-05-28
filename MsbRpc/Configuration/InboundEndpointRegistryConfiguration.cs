using Microsoft.Extensions.Logging;
using MsbRpc.Configuration.Builders.Interfaces;

namespace MsbRpc.Configuration;

public readonly struct InboundEndpointRegistryConfiguration
{
    public readonly ILoggerFactory? LoggerFactory;

    public readonly string LoggingName;

    public readonly LogConfiguration LogRegisteredEndpoint;

    public readonly LogConfiguration LogEndpointThrewException;

    public readonly LogConfiguration LogDeregisteredEndpoint;

    public readonly LogConfiguration LogDeregisteredEndpointOnDisposal;

    public InboundEndpointRegistryConfiguration(IInboundEndPointRegistryConfigurationBuilder builder)
    {
        LoggerFactory = builder.LoggerFactory;
        LoggingName = builder.LoggingName;
        LogRegisteredEndpoint = builder.LogRegisteredEndpoint;
        LogEndpointThrewException = builder.LogEndpointThrewException;
        LogDeregisteredEndpoint = builder.LogDeregisteredEndpoint;
        LogDeregisteredEndpointOnDisposal = builder.LogDeregisteredEndpointOnDisposal;
    }
}