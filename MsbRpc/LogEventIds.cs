namespace MsbRpc;

public enum LogEventIds
{
    InboundEndPointStartedListening,
    InboundEndPointReceivedCall,
    InboundEndPointStoppedListeningWithoutRunningToCompletion,
    InboundEndPointRanToCompletion,
    OutboundEndPointSentRequest,
    ServerWasCreatedWithSpecifiedPort,
    ServerWasCreatedWithEphemeralPort,
    ServerStartedListening,
    ServerAcceptedNewConnection,
    ServerImmediatelyDisposedNewConnection,
    ServerStoppedListeningDueToDisposal,
    ServerStoppedListeningDueToException,
    ServerEndPointRegistered,
    ServerEndPointDeregistered,
    ServerEndPointDeregisteredOnRegistryDisposal,
    ServerEndPointThrewException,
    MessengerConnectionFailed
}