using Microsoft.Extensions.Logging;
using MsbRpc.Configuration.Interfaces;

namespace MsbRpc.Configuration;

public class InboundEndpointRegistryConfiguration : ConfigurationWithLoggerFactory, IInboundEndpointRegistryConfiguration
{
    public string LoggingName { get; }

    public LogConfiguration LogRegisteredEndpoint { get; }

    public LogConfiguration LogEndpointThrewException { get; }

    public LogConfiguration LogDeregisteredEndpoint { get; }

    public LogConfiguration LogDeregisteredEndpointOnDisposal { get; }

    public InboundEndpointRegistryConfiguration
    (
        ILoggerFactory? loggerFactory,
        string loggingName,
        LogConfiguration logRegisteredEndpoint,
        LogConfiguration logEndpointThrewException,
        LogConfiguration logDeregisteredEndpoint,
        LogConfiguration logDeregisteredEndpointOnDisposal
    ) : base(loggerFactory)
    {
        LoggingName = loggingName;
        LogRegisteredEndpoint = logRegisteredEndpoint;
        LogEndpointThrewException = logEndpointThrewException;
        LogDeregisteredEndpoint = logDeregisteredEndpoint;
        LogDeregisteredEndpointOnDisposal = logDeregisteredEndpointOnDisposal;
    }
}