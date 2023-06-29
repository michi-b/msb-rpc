using System;
using System.Diagnostics;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using MsbRpc.Attributes;
using MsbRpc.Configuration;
using MsbRpc.EndPoints.Interfaces;
using MsbRpc.Exceptions;
using MsbRpc.Exceptions.Generic;
using MsbRpc.Extensions;
using MsbRpc.Messaging;
using MsbRpc.Serialization.Buffers;

namespace MsbRpc.EndPoints;

[MayBeUsedByGeneratedCode]
[DebuggerDisplay("{DebuggerDisplay,nq}")]
public abstract class OutboundEndPoint<TProcedure> : EndPoint<TProcedure>, IOutboundEndPoint where TProcedure : Enum
{
    private readonly ILogger<OutboundEndPoint<TProcedure>>? _logger;

    [MayBeUsedByGeneratedCode] protected readonly OutboundEndPointConfiguration Configuration;

    protected OutboundEndPoint
    (
        Messenger messenger,
        int id,
        in OutboundEndPointConfiguration configuration
    )
        : base(messenger, id, configuration.InitialBufferSize, configuration.LoggingName)
    {
        Configuration = configuration;
        _logger = Configuration.LoggerFactory?.CreateLogger<OutboundEndPoint<TProcedure>>();
    }

    protected OutboundEndPoint
    (
        Messenger messenger,
        in OutboundEndPointConfiguration configuration
    )
        : base(messenger, configuration.InitialBufferSize, configuration.LoggingName)
    {
        Configuration = configuration;
        _logger = Configuration.LoggerFactory?.CreateLogger<OutboundEndPoint<TProcedure>>();
    }

    private static RpcRequestException<TProcedure> GetConnectionClosedException(TProcedure procedure)
        => new(procedure, "Connection closed while waiting for the response.");

    private static RpcRequestException<TProcedure> GetConnectionDisposedException(TProcedure procedure)
        => new(procedure, "Connection disposed while waiting for the response.");

    [MayBeUsedByGeneratedCode]
    protected async ValueTask<Response> SendRequestAsync(Request request)
    {
        TProcedure procedure = GetProcedure(request.ProcedureId);

        await Messenger.SendAsync(new Message(request));

        LogSentAnyRequest(procedure, request.Buffer.Count);

        ReceiveResult result = await Messenger.ReceiveAsync(Buffer);

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
            try
            {
                ReceiveResult exceptionTransmissionResult = await Messenger.ReceiveAsync(Buffer);
                switch (exceptionTransmissionResult.ReturnCode)
                {
                    case ReceiveReturnCode.Success:
                        RpcExceptionTransmission exceptionTransmission = RpcExceptionTransmission.Read(exceptionTransmissionResult.Message);
                        switch (exceptionTransmission.RemoteContinuation)
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
            catch (RpcRemoteException<TProcedure> exception)
            {
                LogRemoteRpcException(exception);
                throw;
            }
            catch (Exception e)
            {
                LogExceptionTransmissionException(procedure, e);
                throw;
            }
        }
        catch (RpcRemoteException<TProcedure>)
        {
            throw;
        }
        catch (Exception exception)
        {
            Dispose();
            throw new RpcExceptionTransmissionException(exception);
        }
    }

    [MayBeUsedByGeneratedCode]
    protected Message SendRequest(Request request)
    {
        Messenger.Send(new Message(request));

        TProcedure procedure = GetProcedure(request.ProcedureId);

        LogSentAnyRequest(procedure, request.Buffer.Count);

        ReceiveResult result = Messenger.Receive(Buffer);

        return result.ReturnCode switch
        {
            ReceiveReturnCode.Success => result.Message,
            ReceiveReturnCode.ConnectionClosed => throw GetConnectionClosedException(procedure),
            ReceiveReturnCode.ConnectionDisposed => throw GetConnectionDisposedException(procedure),
            _ => throw new ArgumentOutOfRangeException()
        };
    }

    #region Logging

    private void LogExceptionTransmissionException(TProcedure procedure, Exception exception)
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
                    "{LoggingNameWithId} encountered an exception while receiving an exception transmission for procedure '{ProcedureName}'"
                    + " and as a result this endpoint is being disposed",
                    LoggingNameWithId,
                    GetName(procedure)
                );
            }
        }
    }

    private void LogRemoteRpcException(RpcRemoteException<TProcedure> exception)
    {
        if (_logger != null)
        {
            LogConfiguration configuration = Configuration.LogRemoteRpcException;
            RpcExceptionTransmission transmission = exception.RemoteExceptionTransmission;
            if (_logger.GetIsEnabled(configuration))
            {
                _logger.Log
                (
                    configuration.Level,
                    configuration.Id,
                    exception,
                    "{LoggingNameWithId} had remote endpoint report an exception while handling a request for procedure '{ProcedureName}'"
                    + " with transmitted report '{ExceptionReport}'",
                    LoggingNameWithId,
                    GetName(exception.Procedure),
                    transmission.GetReport()
                );
            }
        }
    }

    private void LogSentAnyRequest(TProcedure procedure, int argumentByteCount)
    {
        if (_logger != null)
        {
            LogConfiguration configuration = Configuration.LogSentAnyRequest;
            if (_logger.GetIsEnabled(configuration))
            {
                _logger.Log
                (
                    configuration.Level,
                    configuration.Id,
                    "{LoggingNameWithId} sent a request to {ProcedureName} with {ArgumentByteCount} argument bytes",
                    LoggingNameWithId,
                    GetName(procedure),
                    argumentByteCount
                );
            }
        }
    }

    #endregion
}