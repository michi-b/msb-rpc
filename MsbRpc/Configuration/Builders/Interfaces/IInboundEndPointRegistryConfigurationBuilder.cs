#region

using JetBrains.Annotations;

#endregion

namespace MsbRpc.Configuration.Builders.Interfaces;

[PublicAPI]
public interface IInboundEndPointRegistryConfigurationBuilder : IConfigurationWithLoggerFactoryBuilder
{
    string LoggingName { get; set; }
    LogConfigurationBuilder LogRegisteredEndpoint { get; set; }
    LogConfigurationBuilder LogEndpointThrewException { get; set; }
    LogConfigurationBuilder LogDeregisteredEndpoint { get; set; }
    LogConfigurationBuilder LogDeregisteredEndpointOnDisposal { get; set; }
}