using Microsoft.Extensions.Logging;
using MsbRpc.Logging;
using MsbRpc.Servers;

namespace MsbRpc.Configuration;

public class InboundEndpointRegistryConfiguration : Configuration
{
    public LogConfiguration LogDeregisteredEndpoint;
    public LogConfiguration LogDeregisteredEndpointOnDisposal;
    public LogConfiguration LogEndpointThrewException;
    public LogConfiguration LogRegisteredEndpoint;
    
    /// <summary>
    /// prefix for log messages for easier identification
    /// </summary>
    public string LoggingName = nameof(InboundEndPointRegistry); 

    public InboundEndpointRegistryConfiguration()
    {
        LogEndpointThrewException = new LogConfiguration(LogEventIds.ServerEndPointThrewException, LogLevel.Error);
        LogRegisteredEndpoint = new LogConfiguration(LogEventIds.ServerEndPointRegistered, LogLevel.Debug);
        LogDeregisteredEndpoint = new LogConfiguration(LogEventIds.ServerEndPointDeregistered, LogLevel.Debug);
        LogDeregisteredEndpointOnDisposal = new LogConfiguration(LogEventIds.ServerEndPointDeregisteredOnRegistryDisposal, LogLevel.Warning);
    }
}