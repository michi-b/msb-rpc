#region

using Microsoft.Extensions.Logging;
using MsbRpc.Configuration.Builders.Interfaces;

#endregion

namespace MsbRpc.Configuration;

public readonly struct OutboundEndPointConfiguration
{
    public readonly int InitialBufferSize;
    public readonly ILoggerFactory? LoggerFactory;
    public readonly string LoggingName;
    public readonly LogConfiguration LogSentAnyRequest;
    public readonly LogConfiguration LogRemoteRpcException;
    public readonly LogConfiguration LogExceptionTransmissionException;

    public OutboundEndPointConfiguration(IOutboundEndPointConfigurationBuilder builder)
    {
        InitialBufferSize = builder.InitialBufferSize;
        LoggerFactory = builder.LoggerFactory;
        LoggingName = builder.LoggingName;
        LogSentAnyRequest = builder.LogSentAnyRequest;
        LogRemoteRpcException = builder.LogRemoteRpcException;
        LogExceptionTransmissionException = builder.LogExceptionTransmissionException;
    }
}