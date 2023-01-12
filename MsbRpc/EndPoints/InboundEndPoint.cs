using System;
using JetBrains.Annotations;
using Microsoft.Extensions.Logging;
using MsbRpc.Contracts;
using MsbRpc.EndPoints.Configuration;
using MsbRpc.Extensions;
using MsbRpc.Messaging;
using MsbRpc.Serialization.Buffers;
using MsbRpc.Utility;

namespace MsbRpc.EndPoints;

[PublicAPI(Messages.ForUseInGeneratedCode)]
public abstract class InboundEndPoint<TEndPoint, TProcedure, TImplementation> : EndPoint<TEndPoint, TProcedure>
    where TEndPoint : InboundEndPoint<TEndPoint, TProcedure, TImplementation>
    where TImplementation : IRpcContract
    where TProcedure : Enum
{
    public readonly TImplementation Implementation;

    private InboundEndPointConfiguration _configuration;

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
        Response response = Execute(procedure, request);
        RanToCompletion = response.RanToCompletion;
        Messenger.Send(new Message(response));
        return RanToCompletion;
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
                    "Ran to completion {ListenReturnCode}",
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