using MsbRpc.Configuration.Builders.Interfaces;
using MsbRpc.Configuration.Interfaces;

namespace MsbRpc.Configuration;

public class OutboundEndPointConfiguration : EndPointConfiguration, IOutboundEndPointConfiguration
{
    public LogConfiguration LogSentAnyRequest { get; }
    public LogConfiguration LogRemoteRpcException { get; }
    public LogConfiguration LogExceptionTransmissionException { get; }

    public OutboundEndPointConfiguration(IOutboundEndPointConfigurationBuilder builder)
        : base(builder)
    {
        LogSentAnyRequest = builder.LogSentAnyRequest;
        LogRemoteRpcException = builder.LogRemoteRpcException;
        LogExceptionTransmissionException = builder.LogExceptionTransmissionException;
    }
}