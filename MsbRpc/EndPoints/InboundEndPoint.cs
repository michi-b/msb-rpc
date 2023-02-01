using System;
using JetBrains.Annotations;
using Microsoft.Extensions.Logging;
using MsbRpc.Attributes;
using MsbRpc.Configuration;
using MsbRpc.Contracts;
using MsbRpc.Exceptions;
using MsbRpc.Extensions;
using MsbRpc.Messaging;
using MsbRpc.Serialization.Buffers;

namespace MsbRpc.EndPoints;

[MayBeUsedByGenerator]
public abstract class InboundEndPoint<TEndPoint, TProcedure, TImplementation> : EndPoint<TEndPoint, TProcedure>
    where TEndPoint : InboundEndPoint<TEndPoint, TProcedure, TImplementation>
    where TImplementation : IRpcContract
    where TProcedure : Enum
{
    private readonly InboundEndPointConfiguration _configuration;
    [PublicAPI] public readonly TImplementation Implementation;

    protected InboundEndPoint
    (
        Messenger messenger,
        TImplementation implementation,
        InboundEndPointConfiguration configuration
    ) : base(messenger, configuration)
    {
        _configuration = configuration;
        Implementation = implementation;
    }

    public void Listen()
    {
        LogStartListening();

        try
        {
            ListenReturnCode listenReturnCode = Messenger.Listen(Buffer, ReceiveMessage);
            if (RanToCompletion)
            {
                LogRanToCompletion(listenReturnCode);
            }
            else
            {
                LogStoppedListeningWithoutRunningToCompletion(listenReturnCode);
            }
        }
        finally
        {
            Dispose();
        }
    }

    public override void Dispose()
    {
        base.Dispose();
        if (Implementation is IDisposable disposableImplementation)
        {
            disposableImplementation.Dispose();
        }
    }

    private bool ReceiveMessage(Message message)
    {
        Request request = new(message);
        TProcedure procedure = GetProcedure(request.ProcedureId);
        LogReceivedCall(procedure, request.Length);

        try
        {
            Response response = Execute(procedure, request);
            RanToCompletion = response.Flags.HasFlag(ResponseFlags.RanToCompletion);
            Messenger.Send(new Message(response));
            return RanToCompletion;
        }
        catch (RpcExecutionException<TProcedure> rpcExecutionException)
        {
            Exception originalException = rpcExecutionException.OriginalException;
            RpcExceptionHandlingInstructions exceptionHandlingInstructions = Implementation.HandleException
                (ref originalException, request.ProcedureId, rpcExecutionException.Stage);
            return HandleException(originalException, rpcExecutionException.Stage, exceptionHandlingInstructions);
        }
    }

    /// <returns>whether listening should stop</returns>
    private bool HandleException(Exception originalException, RpcExecutionStage sourceStage, RpcExceptionHandlingInstructions handlingInstructions)
    {
        try
        {
            if (handlingInstructions.Log && Logger != null)
            {
                LogRpcException(Logger, originalException, sourceStage, handlingInstructions);
            }

            RpcExceptionContinuation continuation = handlingInstructions.Continuation;
            Response response = Buffer.GetFaultedResponse(continuation == RpcExceptionContinuation.MarkRanToCompletion);
            Messenger.Send(new Message(response));
            RpcExceptionTransmission exceptionTransmission = new(originalException, sourceStage, continuation, handlingInstructions.TransmissionOptions);
            Messenger.Send(exceptionTransmission.GetMessage(Buffer));
            return continuation != RpcExceptionContinuation.Continue;
        }
        catch (Exception exception)
        {
            LogExceptionTransmissionException(exception, originalException, handlingInstructions, sourceStage, handlingInstructions.Continuation);
            return true;
        }
    }
    
    protected abstract Response Execute(TProcedure procedure, Request request);
    
    #region Logging

    private void LogRpcException
    (
        ILogger<TEndPoint> logger,
        Exception originalException,
        RpcExecutionStage sourceStage,
        RpcExceptionHandlingInstructions handlingInstructions
    )
    {
        string handlingInstructionsString = handlingInstructions.ToString();
        switch (sourceStage)
        {
            case RpcExecutionStage.None:
                throw new ArgumentException
                (
                    "Cannot log exception with source stage 'None'. This value is reserved for transmitted exceptions.",
                    nameof(sourceStage)
                );
            case RpcExecutionStage.ArgumentDeserialization:
                LogArgumentDeserializationException(logger, originalException, handlingInstructionsString);
                break;
            case RpcExecutionStage.ImplementationExecution:
                LogImplementationExecutionException(logger, originalException, handlingInstructionsString);
                break;
            case RpcExecutionStage.ResponseSerialization:
                LogResponseSerializationException(logger, originalException, handlingInstructionsString);
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(sourceStage), sourceStage, null);
        }
    }

    private void LogExceptionTransmissionException
    (
        Exception exception,
        Exception originalException,
        RpcExceptionHandlingInstructions handlingInstructions,
        RpcExecutionStage rpcExecutionStage,
        RpcExceptionContinuation continuation
    )
    {
        if (Logger != null)
        {
            LogConfiguration configuration = _configuration.LogExceptionTransmissionException;
            if (Logger.GetIsEnabled(configuration))
            {
                Logger.Log
                (
                    configuration.Level,
                    configuration.Id,
                    exception,
                    "Transmission of exception ({ExceptionType}) with message '{ExceptionMessage}' and handling instructions '{HandlingInstructions}'"
                    + " thrown in execution stage {ExecutionStage} and with continuation {RpcExceptionContinuation} failed"
                    + ", and as a result the endpoint is being disposed",
                    originalException.GetType().FullName,
                    originalException.Message,
                    handlingInstructions.ToString(),
                    rpcExecutionStage.GetName(),
                    continuation.GetName()
                );
            }
        }
    }

    private void LogArgumentDeserializationException(ILogger logger, Exception originalException, string handlingInstructionsString)
    {
        LogConfiguration configuration = _configuration.LogArgumentDeserializationException;
        if (logger.GetIsEnabled(configuration))
        {
            logger.Log
            (
                configuration.Level,
                configuration.Id,
                originalException,
                "RPC argument deserialization failed, and the exception is handled with instructions '{HandlingInstructions}'",
                handlingInstructionsString
            );
        }
    }

    private void LogImplementationExecutionException(ILogger<TEndPoint> logger, Exception originalException, string handlingInstructionsString)
    {
        LogConfiguration configuration = _configuration.LogProcedureExecutionException;
        if (logger.GetIsEnabled(configuration))
        {
            logger.Log
            (
                configuration.Level,
                configuration.Id,
                originalException,
                "RPC implementation execution failed, and the exception is handled with instructions '{HandlingInstructions}'",
                handlingInstructionsString
            );
        }
    }

    private void LogResponseSerializationException(ILogger<TEndPoint> logger, Exception originalException, string handlingInstructionsString)
    {
        LogConfiguration configuration = _configuration.LogResponseSerializationException;
        if (logger.GetIsEnabled(configuration))
        {
            logger.Log
            (
                configuration.Level,
                configuration.Id,
                originalException,
                "RPC result serialization failed, and the exception is handled with instructions '{HandlingInstructions}'",
                handlingInstructionsString
            );
        }
    }

    private void LogStoppedListeningWithoutRunningToCompletion(ListenReturnCode listenReturnCode)
    {
        if (Logger != null)
        {
            LogConfiguration configuration = _configuration.LogStoppedListeningWithoutRunningToCompletion;
            if (Logger.GetIsEnabled(configuration))
            {
                Logger.Log
                (
                    configuration.Level,
                    configuration.Id,
                    "Stopped listening without running to completion with listen return code: {ListenReturnCode}",
                    listenReturnCode.GetName()
                );
            }
        }
    }

    private void LogStartListening()
    {
        if (Logger != null)
        {
            LogConfiguration configuration = _configuration.LogStartedListening;
            if (Logger.GetIsEnabled(configuration))
            {
                Logger.Log
                (
                    configuration.Level,
                    configuration.Id,
                    "Started listening"
                );
            }
        }
    }

    private void LogRanToCompletion(ListenReturnCode listenReturnCode)
    {
        if (Logger != null)
        {
            LogConfiguration configuration = _configuration.LogRanToCompletion;
            if (Logger.GetIsEnabled(configuration))
            {
                Logger.Log
                (
                    configuration.Level,
                    configuration.Id,
                    "Ran to completion with listen return code: {ListenReturnCode}",
                    listenReturnCode.GetName()
                );
            }
        }
    }

    private void LogReceivedCall(TProcedure procedure, int argumentByteCount)
    {
        if (Logger != null)
        {
            LogConfiguration configuration = _configuration.LogReceivedCall;
            if (Logger.GetIsEnabled(configuration))
            {
                Logger.Log
                (
                    configuration.Level,
                    configuration.Id,
                    "Received call to {ProcedureName} with {ArgumentByteCount} argument bytes",
                    GetName(procedure),
                    argumentByteCount
                );
            }
        }
    }

    #endregion

}