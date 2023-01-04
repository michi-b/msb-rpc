namespace MsbRpc;

public enum LogEventIds
{
    InboundEndPointStartedListening,
    InboundEndPointReceivedCall,
    InboundEndPointStoppedListening,
    OutboundEndPointSentRequest,
    ServerWasCreatedWithSpecifiedPort,
    ServerWasCreatedWithEphemeralPort,
    ServerStartedListening,
    ServerAcceptedNewConnection,
    ServerImmediatelyDisposedNewConnection,
    ServerStoppedListeningDueToDisposal,
    ServerStoppedListeningDueToException,
    RootEndPointRegistered,
    RootEndPointDeregistered,
    RootEndPointDeregisteredOnRegistryDisposal,
    RootEndPointThrewException,
    MessengerConnectionFailed
}