// using Microsoft.Extensions.Logging;
// using MsbRpc.Configuration;
// using MsbRpc.Test.Integration.LoopBack.Generated;
//
// namespace MsbRpc.Test.Integration.LoopBack;
//
// public class LoopBackInitiatorServerEndPointConfiguration : InboundEndPointConfiguration
// {
//     public IFactory<LoopBackInitiatorServerEndPoint> EndPointFactory { get; }
//
//     public LoopBackInitiatorServerEndPointConfiguration
//     (
//         IFactory<LoopBackInitiatorServerEndPoint> endPointFactory,
//         int initialBufferSize,
//         ILoggerFactory? loggerFactory,
//         string loggingName,
//         LogConfiguration logStartedListening,
//         LogConfiguration logReceivedAnyRequest,
//         LogConfiguration logArgumentDeserializationException,
//         LogConfiguration logExceptionTransmissionException,
//         LogConfiguration logProcedureExecutionException,
//         LogConfiguration logResponseSerializationException,
//         LogConfiguration logRanToCompletion,
//         LogConfiguration logStoppedListeningWithoutRunningToCompletion
//     ) : base
//     (
//         initialBufferSize,
//         loggerFactory,
//         loggingName,
//         logStartedListening,
//         logReceivedAnyRequest,
//         logArgumentDeserializationException,
//         logExceptionTransmissionException,
//         logProcedureExecutionException,
//         logResponseSerializationException,
//         logRanToCompletion,
//         logStoppedListeningWithoutRunningToCompletion
//     )
//     {
//         _endPointFactory = endPointFactory;
//     }
// }