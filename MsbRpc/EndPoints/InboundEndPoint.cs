﻿using System;
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
    public readonly TImplementation Implementation;

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
            return HandleException(originalException, exceptionHandlingInstructions, rpcExecutionException.Stage);
        }
    }

    /// <returns>whether listening should stop</returns>
    private bool HandleException(Exception originalException, RpcExceptionHandlingInstructions exceptionHandlingInstructions, RpcExecutionStage rpcExecutionStage)
    {
        Response response = Buffer.GetFaultedResponse(exceptionHandlingInstructions.Continuation == RpcExceptionContinuation.RanToCompletion);
        Messenger.Send(new Message(response));
        RpcExceptionTransmission exceptionTransmission = new(originalException, rpcExecutionStage, exceptionHandlingInstructions.TransmissionOptions);
        Messenger.Send(exceptionTransmission.GetMessage(Buffer));
        return exceptionHandlingInstructions.Continuation != RpcExceptionContinuation.Continue;
    }

    protected Message GetResultMessageBuffer(int count) => Buffer.GetMessage(count);

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

    protected abstract Response Execute(TProcedure procedure, Request request);
}