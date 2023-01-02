using MsbRpc.Serialization.Buffers;

namespace MsbRpc.Messaging;

public readonly struct ReceiveMessageResult
{
    public ReceiveMessageResult(Message message, ReceiveMessageReturnCode returnCode)
    {
        Message = message;
        ReturnCode = returnCode;
    }
    public Message Message { get; }
    public ReceiveMessageReturnCode ReturnCode { get; }
}