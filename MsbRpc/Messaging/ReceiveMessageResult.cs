namespace MsbRpc.Messaging;

public readonly struct ReceiveMessageResult
{
    public ReceiveMessageResult(int count, ReceiveMessageReturnCode messageReturnCode)
    {
        Count = count;
        MessageReturnCode = messageReturnCode;
    }

    public int Count { get; }
    public ReceiveMessageReturnCode MessageReturnCode { get; }
}