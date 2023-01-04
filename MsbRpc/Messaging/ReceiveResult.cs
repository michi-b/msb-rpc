using MsbRpc.Serialization.Buffers;

namespace MsbRpc.Messaging;

public readonly struct ReceiveResult
{
    public ReceiveResult(Message message, ReceiveReturnCode returnCode)
    {
        Message = message;
        ReturnCode = returnCode;
    }

    public Message Message { get; }
    public ReceiveReturnCode ReturnCode { get; }
}