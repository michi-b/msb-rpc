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

    public static readonly EventId ServerWasCreatedWithSpecifiedPort = new((int)Ids.ServerWasCreatedWithSpecifiedPort, nameof(Ids.ServerWasCreatedWithSpecifiedPort));

    public static readonly EventId ServerWasCreatedWithEphemeralPort = new((int)Ids.ServerWasCreatedWithEphemeralPort, nameof(Ids.ServerWasCreatedWithEphemeralPort));

    public static readonly EventId ServerStartedListening = new((int)Ids.ServerStartedListening, nameof(Ids.ServerStartedListening));

    public static readonly EventId ServerAcceptedNewUnIdentifiedConnection =
        new((int)Ids.ServerAcceptedNewUnIdentifiedConnection, nameof(Ids.ServerAcceptedNewUnIdentifiedConnection));

    public static readonly EventId ServerAcceptedNewIdentifiedConnection =
        new((int)Ids.ServerAcceptedNewIdentifiedConnection, nameof(Ids.ServerAcceptedNewIdentifiedConnection));

    public static readonly EventId ServerDeclinedNewConnectionDuringDisposal
        = new((int)Ids.ServerDeclinedNewConnectionDuringDisposal, nameof(Ids.ServerDeclinedNewConnectionDuringDisposal));

    public static readonly EventId ServerStoppedListeningDueToDisposal
        = new((int)Ids.ServerStoppedListeningDueToDisposal, nameof(Ids.ServerStoppedListeningDueToDisposal));

    public static readonly EventId ServerDeclinedNewConnectionDueToException
        = new((int)Ids.ServerDeclinedNewConnectionDueToException, nameof(Ids.ServerDeclinedNewConnectionDueToException));

    public static readonly EventId ServerStoppedListeningDueToException
        = new((int)Ids.ServerStoppedListeningDueToException, nameof(Ids.ServerStoppedListeningDueToException));

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
        ServerWasCreatedWithSpecifiedPort,
        ServerWasCreatedWithEphemeralPort,
        ServerStartedListening,
        ServerAcceptedNewUnIdentifiedConnection,
        ServerAcceptedNewIdentifiedConnection,
        ServerDeclinedNewConnectionDuringDisposal,
        ServerDeclinedNewConnectionDueToException,
        ServerStoppedListeningDueToDisposal,
        ServerStoppedListeningDueToException,
        ServerEndPointRegistered,
        ServerEndPointDeregistered,
        ServerEndPointDeregisteredOnRegistryDisposal,
        ServerEndPointThrewException,
        MessengerConnectionFailed
    }
}