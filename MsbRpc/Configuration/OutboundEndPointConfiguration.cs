﻿using Microsoft.Extensions.Logging;
using MsbRpc.Logging;

namespace MsbRpc.Configuration;

public abstract class OutboundEndPointConfiguration : EndPointConfiguration
{
    public LogConfiguration LogSentAnyRequest;
    public LogConfiguration LogRemoteRpcException;
    public LogConfiguration LogExceptionTransmissionException;

    protected OutboundEndPointConfiguration()
    {
        LogSentAnyRequest = new LogConfiguration(LogEventIds.OutboundEndPointSentAnyRequest, LogLevel.Trace);
        LogRemoteRpcException = new LogConfiguration(LogEventIds.OutboundEndPointRemoteRpcException, LogLevel.Error);
        LogExceptionTransmissionException = new LogConfiguration(LogEventIds.OutboundEndPointExceptionTransmissionException, LogLevel.Critical);
    }
}