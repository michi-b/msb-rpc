using Microsoft.Extensions.Logging;
using MsbRpc.Configuration.Builders.Interfaces;

namespace MsbRpc.Configuration;

public readonly struct OutboundEndPointConfiguration
{
    public readonly int InitialBufferSize;

    public readonly ILoggerFactory? LoggerFactory;

    public readonly LogConfiguration LogSentAnyRequest;
    public readonly LogConfiguration LogRemoteRpcException;
    public readonly LogConfiguration LogExceptionTransmissionException;

    public OutboundEndPointConfiguration(IOutboundEndPointConfigurationBuilder builder)
    {
        InitialBufferSize = builder.InitialBufferSize;
        LoggerFactory = builder.LoggerFactory;
        LogSentAnyRequest = builder.LogSentAnyRequest;
        LogRemoteRpcException = builder.LogRemoteRpcException;
        LogExceptionTransmissionException = builder.LogExceptionTransmissionException;
    }
}