using System.Diagnostics;

namespace MsbRpc.Messaging.Listeners;

public class ActiveListener : AListener
{
    public const int DefaultBufferSize = 1024;
    private readonly Action<ArraySegment<byte>> _receiveMessage;
    private byte[] _buffer;
    private readonly AutoResetEvent _mayReuseBuffer;

    public ActiveListener(Messenger messenger, Action<ArraySegment<byte>> receiveMessage, int bufferSize = DefaultBufferSize) : base(messenger)
    {
        _receiveMessage = receiveMessage;
        _buffer = new byte[bufferSize];
        _mayReuseBuffer = new AutoResetEvent(true);
    }

    protected override void ReceiveMessage(ArraySegment<byte> message)
    {
        Debug.Assert(!CanReuseBuffer);
        _receiveMessage.Invoke(message);
        _mayReuseBuffer.Set();
    }

    private bool CanReuseBuffer
    {
        get
        {
            bool canReuseBuffer = _mayReuseBuffer.WaitOne(0);
            if (canReuseBuffer)
            {
                _mayReuseBuffer.Set();
            }
            return canReuseBuffer;
        }
    }

    protected override Task<ArraySegment<byte>> Allocate(int count)
    {
        _mayReuseBuffer.WaitOne();
        
        if (_buffer.Length < count)
        {
            _buffer = new byte[count];
        }

        return Task.FromResult(new ArraySegment<byte>(_buffer, 0, count));
    }
}