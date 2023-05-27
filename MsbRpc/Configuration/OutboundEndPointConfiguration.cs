using Microsoft.Extensions.Logging;
using MsbRpc.Configuration.Interfaces;

namespace MsbRpc.Configuration;

public class OutboundEndPointConfiguration : EndPointConfiguration, IOutboundEndPointConfiguration
{
    public LogConfiguration LogSentAnyRequest { get; }
    public LogConfiguration LogRemoteRpcException { get; }
    public LogConfiguration LogExceptionTransmissionException { get; }

    public OutboundEndPointConfiguration
    (
        int initialBufferSize,
        ILoggerFactory? loggerFactory,
        LogConfiguration logSentAnyRequest,
        LogConfiguration logRemoteRpcException,
        LogConfiguration logExceptionTransmissionException
    )
        : base(initialBufferSize, loggerFactory)
    {
        LogSentAnyRequest = logSentAnyRequest;
        LogRemoteRpcException = logRemoteRpcException;
        LogExceptionTransmissionException = logExceptionTransmissionException;
    }
}