using System;
using JetBrains.Annotations;
using MsbRpc.Contracts;
using MsbRpc.Serialization;
using MsbRpc.Serialization.Buffers;

namespace MsbRpc.EndPoints;

public class RpcExceptionTransmission
{
    private readonly RpcExceptionTransmissionOptions _options;
    [PublicAPI] public readonly bool HasTypeName;
    [PublicAPI] public readonly bool HasExecutionStage;
    [PublicAPI] public readonly bool HasMessage;
    [PublicAPI] public string TypeName { get; private set; }
    [PublicAPI] public RpcExecutionStage ExecutionStage { get; private set; }
    [PublicAPI] public string Message { get; private set; }

    private RpcExceptionTransmission(RpcExceptionTransmissionOptions options)
    {
        _options = options;
        HasTypeName = (options & RpcExceptionTransmissionOptions.TypeName) != 0;
        HasExecutionStage = (options & RpcExceptionTransmissionOptions.ExecutionStage) != 0;
        HasMessage = (options & RpcExceptionTransmissionOptions.Message) != 0;
        TypeName = string.Empty;
        ExecutionStage = RpcExecutionStage.None;
        Message = string.Empty;
    }
    
    public RpcExceptionTransmission(Exception exception, RpcExecutionStage executionStage, RpcExceptionTransmissionOptions options)
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
    }

    public Message GetMessage(RpcBuffer buffer)
    {
        const int optionsSize = 1;
        int typeNameSize = HasTypeName ? StringSerializer.GetSize(TypeName) : 0;
        int executionStageSize = HasExecutionStage ? 1 : 0;
        int messageSize = HasMessage ? StringSerializer.GetSize(Message) : 0;
        
        int size = optionsSize + typeNameSize + messageSize + executionStageSize;
        
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
        
        return transmission;
    }
}