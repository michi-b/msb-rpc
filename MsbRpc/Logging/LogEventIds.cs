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

    public static readonly EventId ConnectionListenerWasCreatedWithSpecifiedPort = new((int)Ids.ConnectionListenerWasCreatedWithSpecifiedPort, nameof(Ids.ConnectionListenerWasCreatedWithSpecifiedPort));

    public static readonly EventId ConnectionListenerWasCreatedWithEphemeralPort = new((int)Ids.ConnectionListenerWasCreatedWithEphemeralPort, nameof(Ids.ConnectionListenerWasCreatedWithEphemeralPort));

    public static readonly EventId ConnectionListenerStartedListening = new((int)Ids.ConnectionListenerStartedListening, nameof(Ids.ConnectionListenerStartedListening));

    public static readonly EventId ConnectionListenerAcceptedNewUnIdentifiedConnection =
        new((int)Ids.ConnectionListenerAcceptedNewUnIdentifiedConnection, nameof(Ids.ConnectionListenerAcceptedNewUnIdentifiedConnection));

    public static readonly EventId ConnectionListenerAcceptedNewIdentifiedConnection =
        new((int)Ids.ConnectionListenerAcceptedNewIdentifiedConnection, nameof(Ids.ConnectionListenerAcceptedNewIdentifiedConnection));

    public static readonly EventId ConnectionListenerDeclinedNewConnectionDuringDisposal
        = new((int)Ids.ConnectionListenerDeclinedNewConnectionDuringDisposal, nameof(Ids.ConnectionListenerDeclinedNewConnectionDuringDisposal));

    public static readonly EventId ConnectionListenerStoppedListeningDueToDisposal
        = new((int)Ids.ConnectionListenerStoppedListeningDueToDisposal, nameof(Ids.ConnectionListenerStoppedListeningDueToDisposal));

    public static readonly EventId ConnectionListenerDeclinedNewConnectionDueToException
        = new((int)Ids.ConnectionListenerDeclinedNewConnectionDueToException, nameof(Ids.ConnectionListenerDeclinedNewConnectionDueToException));

    public static readonly EventId ConnectionListenerStoppedListeningDueToException
        = new((int)Ids.ConnectionListenerStoppedListeningDueToException, nameof(Ids.ConnectionListenerStoppedListeningDueToException));

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
        ConnectionListenerWasCreatedWithSpecifiedPort,
        ConnectionListenerWasCreatedWithEphemeralPort,
        ConnectionListenerStartedListening,
        ConnectionListenerAcceptedNewUnIdentifiedConnection,
        ConnectionListenerAcceptedNewIdentifiedConnection,
        ConnectionListenerDeclinedNewConnectionDuringDisposal,
        ConnectionListenerDeclinedNewConnectionDueToException,
        ConnectionListenerStoppedListeningDueToDisposal,
        ConnectionListenerStoppedListeningDueToException,
        ServerEndPointRegistered,
        ServerEndPointDeregistered,
        ServerEndPointDeregisteredOnRegistryDisposal,
        ServerEndPointThrewException,
        MessengerConnectionFailed
    }
}