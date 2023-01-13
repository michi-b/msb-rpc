using Microsoft.Extensions.Logging;

namespace MsbRpc;

public static class LogEventIds
{
    public static readonly EventId InboundEndPointStartedListening =
        new((int)Ids.InboundEndPointStartedListening, nameof(Ids.InboundEndPointStartedListening));

    public static readonly EventId InboundEndPointReceivedCall =
        new((int)Ids.InboundEndPointReceivedCall, nameof(Ids.InboundEndPointReceivedCall));

    public static readonly EventId InboundEndPointStoppedListeningWithoutRunningToCompletion = new
    (
        (int)Ids.InboundEndPointStoppedListeningWithoutRunningToCompletion,
        nameof(Ids.InboundEndPointStoppedListeningWithoutRunningToCompletion)
    );

    public static readonly EventId InboundEndPointRanToCompletion =
        new((int)Ids.InboundEndPointRanToCompletion, nameof(Ids.InboundEndPointRanToCompletion));

    public static readonly EventId OutboundEndPointSentRequest =
        new((int)Ids.OutboundEndPointSentRequest, nameof(Ids.OutboundEndPointSentRequest));

    public static readonly EventId ServerWasCreatedWithSpecifiedPort =
        new((int)Ids.ServerWasCreatedWithSpecifiedPort, nameof(Ids.ServerWasCreatedWithSpecifiedPort));

    public static readonly EventId ServerWasCreatedWithEphemeralPort =
        new((int)Ids.ServerWasCreatedWithEphemeralPort, nameof(Ids.ServerWasCreatedWithEphemeralPort));

    public static readonly EventId ServerStartedListening = new((int)Ids.ServerStartedListening, nameof(Ids.ServerStartedListening));

    public static readonly EventId ServerAcceptedNewConnection =
        new((int)Ids.ServerAcceptedNewConnection, nameof(Ids.ServerAcceptedNewConnection));

    public static readonly EventId ServerDisposedNewConnectionAfterDisposal = new
        ((int)Ids.ServerImmediatelyDisposedNewConnection, nameof(Ids.ServerImmediatelyDisposedNewConnection));

    public static readonly EventId ServerStoppedListeningDueToDisposal =
        new((int)Ids.ServerStoppedListeningDueToDisposal, nameof(Ids.ServerStoppedListeningDueToDisposal));

    public static readonly EventId ServerStoppedListeningDueToException = new
        ((int)Ids.ServerStoppedListeningDueToException, nameof(Ids.ServerStoppedListeningDueToException));

    public static readonly EventId ServerEndPointRegistered = new((int)Ids.ServerEndPointRegistered, nameof(Ids.ServerEndPointRegistered));

    public static readonly EventId ServerEndPointDeregistered =
        new((int)Ids.ServerEndPointDeregistered, nameof(Ids.ServerEndPointDeregistered));

    public static readonly EventId ServerEndPointDeregisteredOnRegistryDisposal = new
        ((int)Ids.ServerEndPointDeregisteredOnRegistryDisposal, nameof(Ids.ServerEndPointDeregisteredOnRegistryDisposal));

    public static readonly EventId ServerEndPointThrewException =
        new((int)Ids.ServerEndPointThrewException, nameof(Ids.ServerEndPointThrewException));

    public static readonly EventId MessengerConnectionFailed = new((int)Ids.MessengerConnectionFailed, nameof(Ids.MessengerConnectionFailed));

    private enum Ids
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
}