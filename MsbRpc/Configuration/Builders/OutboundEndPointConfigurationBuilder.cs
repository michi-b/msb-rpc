using Microsoft.Extensions.Logging;
using MsbRpc.Configuration.Builders.Abstract;
using MsbRpc.Configuration.Builders.Interfaces;
using MsbRpc.Logging;

namespace MsbRpc.Configuration.Builders;

public class OutboundEndPointConfigurationBuilder : EndPointConfigurationBuilder<OutboundEndPointConfiguration>, IOutboundEndPointConfigurationBuilder
{
    public string LoggingName { get; set; } = "OutboundEndPoint";
    public LogConfigurationBuilder LogSentAnyRequest { get; set; } = new(LogEventIds.OutboundEndPointSentAnyRequest, LogLevel.Trace);
    public LogConfigurationBuilder LogRemoteRpcException { get; set; } = new(LogEventIds.OutboundEndPointRemoteRpcException, LogLevel.Error);
    public LogConfigurationBuilder LogExceptionTransmissionException { get; set; } = new(LogEventIds.OutboundEndPointExceptionTransmissionException, LogLevel.Critical);
    public override OutboundEndPointConfiguration Build() => new(this);
}