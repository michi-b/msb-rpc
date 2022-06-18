namespace MsbRpc.Messaging;

public readonly struct ReceiveMessageResult
{
    public ReceiveMessageResult(int length, ReceiveMessageReturnCode messageReturnCode)
    {
        Length = length;
        MessageReturnCode = messageReturnCode;
    }

    public int Length { get; }
    public ReceiveMessageReturnCode MessageReturnCode { get; }
}