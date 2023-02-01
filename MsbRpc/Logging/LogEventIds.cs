using Microsoft.Extensions.Logging;
using MsbRpc.Attributes;

namespace MsbRpc.Logging;

public static class LogEventIds
{
    [MayBeUsedByGenerator] public const int FirstAvailable = 17;

    public static readonly EventId InboundEndPointStartedListening = new((int)Ids.InboundEndPointStartedListening, nameof(Ids.InboundEndPointStartedListening));

    public static readonly EventId InboundEndPointReceivedCall = new((int)Ids.InboundEndPointReceivedCall, nameof(Ids.InboundEndPointReceivedCall));

    public static readonly EventId InboundEndPointStoppedListeningWithoutRunningToCompletion = new
    (
        (int)Ids.InboundEndPointStoppedListeningWithoutRunningToCompletion,
        nameof(Ids.InboundEndPointStoppedListeningWithoutRunningToCompletion)
    );

    public static readonly EventId InboundEndPointRanToCompletion = new((int)Ids.InboundEndPointRanToCompletion, nameof(Ids.InboundEndPointRanToCompletion));

    public static readonly EventId OutboundEndPointSentRequest = new((int)Ids.OutboundEndPointSentRequest, nameof(Ids.OutboundEndPointSentRequest));

    public static readonly EventId ServerWasCreatedWithSpecifiedPort = new((int)Ids.ServerWasCreatedWithSpecifiedPort, nameof(Ids.ServerWasCreatedWithSpecifiedPort));

    public static readonly EventId ServerWasCreatedWithEphemeralPort = new((int)Ids.ServerWasCreatedWithEphemeralPort, nameof(Ids.ServerWasCreatedWithEphemeralPort));

    public static readonly EventId ServerStartedListening = new((int)Ids.ServerStartedListening, nameof(Ids.ServerStartedListening));

    public static readonly EventId ServerAcceptedNewConnection = new((int)Ids.ServerAcceptedNewConnection, nameof(Ids.ServerAcceptedNewConnection));

    public static readonly EventId ServerDisposedNewConnectionAfterDisposal
        = new((int)Ids.ServerImmediatelyDisposedNewConnection, nameof(Ids.ServerImmediatelyDisposedNewConnection));

    public static readonly EventId ServerStoppedListeningDueToDisposal
        = new((int)Ids.ServerStoppedListeningDueToDisposal, nameof(Ids.ServerStoppedListeningDueToDisposal));

    public static readonly EventId ServerStoppedListeningDueToException
        = new((int)Ids.ServerStoppedListeningDueToException, nameof(Ids.ServerStoppedListeningDueToException));

    public static readonly EventId ServerEndPointRegistered = new((int)Ids.ServerEndPointRegistered, nameof(Ids.ServerEndPointRegistered));

    public static readonly EventId ServerEndPointDeregistered = new((int)Ids.ServerEndPointDeregistered, nameof(Ids.ServerEndPointDeregistered));

    public static readonly EventId ServerEndPointDeregisteredOnRegistryDisposal = new
        ((int)Ids.ServerEndPointDeregisteredOnRegistryDisposal, nameof(Ids.ServerEndPointDeregisteredOnRegistryDisposal));

    public static readonly EventId ServerEndPointThrewException = new((int)Ids.ServerEndPointThrewException, nameof(Ids.ServerEndPointThrewException));

    public static readonly EventId MessengerConnectionFailed = new((int)Ids.MessengerConnectionFailed, nameof(Ids.MessengerConnectionFailed));

    public static EventId InboundEndPointArgumentDeserializationException = new
        ((int)Ids.InboundEndPointArgumentDeserializationException, nameof(Ids.InboundEndPointArgumentDeserializationException));

    public static EventId InboundEndPointProcedureExecutionException =
        new((int)Ids.InboundEndPointProcedureExecutionException, nameof(Ids.InboundEndPointProcedureExecutionException));

    public static EventId InboundEndPointResponseSerializationException =
        new((int)Ids.InboundEndPointResponseSerializationException, nameof(Ids.InboundEndPointResponseSerializationException));

    public static EventId InboundEndPointExceptionTransmissionException =
        new((int)Ids.InboundEndPointExceptionTransmissionException, nameof(Ids.InboundEndPointExceptionTransmissionException));

    private enum Ids
    {
        InboundEndPointStartedListening = 0,
        InboundEndPointReceivedCall = 1,
        InboundEndPointStoppedListeningWithoutRunningToCompletion = 2,
        InboundEndPointRanToCompletion = 3,
        InboundEndPointArgumentDeserializationException = 4,
        InboundEndPointProcedureExecutionException = 5,
        InboundEndPointResponseSerializationException = 6,
        InboundEndPointExceptionTransmissionException = 7,
        OutboundEndPointSentRequest = 8,
        ServerWasCreatedWithSpecifiedPort = 9,
        ServerWasCreatedWithEphemeralPort = 10,
        ServerStartedListening = 11,
        ServerAcceptedNewConnection = 12,
        ServerImmediatelyDisposedNewConnection = 13,
        ServerStoppedListeningDueToDisposal = 14,
        ServerStoppedListeningDueToException = 15,
        ServerEndPointRegistered = 16,
        ServerEndPointDeregistered = 17,
        ServerEndPointDeregisteredOnRegistryDisposal = 18,
        ServerEndPointThrewException = 19,
        MessengerConnectionFailed = 20
    }
}