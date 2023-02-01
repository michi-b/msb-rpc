using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using MsbRpc.Attributes;
using MsbRpc.Configuration;
using MsbRpc.Exceptions;
using MsbRpc.Exceptions.Generic;
using MsbRpc.Extensions;
using MsbRpc.Messaging;
using MsbRpc.Serialization.Buffers;

namespace MsbRpc.EndPoints;

[MayBeUsedByGenerator]
public abstract class OutboundEndPoint<TEndPoint, TProcedure> : EndPoint<TEndPoint, TProcedure>
    where TEndPoint : OutboundEndPoint<TEndPoint, TProcedure>
    where TProcedure : Enum
{
    private readonly OutboundEndPointConfiguration _configuration;

    protected OutboundEndPoint
    (
        Messenger messenger,
        OutboundEndPointConfiguration configuration
    )
        : base(messenger, configuration)
        => _configuration = configuration;

    private static RpcRequestException<TProcedure> GetConnectionClosedException(TProcedure procedure)
        => new(procedure, "Connection closed while waiting for the response.");

    private static RpcRequestException<TProcedure> GetConnectionDisposedException(TProcedure procedure)
        => new(procedure, "Connection disposed while waiting for the response.");

    [MayBeUsedByGenerator]
    protected async ValueTask<Response> SendRequestAsync(Request request)
    {
        TProcedure procedure = GetProcedure(request.ProcedureId);

        await Messenger.SendAsync(new Message(request));

        LogSentCall(procedure, request.Buffer.Count);

        ReceiveResult result = await Messenger.ReceiveMessageAsync(Buffer);

        switch (result.ReturnCode)
        {
            case ReceiveReturnCode.Success:
                Response response = new(result.Message);
                if ((response.Flags & ResponseFlags.RanToCompletion) != 0)
                {
                    RanToCompletion = true;
                    Dispose();
                }

                if ((response.Flags & ResponseFlags.Faulted) != 0)
                {
                    await HandleFaultedRpcInvocation(procedure);
                }

                return response;
            case ReceiveReturnCode.ConnectionClosed:
                throw GetConnectionClosedException(procedure);
            case ReceiveReturnCode.ConnectionDisposed:
                throw GetConnectionDisposedException(procedure);
            default:
                throw new ArgumentOutOfRangeException();
        }
    }

    private async ValueTask HandleFaultedRpcInvocation(TProcedure procedure)
    {
        try
        {
            ReceiveResult exceptionTransmissionResult = await Messenger.ReceiveMessageAsync(Buffer);
            switch (exceptionTransmissionResult.ReturnCode)
            {
                case ReceiveReturnCode.Success:
                    RpcExceptionTransmission exceptionTransmission = RpcExceptionTransmission.Read(exceptionTransmissionResult.Message);
                    switch (exceptionTransmission.Continuation)
                    {
                        case RemoteContinuation.Disposed:
                            Dispose();
                            break;
                        case RemoteContinuation.RanToCompletion:
                            RanToCompletion = true;
                            Dispose();
                            break;
                        case RemoteContinuation.Undefined:
                        case RemoteContinuation.Continues:
                            break;
                        default:
                            throw new ArgumentOutOfRangeException();
                    }

                    throw new RpcRemoteException<TProcedure>(exceptionTransmission, procedure);
                case ReceiveReturnCode.ConnectionClosed:
                    throw GetConnectionClosedException(procedure);
                case ReceiveReturnCode.ConnectionDisposed:
                    throw GetConnectionDisposedException(procedure);
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
        catch (RpcRemoteException<TProcedure> remoteException)
        {
            throw;
        }
        catch (Exception exception)
        {
            Dispose();
            throw new RpcExceptionTransmissionException(exception);
        }
    }

    [MayBeUsedByGenerator]
    protected Message SendRequest(Request request)
    {
        Messenger.Send(new Message(request));

        TProcedure procedure = GetProcedure(request.ProcedureId);

        LogSentCall(procedure, request.Buffer.Count);

        ReceiveResult result = Messenger.ReceiveMessage(Buffer);

        return result.ReturnCode switch
        {
            ReceiveReturnCode.Success => result.Message,
            ReceiveReturnCode.ConnectionClosed => throw GetConnectionClosedException(procedure),
            ReceiveReturnCode.ConnectionDisposed => throw GetConnectionDisposedException(procedure),
            _ => throw new ArgumentOutOfRangeException()
        };
    }

    private void LogSentCall(TProcedure procedure, int argumentByteCount)
    {
        if (Logger != null)
        {
            LogConfiguration configuration = _configuration.LogSentCall;
            if (Logger.GetIsEnabled(configuration))
            {
                Logger.Log
                (
                    configuration.Level,
                    configuration.Id,
                    "Sent a request to {ProcedureName} with {ArgumentByteCount} argument bytes",
                    GetName(procedure),
                    argumentByteCount
                );
            }
        }
    }
}