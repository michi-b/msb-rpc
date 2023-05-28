﻿using JetBrains.Annotations;

namespace MsbRpc.Configuration.Interfaces;

[PublicAPI]
public interface IInboundEndpointRegistryConfiguration : IConfigurationWithLoggerFactory
{
    /// <summary>
    ///     prefix for log messages for easier identification
    /// </summary>
    string LoggingName { get; }

    LogConfiguration LogRegisteredEndpoint { get; }
    LogConfiguration LogEndpointThrewException { get; }
    LogConfiguration LogDeregisteredEndpoint { get; }
    LogConfiguration LogDeregisteredEndpointOnDisposal { get; }
}