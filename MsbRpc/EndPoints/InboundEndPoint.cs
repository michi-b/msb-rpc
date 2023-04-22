﻿using System;
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
public abstract class InboundEndPoint<TProcedure, TImplementation> : EndPoint<TProcedure>, IInboundEndPoint where TImplementation : IRpcContract
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

    protected override void DisposeManagedResources()
    {
        Implementation.Dispose();
        base.DisposeManagedResources();
    }

    private bool ReceiveMessage(Message message)
    {
        Request request = new(message);
        TProcedure procedure = GetProcedure(request.ProcedureId);
        LogReceivedAnyRequest(procedure, request.Length);

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
            Dispose();
            return true;
        }
    }

    protected abstract Response Execute(TProcedure procedure, Request request);

    #region Logging

    private void LogRpcException
    (
        ILogger logger,
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
                    "{LoggingName} failed to transmit exception ({ExceptionType}) with message '{ExceptionMessage}' and handling instructions '{HandlingInstructions}'"
                    + " thrown in execution stage {ExecutionStage} and with continuation {RpcExceptionContinuation}"
                    + ", and as a result the endpoint is being disposed",
                    _configuration.LoggingName,
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
                "{LoggingName} failed to deserialize RPC argument, and the exception is handled with instructions '{HandlingInstructions}'",
                _configuration.LoggingName,
                handlingInstructionsString
            );
        }
    }

    private void LogImplementationExecutionException(ILogger logger, Exception originalException, string handlingInstructionsString)
    {
        LogConfiguration configuration = _configuration.LogProcedureExecutionException;
        if (logger.GetIsEnabled(configuration))
        {
            logger.Log
            (
                configuration.Level,
                configuration.Id,
                originalException,
                "{LoggingName} failed during RPC procedure invocation, and the exception is handled with instructions '{HandlingInstructions}'",
                _configuration.LoggingName,
                handlingInstructionsString
            );
        }
    }

    private void LogResponseSerializationException(ILogger logger, Exception originalException, string handlingInstructionsString)
    {
        LogConfiguration configuration = _configuration.LogResponseSerializationException;
        if (logger.GetIsEnabled(configuration))
        {
            logger.Log
            (
                configuration.Level,
                configuration.Id,
                originalException,
                "{LoggingName} failed to serialize RPC result, and the exception is handled with instructions '{HandlingInstructions}'",
                _configuration.LoggingName,
                handlingInstructionsString
            );
        }
    }

    private void LogStoppedListeningWithoutRunningToCompletion(ListenReturnCode listenReturnCode)
    {
        ILogger<EndPoint<TProcedure>>? logger = Logger;
        if (logger != null)
        {
            LogConfiguration configuration = _configuration.LogStoppedListeningWithoutRunningToCompletion;
            if (logger.GetIsEnabled(configuration))
            {
                logger.Log
                (
                    configuration.Level,
                    configuration.Id,
                    "{LoggingName} stopped listening without running to completion with listen return code: {ListenReturnCode}",
                    _configuration.LoggingName,
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
                    "{LoggingName} started listening",
                    _configuration.LoggingName
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
                    "{LoggingName} ran to completion with listen return code: {ListenReturnCode}",
                    _configuration.LoggingName,
                    listenReturnCode.GetName()
                );
            }
        }
    }

    private void LogReceivedAnyRequest(TProcedure procedure, int argumentByteCount)
    {
        if (Logger != null)
        {
            LogConfiguration configuration = _configuration.LogReceivedAnyRequest;
            if (Logger.GetIsEnabled(configuration))
            {
                Logger.Log
                (
                    configuration.Level,
                    configuration.Id,
                    "{LoggingName} received call to {ProcedureName} with {ArgumentByteCount} argument bytes",
                    _configuration.LoggingName,
                    GetName(procedure),
                    argumentByteCount
                );
            }
        }
    }

    #endregion
}