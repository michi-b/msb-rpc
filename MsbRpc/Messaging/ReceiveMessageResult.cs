namespace MsbRpc.Messaging;

public readonly struct ReceiveMessageResult
{
    public ReceiveMessageResult(ArraySegment<byte> message, ReceiveMessageReturnCode messageReturnCode)
    {
        Message = message;
        MessageReturnCode = messageReturnCode;
    }

    public ArraySegment<byte> Message { get; }
    public ReceiveMessageReturnCode MessageReturnCode { get; }
}