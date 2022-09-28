namespace MsbRpc.Messaging.Messenger;

public readonly struct ReceiveMessageResult
{
    public ReceiveMessageResult(ArraySegment<byte> bytes, ReceiveMessageReturnCode messageReturnCode)
    {
        Bytes = bytes;
        MessageReturnCode = messageReturnCode;
    }

    public byte[] Bytes { get; }
    public ReceiveMessageReturnCode MessageReturnCode { get; }
}