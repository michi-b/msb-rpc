namespace MsbRpc.Configuration.Interfaces;

public interface IOutboundEndPointConfiguration : IEndPointConfiguration
{
    LogConfiguration LogExceptionTransmissionException { get; }
    LogConfiguration LogRemoteRpcException { get; }
    LogConfiguration LogSentAnyRequest { get; }
}