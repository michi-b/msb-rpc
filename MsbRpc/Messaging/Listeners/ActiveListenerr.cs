namespace MsbRpc.Messaging.Listeners;

public class ActiveListener : AListener
{
    public const int DefaultBufferSize = 1024;
    private readonly Action<ArraySegment<byte>> _receiveMessage;
    private byte[] _buffer;

    public ActiveListener(Messenger messenger, Action<ArraySegment<byte>> receiveMessage, int bufferSize = DefaultBufferSize) : base(messenger)
    {
        _receiveMessage = receiveMessage;
        _buffer = new byte[bufferSize];
    }

    protected override void ReceiveMessage(ArraySegment<byte> message)
    {
        _receiveMessage.Invoke(message);
    }

    protected override Task<ArraySegment<byte>> Allocate(int count)
    {
        if (_buffer.Length < count)
        {
            _buffer = new byte[count];
        }

        return Task.FromResult(new ArraySegment<byte>(_buffer, 0, count));
    }
}