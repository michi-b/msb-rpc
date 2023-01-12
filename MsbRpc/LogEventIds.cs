using Microsoft.Extensions.Logging;

namespace MsbRpc;

public static class LogEventIds
{
    public static readonly EventId InboundEndPointStartedListening =
        new((int)Values.InboundEndPointStartedListening, nameof(Values.InboundEndPointStartedListening));

    public static readonly EventId InboundEndPointReceivedCall =
        new((int)Values.InboundEndPointReceivedCall, nameof(Values.InboundEndPointReceivedCall));

    public static readonly EventId InboundEndPointStoppedListeningWithoutRunningToCompletion = new
    (
        (int)Values.InboundEndPointStoppedListeningWithoutRunningToCompletion,
        nameof(Values.InboundEndPointStoppedListeningWithoutRunningToCompletion)
    );

    public static readonly EventId InboundEndPointRanToCompletion =
        new((int)Values.InboundEndPointRanToCompletion, nameof(Values.InboundEndPointRanToCompletion));

    public static readonly EventId OutboundEndPointSentRequest =
        new((int)Values.OutboundEndPointSentRequest, nameof(Values.OutboundEndPointSentRequest));

    public static readonly EventId ServerWasCreatedWithSpecifiedPort =
        new((int)Values.ServerWasCreatedWithSpecifiedPort, nameof(Values.ServerWasCreatedWithSpecifiedPort));

    public static readonly EventId ServerWasCreatedWithEphemeralPort =
        new((int)Values.ServerWasCreatedWithEphemeralPort, nameof(Values.ServerWasCreatedWithEphemeralPort));

    public static readonly EventId ServerStartedListening = new((int)Values.ServerStartedListening, nameof(Values.ServerStartedListening));

    public static readonly EventId ServerAcceptedNewConnection =
        new((int)Values.ServerAcceptedNewConnection, nameof(Values.ServerAcceptedNewConnection));

    public static readonly EventId ServerDisposedNewConnectionAfterDisposal = new
        ((int)Values.ServerImmediatelyDisposedNewConnection, nameof(Values.ServerImmediatelyDisposedNewConnection));

    public static readonly EventId ServerStoppedListeningDueToDisposal =
        new((int)Values.ServerStoppedListeningDueToDisposal, nameof(Values.ServerStoppedListeningDueToDisposal));

    public static readonly EventId ServerStoppedListeningDueToException = new
        ((int)Values.ServerStoppedListeningDueToException, nameof(Values.ServerStoppedListeningDueToException));

    public static readonly EventId ServerEndPointRegistered = new((int)Values.ServerEndPointRegistered, nameof(Values.ServerEndPointRegistered));

    public static readonly EventId ServerEndPointDeregistered =
        new((int)Values.ServerEndPointDeregistered, nameof(Values.ServerEndPointDeregistered));

    public static readonly EventId ServerEndPointDeregisteredOnRegistryDisposal = new
        ((int)Values.ServerEndPointDeregisteredOnRegistryDisposal, nameof(Values.ServerEndPointDeregisteredOnRegistryDisposal));

    public static readonly EventId ServerEndPointThrewException =
        new((int)Values.ServerEndPointThrewException, nameof(Values.ServerEndPointThrewException));

    public static readonly EventId MessengerConnectionFailed = new((int)Values.MessengerConnectionFailed, nameof(Values.MessengerConnectionFailed));

    private enum Values
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