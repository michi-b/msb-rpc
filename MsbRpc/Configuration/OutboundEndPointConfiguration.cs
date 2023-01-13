﻿using Microsoft.Extensions.Logging;
using MsbRpc.Logging;

namespace MsbRpc.Configuration;

public abstract class OutboundEndPointConfiguration : EndPointConfiguration
{
    public LogConfiguration LogSentCall;

    protected OutboundEndPointConfiguration() => LogSentCall = new LogConfiguration(LogEventIds.OutboundEndPointSentRequest, LogLevel.Trace);
}