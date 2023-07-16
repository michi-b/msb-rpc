#region

using System;
using System.Text;
using JetBrains.Annotations;
using MsbRpc.Contracts;
using MsbRpc.Serialization;
using MsbRpc.Serialization.Buffers;

#endregion

namespace MsbRpc.Exceptions;

public class RpcExceptionTransmission
{
    private readonly RpcExceptionTransmissionOptions _options;
    [PublicAPI] public readonly bool HasExceptionMessage;
    [PublicAPI] public readonly bool HasExceptionTypeName;
    [PublicAPI] public readonly bool HasRemoteContinuation;
    [PublicAPI] public readonly bool HasSourceExecutionStage;
    [PublicAPI] public string ExceptionTypeName { get; private set; }
    [PublicAPI] public RpcExecutionStage SourceExecutionStage { get; private set; }
    [PublicAPI] public string ExceptionMessage { get; private set; }
    [PublicAPI] public RemoteContinuation RemoteContinuation { get; private set; }

    private RpcExceptionTransmission(RpcExceptionTransmissionOptions options)
    {
        _options = options;

        HasExceptionTypeName = options.HasTypeName();
        HasSourceExecutionStage = options.HasExecutionStage();
        HasExceptionMessage = options.HasMessage();
        HasRemoteContinuation = options.HasContinuation();

        ExceptionTypeName = string.Empty;
        SourceExecutionStage = RpcExecutionStage.None;
        ExceptionMessage = string.Empty;
        RemoteContinuation = RemoteContinuation.Undefined;
    }

    public RpcExceptionTransmission
        (Exception exception, RpcExecutionStage sourceExecutionStage, RpcExceptionContinuation continuation, RpcExceptionTransmissionOptions options)
        : this(options)
    {
        if (HasExceptionTypeName)
        {
            ExceptionTypeName = exception.GetType().FullName;
        }

        if (HasExceptionMessage)
        {
            ExceptionMessage = exception.Message;
        }

        if (HasSourceExecutionStage)
        {
            SourceExecutionStage = sourceExecutionStage;
        }

        RemoteContinuation = HasRemoteContinuation ? GetRemoteContinuation(continuation) : RemoteContinuation.Undefined;
    }

    public Message GetMessage(RpcBuffer buffer)
    {
        const int optionsSize = 1;
        int typeNameSize = HasExceptionTypeName ? StringSerializer.GetSize(ExceptionTypeName) : 0;
        int executionStageSize = HasSourceExecutionStage ? 1 : 0;
        int messageSize = HasExceptionMessage ? StringSerializer.GetSize(ExceptionMessage) : 0;
        int continuationSize = HasRemoteContinuation ? 1 : 0;

        int size = optionsSize + typeNameSize + messageSize + executionStageSize + continuationSize;

        Message message = buffer.GetMessage(size);

        BufferWriter writer = message.GetWriter();

        writer.Write((byte)_options);
        if (HasExceptionTypeName)
        {
            writer.Write(ExceptionTypeName);
        }

        if (HasSourceExecutionStage)
        {
            writer.Write((byte)SourceExecutionStage);
        }

        if (HasExceptionMessage)
        {
            writer.Write(ExceptionMessage);
        }

        if (HasRemoteContinuation)
        {
            writer.Write((byte)RemoteContinuation);
        }

        return message;
    }

    public static RpcExceptionTransmission Read(Message message)
    {
        BufferReader reader = message.GetReader();

        var options = (RpcExceptionTransmissionOptions)reader.ReadByte();

        RpcExceptionTransmission transmission = new(options);

        if (transmission.HasExceptionTypeName)
        {
            transmission.ExceptionTypeName = reader.ReadString();
        }

        if (transmission.HasSourceExecutionStage)
        {
            transmission.SourceExecutionStage = (RpcExecutionStage)reader.ReadByte();
        }

        if (transmission.HasExceptionMessage)
        {
            transmission.ExceptionMessage = reader.ReadString();
        }

        if (transmission.HasRemoteContinuation)
        {
            transmission.RemoteContinuation = (RemoteContinuation)reader.ReadByte();
        }

        return transmission;
    }

    public string GetReport()
    {
        StringBuilder stringBuilder = new(200);
        const string notTransmitted = "not transmitted";

        stringBuilder.Append("exception type: ");
        stringBuilder.Append(HasExceptionTypeName ? ExceptionTypeName : notTransmitted);

        stringBuilder.Append(", exception message: ");
        stringBuilder.Append(HasExceptionMessage ? ExceptionMessage : notTransmitted);

        stringBuilder.Append(", source execution stage: ");
        stringBuilder.Append(HasSourceExecutionStage ? SourceExecutionStage.GetName() : notTransmitted);

        stringBuilder.Append(", remote endpoint continuation: ");
        stringBuilder.Append(HasRemoteContinuation ? RemoteContinuation.GetName() : notTransmitted);

        return stringBuilder.ToString();
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