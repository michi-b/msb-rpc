using JetBrains.Annotations;

namespace MsbRpc.Configuration.Builders.Interfaces;

[PublicAPI]
public interface IOutboundEndPointConfigurationBuilder : IEndPointConfigurationBuilder
{
    LogConfigurationBuilder LogSentAnyRequest { get; set; }
    LogConfigurationBuilder LogRemoteRpcException { get; set; }
    LogConfigurationBuilder LogExceptionTransmissionException { get; set; }
    string LoggingName { get; set; }
}