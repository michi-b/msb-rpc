using JetBrains.Annotations;

namespace MsbRpc.Configuration.Interfaces;

[PublicAPI]
public interface IOutboundEndPointConfiguration : IEndPointConfiguration
{
    LogConfiguration LogExceptionTransmissionException { get; }
    LogConfiguration LogRemoteRpcException { get; }
    LogConfiguration LogSentAnyRequest { get; }
}