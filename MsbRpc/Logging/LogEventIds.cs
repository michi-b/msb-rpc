using Microsoft.Extensions.Logging;

namespace MsbRpc.Logging;

public static class LogEventIds
{
    public static readonly EventId InboundEndPointStartedListening = new((int)Ids.InboundEndPointStartedListening, nameof(Ids.InboundEndPointStartedListening));

    public static readonly EventId InboundEndPointReceivedAnyRequest = new((int)Ids.InboundEndPointReceivedCall, nameof(Ids.InboundEndPointReceivedCall));

    public static readonly EventId InboundEndPointStoppedListeningWithoutRunningToCompletion = new
    (
        (int)Ids.InboundEndPointStoppedListeningWithoutRunningToCompletion,
        nameof(Ids.InboundEndPointStoppedListeningWithoutRunningToCompletion)
    );

    public static readonly EventId InboundEndPointRanToCompletion = new((int)Ids.InboundEndPointRanToCompletion, nameof(Ids.InboundEndPointRanToCompletion));

    public static EventId InboundEndPointArgumentDeserializationException = new
        ((int)Ids.InboundEndPointArgumentDeserializationException, nameof(Ids.InboundEndPointArgumentDeserializationException));

    public static EventId InboundEndPointProcedureExecutionException =
        new((int)Ids.InboundEndPointProcedureExecutionException, nameof(Ids.InboundEndPointProcedureExecutionException));

    public static EventId InboundEndPointResponseSerializationException =
        new((int)Ids.InboundEndPointResponseSerializationException, nameof(Ids.InboundEndPointResponseSerializationException));

    public static EventId InboundEndPointExceptionTransmissionException =
        new((int)Ids.InboundEndPointExceptionTransmissionException, nameof(Ids.InboundEndPointExceptionTransmissionException));

    public static readonly EventId OutboundEndPointSentAnyRequest = new((int)Ids.OutboundEndPointSentRequest, nameof(Ids.OutboundEndPointSentRequest));

    public static EventId OutboundEndPointRemoteRpcException = new((int)Ids.OutboundEndPointRemoteRpcException, nameof(Ids.OutboundEndPointRemoteRpcException));

    public static EventId OutboundEndPointExceptionTransmissionException = new
        ((int)Ids.OutboundEndPointExceptionTransmissionException, nameof(Ids.OutboundEndPointExceptionTransmissionException));

    public static readonly EventId MessengerListenerWasCreated = new((int)Ids.MessengerListenerWasCreated, nameof(Ids.MessengerListenerWasCreated));

    public static readonly EventId MessengerListenerStartedListening = new((int)Ids.MessengerListenerStartedListening, nameof(Ids.MessengerListenerStartedListening));

    public static readonly EventId MessengerListenerAcceptedNewUnIdentifiedConnection =
        new((int)Ids.MessengerListenerAcceptedNewUnIdentifiedConnection, nameof(Ids.MessengerListenerAcceptedNewUnIdentifiedConnection));

    public static readonly EventId MessengerListenerAcceptedNewIdentifiedConnection =
        new((int)Ids.MessengerListenerAcceptedNewIdentifiedConnection, nameof(Ids.MessengerListenerAcceptedNewIdentifiedConnection));

    public static readonly EventId MessengerListenerDeclinedNewConnectionDuringDisposal
        = new((int)Ids.MessengerListenerDeclinedNewConnectionDuringDisposal, nameof(Ids.MessengerListenerDeclinedNewConnectionDuringDisposal));

    public static readonly EventId MessengerListenerStoppedListeningDueToDisposal
        = new((int)Ids.MessengerListenerStoppedListeningDueToDisposal, nameof(Ids.MessengerListenerStoppedListeningDueToDisposal));

    public static readonly EventId MessengerListenerDeclinedNewConnectionDueToException
        = new((int)Ids.MessengerListenerDeclinedNewConnectionDueToException, nameof(Ids.MessengerListenerDeclinedNewConnectionDueToException));

    public static readonly EventId MessengerListenerStoppedListeningDueToException
        = new((int)Ids.MessengerListenerStoppedListeningDueToException, nameof(Ids.MessengerListenerStoppedListeningDueToException));

    public static readonly EventId ServerEndPointRegistered = new((int)Ids.ServerEndPointRegistered, nameof(Ids.ServerEndPointRegistered));

    public static readonly EventId ServerEndPointDeregistered = new((int)Ids.ServerEndPointDeregistered, nameof(Ids.ServerEndPointDeregistered));

    public static readonly EventId ServerEndPointDeregisteredOnRegistryDisposal = new
        ((int)Ids.ServerEndPointDeregisteredOnRegistryDisposal, nameof(Ids.ServerEndPointDeregisteredOnRegistryDisposal));

    public static readonly EventId ServerEndPointThrewException = new((int)Ids.ServerEndPointThrewException, nameof(Ids.ServerEndPointThrewException));

    public static readonly EventId MessengerConnectionFailed = new((int)Ids.MessengerConnectionFailed, nameof(Ids.MessengerConnectionFailed));

    private enum Ids
    {
        InboundEndPointStartedListening,
        InboundEndPointReceivedCall,
        InboundEndPointStoppedListeningWithoutRunningToCompletion,
        InboundEndPointRanToCompletion,
        InboundEndPointArgumentDeserializationException,
        InboundEndPointProcedureExecutionException,
        InboundEndPointResponseSerializationException,
        InboundEndPointExceptionTransmissionException,
        OutboundEndPointSentRequest,
        OutboundEndPointRemoteRpcException,
        OutboundEndPointExceptionTransmissionException,
        MessengerListenerWasCreated,
        MessengerListenerStartedListening,
        MessengerListenerAcceptedNewUnIdentifiedConnection,
        MessengerListenerAcceptedNewIdentifiedConnection,
        MessengerListenerDeclinedNewConnectionDuringDisposal,
        MessengerListenerDeclinedNewConnectionDueToException,
        MessengerListenerStoppedListeningDueToDisposal,
        MessengerListenerStoppedListeningDueToException,
        ServerEndPointRegistered,
        ServerEndPointDeregistered,
        ServerEndPointDeregisteredOnRegistryDisposal,
        ServerEndPointThrewException,
        MessengerConnectionFailed
    }
}