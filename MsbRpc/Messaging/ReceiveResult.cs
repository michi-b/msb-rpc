namespace MsbRpc.Messaging;

public readonly struct ReceiveResult
{
    public ReceiveResult(int count, ReceiveReturnCode returnCode)
    {
        Count = count;
        ReturnCode = returnCode;
    }

    public int Count { get; }
    public ReceiveReturnCode ReturnCode { get; }
}