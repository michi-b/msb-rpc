namespace MsbRpc.Messaging.Sockets;

public readonly struct ReceiveMessageResult
{
    public ReceiveMessageResult(byte[] bytes, ReceiveMessageReturnCode messageReturnCode)
    {
        Bytes = bytes;
        MessageReturnCode = messageReturnCode;
    }

    public byte[] Bytes { get; }
    public ReceiveMessageReturnCode MessageReturnCode { get; }
}