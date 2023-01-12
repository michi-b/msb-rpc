﻿using Microsoft.Extensions.Logging;

namespace MsbRpc.EndPoints.Configuration;

public class ServerConfiguration : Configuration
{
    public const int DefaultListenBacklogSize = 100;
    public int Port = 0;
    public int ListenBacklogSize = DefaultListenBacklogSize;
    public LogConfiguration LogDisposedNewConnectionAfterDisposal;
    public LogConfiguration LogStoppedListeningDueToException;
    public LogConfiguration LogStartedListening;
    public LogConfiguration LogStoppedListeningDueToDisposal;
    public LogConfiguration LogWasCreatedWithEphemeralPort;
    public LogConfiguration LogWasCreatedWithSpecifiedPort;
    public LogConfiguration LogAcceptedNewConnection;
    
    //endpoint registry
    public LogConfiguration LogEndpointThrewException;
    public LogConfiguration LogRegisteredEndpoint;
    public LogConfiguration LogDeregisteredEndpoint;
    public LogConfiguration LogDeregisteredEndpointOnDisposal;
    
    public ServerConfiguration()
    {
        LogStoppedListeningDueToException = new LogConfiguration(LogEventIds.ServerStoppedListeningDueToException, LogLevel.Error);
        LogStartedListening = new LogConfiguration(LogEventIds.ServerStartedListening, LogLevel.Information);
        LogStoppedListeningDueToDisposal = new LogConfiguration(LogEventIds.ServerStoppedListeningDueToDisposal, LogLevel.Information);
        LogWasCreatedWithEphemeralPort = new LogConfiguration(LogEventIds.ServerWasCreatedWithEphemeralPort, LogLevel.Information);
        LogWasCreatedWithSpecifiedPort = new LogConfiguration(LogEventIds.ServerWasCreatedWithSpecifiedPort, LogLevel.Information);
        LogAcceptedNewConnection = new LogConfiguration(LogEventIds.ServerAcceptedNewConnection, LogLevel.Trace);
        LogDisposedNewConnectionAfterDisposal = new LogConfiguration(LogEventIds.ServerDisposedNewConnectionAfterDisposal, LogLevel.Warning);
        
        //endpoint registry
        LogEndpointThrewException = new LogConfiguration(LogEventIds.ServerEndPointThrewException, LogLevel.Error);
        LogRegisteredEndpoint = new LogConfiguration(LogEventIds.ServerEndPointRegistered, LogLevel.Debug);
        LogDeregisteredEndpoint = new LogConfiguration(LogEventIds.ServerEndPointDeregistered, LogLevel.Debug);
        LogDeregisteredEndpointOnDisposal = new LogConfiguration(LogEventIds.ServerEndPointDeregisteredOnRegistryDisposal, LogLevel.Warning);
    }
}