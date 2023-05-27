using JetBrains.Annotations;
using Microsoft.Extensions.Logging;
using MsbRpc.Logging;

namespace MsbRpc.Configuration.Builders;

[PublicAPI]
public class OutboundEndPointConfigurationBuilder : EndPointConfigurationBuilder<OutboundEndPointConfiguration>
{
    public LogConfiguration LogSentAnyRequest { get; set; } = new(LogEventIds.OutboundEndPointSentAnyRequest, LogLevel.Trace);
    public LogConfiguration LogRemoteRpcException { get; set; } = new(LogEventIds.OutboundEndPointRemoteRpcException, LogLevel.Error);
    public LogConfiguration LogExceptionTransmissionException { get; set; } = new(LogEventIds.OutboundEndPointExceptionTransmissionException, LogLevel.Critical);

    public override OutboundEndPointConfiguration Build()
        => new
        (
            InitialBufferSize,
            LoggerFactory,
            LogSentAnyRequest,
            LogRemoteRpcException,
            LogExceptionTransmissionException
        );
}