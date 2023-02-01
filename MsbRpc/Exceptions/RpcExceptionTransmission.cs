using System;
using JetBrains.Annotations;
using MsbRpc.Contracts;
using MsbRpc.Serialization;
using MsbRpc.Serialization.Buffers;

namespace MsbRpc.Exceptions;

public class RpcExceptionTransmission
{
    private readonly RpcExceptionTransmissionOptions _options;
    [PublicAPI] public readonly bool HasContinuation;
    [PublicAPI] public readonly bool HasExecutionStage;
    [PublicAPI] public readonly bool HasMessage;
    [PublicAPI] public readonly bool HasTypeName;
    [PublicAPI] public string TypeName { get; private set; }
    [PublicAPI] public RpcExecutionStage ExecutionStage { get; private set; }
    [PublicAPI] public string Message { get; private set; }
    [PublicAPI] public RemoteContinuation Continuation { get; private set; }

    private RpcExceptionTransmission(RpcExceptionTransmissionOptions options)
    {
        _options = options;

        HasTypeName = options.HasTypeName();
        HasExecutionStage = options.HasExecutionStage();
        HasMessage = options.HasMessage();
        HasContinuation = options.HasContinuation();

        TypeName = string.Empty;
        ExecutionStage = RpcExecutionStage.None;
        Message = string.Empty;
        Continuation = RemoteContinuation.Undefined;
    }

    public RpcExceptionTransmission(Exception exception, RpcExecutionStage executionStage, RpcExceptionContinuation continuation, RpcExceptionTransmissionOptions options)
        : this(options)
    {
        if (HasTypeName)
        {
            TypeName = exception.GetType().FullName;
        }

        if (HasMessage)
        {
            Message = exception.Message;
        }

        if (HasExecutionStage)
        {
            ExecutionStage = executionStage;
        }

        Continuation = HasContinuation ? GetRemoteContinuation(continuation) : RemoteContinuation.Undefined;
    }

    public Message GetMessage(RpcBuffer buffer)
    {
        const int optionsSize = 1;
        int typeNameSize = HasTypeName ? StringSerializer.GetSize(TypeName) : 0;
        int executionStageSize = HasExecutionStage ? 1 : 0;
        int messageSize = HasMessage ? StringSerializer.GetSize(Message) : 0;
        int continuationSize = HasContinuation ? 1 : 0;

        int size = optionsSize + typeNameSize + messageSize + executionStageSize + continuationSize;

        Message message = buffer.GetMessage(size);

        BufferWriter writer = message.GetWriter();

        writer.Write((byte)_options);
        if (HasTypeName)
        {
            StringSerializer.Write(TypeName, writer);
        }

        if (HasExecutionStage)
        {
            writer.Write((byte)ExecutionStage);
        }

        if (HasMessage)
        {
            StringSerializer.Write(Message, writer);
        }

        if (HasContinuation)
        {
            writer.Write((byte)Continuation);
        }

        return message;
    }

    public static RpcExceptionTransmission Read(Message message)
    {
        BufferReader reader = message.GetReader();

        var options = (RpcExceptionTransmissionOptions)reader.ReadByte();

        RpcExceptionTransmission transmission = new(options);

        if (transmission.HasTypeName)
        {
            transmission.TypeName = StringSerializer.Read(reader);
        }

        if (transmission.HasExecutionStage)
        {
            transmission.ExecutionStage = (RpcExecutionStage)reader.ReadByte();
        }

        if (transmission.HasMessage)
        {
            transmission.Message = StringSerializer.Read(reader);
        }

        if (transmission.HasContinuation)
        {
            transmission.Continuation = (RemoteContinuation)reader.ReadByte();
        }

        return transmission;
    }

    private static RemoteContinuation GetRemoteContinuation(RpcExceptionContinuation continuation)
    {
        return continuation switch
        {
            RpcExceptionContinuation.Dispose => RemoteContinuation.Disposed,
            RpcExceptionContinuation.MarkRanToCompletion => RemoteContinuation.RanToCompletion,
            RpcExceptionContinuation.Continue => RemoteContinuation.Continues,
            _ => throw new ArgumentOutOfRangeException(nameof(continuation), continuation, null)
        };
    }
}