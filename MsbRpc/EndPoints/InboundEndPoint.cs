using System;
using System.Diagnostics;
using JetBrains.Annotations;
using Microsoft.Extensions.Logging;
using MsbRpc.Attributes;
using MsbRpc.Configuration;
using MsbRpc.Contracts;
using MsbRpc.EndPoints.Interfaces;
using MsbRpc.Exceptions;
using MsbRpc.Extensions;
using MsbRpc.Messaging;
using MsbRpc.Serialization.Buffers;

namespace MsbRpc.EndPoints;

[MayBeUsedByGeneratedCode]
[DebuggerDisplay("{DebuggerDisplay,nq}")]
public abstract class InboundEndPoint<TProcedure, TImplementation> : EndPoint<TProcedure>, IInboundEndPoint where TImplementation : IRpcContract
    where TProcedure : Enum
{
    private readonly ILogger<InboundEndPoint<TProcedure, TImplementation>>? _logger;

    [MayBeUsedByGeneratedCode] protected readonly InboundEndPointConfiguration Configuration;

    [PublicAPI] public readonly TImplementation Implementation;

    protected InboundEndPoint
    (
        Messenger messenger,
        TImplementation implementation,
        in InboundEndPointConfiguration configuration
    ) : base(messenger, new RpcBuffer(configuration.InitialBufferSize), configuration.LoggingName)
    {
        _logger = configuration.LoggerFactory?.CreateLogger<InboundEndPoint<TProcedure, TImplementation>>();
        Configuration = configuration;
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

    protected override void Dispose(bool disposing)
    {
        Implementation.Dispose();
        base.Dispose(disposing);
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
            if (handlingInstructions.Log && _logger != null)
            {
                LogRpcException(_logger, originalException, sourceStage, handlingInstructions);
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
        ILogger<InboundEndPoint<TProcedure, TImplementation>> logger,
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
                    $"{LoggingName} Cannot log exception with source stage 'None'. This value is reserved for transmitted exceptions.",
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
        if (_logger != null)
        {
            LogConfiguration configuration = Configuration.LogExceptionTransmissionException;
            if (_logger.GetIsEnabled(configuration))
            {
                _logger.Log
                (
                    configuration.Level,
                    configuration.Id,
                    exception,
                    "{Name} failed to transmit exception ({ExceptionType}) with message '{ExceptionMessage}' and handling instructions '{HandlingInstructions}'"
                    + " thrown in execution stage {ExecutionStage} and with continuation {RpcExceptionContinuation}"
                    + ", and as a result the endpoint is being disposed",
                    LoggingName,
                    originalException.GetType().FullName,
                    originalException.Message,
                    handlingInstructions.ToString(),
                    rpcExecutionStage.GetName(),
                    continuation.GetName()
                );
            }
        }
    }

    private void LogArgumentDeserializationException
        (ILogger<InboundEndPoint<TProcedure, TImplementation>> logger, Exception originalException, string handlingInstructionsString)
    {
        LogConfiguration configuration = Configuration.LogArgumentDeserializationException;
        if (logger.GetIsEnabled(configuration))
        {
            logger.Log
            (
                configuration.Level,
                configuration.Id,
                originalException,
                "{Name} failed to deserialize RPC argument, and the exception is handled with instructions '{HandlingInstructions}'",
                LoggingName,
                handlingInstructionsString
            );
        }
    }

    private void LogImplementationExecutionException
        (ILogger<InboundEndPoint<TProcedure, TImplementation>> logger, Exception originalException, string handlingInstructionsString)
    {
        LogConfiguration configuration = Configuration.LogProcedureExecutionException;
        if (logger.GetIsEnabled(configuration))
        {
            logger.Log
            (
                configuration.Level,
                configuration.Id,
                originalException,
                "{Name} failed during RPC procedure invocation, and the exception is handled with instructions '{HandlingInstructions}'",
                LoggingName,
                handlingInstructionsString
            );
        }
    }

    private void LogResponseSerializationException
        (ILogger<InboundEndPoint<TProcedure, TImplementation>> logger, Exception originalException, string handlingInstructionsString)
    {
        LogConfiguration configuration = Configuration.LogResponseSerializationException;
        if (logger.GetIsEnabled(configuration))
        {
            logger.Log
            (
                configuration.Level,
                configuration.Id,
                originalException,
                "{Name} failed to serialize RPC result, and the exception is handled with instructions '{HandlingInstructions}'",
                LoggingName,
                handlingInstructionsString
            );
        }
    }

    private void LogStoppedListeningWithoutRunningToCompletion(ListenReturnCode listenReturnCode)
    {
        if (_logger != null)
        {
            LogConfiguration configuration = Configuration.LogStoppedListeningWithoutRunningToCompletion;
            if (_logger.GetIsEnabled(configuration))
            {
                _logger.Log
                (
                    configuration.Level,
                    configuration.Id,
                    "{Name} stopped listening without running to completion with listen return code: {ListenReturnCode}",
                    LoggingName,
                    listenReturnCode.GetName()
                );
            }
        }
    }

    private void LogStartListening()
    {
        if (_logger != null)
        {
            LogConfiguration configuration = Configuration.LogStartedListening;
            if (_logger.GetIsEnabled(configuration))
            {
                _logger.Log
                (
                    configuration.Level,
                    configuration.Id,
                    "{Name} started listening",
                    LoggingName
                );
            }
        }
    }

    private void LogRanToCompletion(ListenReturnCode listenReturnCode)
    {
        if (_logger != null)
        {
            LogConfiguration configuration = Configuration.LogRanToCompletion;
            if (_logger.GetIsEnabled(configuration))
            {
                _logger.Log
                (
                    configuration.Level,
                    configuration.Id,
                    "{Name} ran to completion with listen return code: {ListenReturnCode}",
                    LoggingName,
                    listenReturnCode.GetName()
                );
            }
        }
    }

    private void LogReceivedAnyRequest(TProcedure procedure, int argumentByteCount)
    {
        if (_logger != null)
        {
            LogConfiguration configuration = Configuration.LogReceivedAnyRequest;
            if (_logger.GetIsEnabled(configuration))
            {
                _logger.Log
                (
                    configuration.Level,
                    configuration.Id,
                    "{Name} received call to {ProcedureName} with {ArgumentByteCount} argument bytes",
                    LoggingName,
                    GetName(procedure),
                    argumentByteCount
                );
            }
        }
    }

    #endregion
}