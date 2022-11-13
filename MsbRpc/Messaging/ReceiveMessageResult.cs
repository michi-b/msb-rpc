namespace MsbRpc.Messaging;

public readonly struct ReceiveMessageResult
{
    public ReceiveMessageResult(ArraySegment<byte> message, ReceiveMessageReturnCode returnCode)
    {
        Message = message;
        ReturnCode = returnCode;
    }

    public ArraySegment<byte> Message { get; }
    public ReceiveMessageReturnCode ReturnCode { get; }
}