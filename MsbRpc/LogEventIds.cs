namespace MsbRpc;

public enum LogEventIds
{
    InboundEndPointStartedListening,
    InboundEndPointReceivedCall,
    InboundEndPointStoppedListening,
    OutboundEndPointRequestedCall,
    ServerWasCreatedWithSpecifiedPort,
    ServerWasCreatedWithEphemeralPort,
    ServerStartedListening,
    ServerStoppedListeningDueToDisposal,
    ServerStoppedListeningDueToException,
    RootEndPointRegistered,
    RootEndPointDeregistered,
    RootEndPointThrewException
    
}