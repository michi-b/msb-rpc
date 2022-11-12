namespace MsbRpc.Messaging.Listeners;

public class ActiveListener : Listener
{
    public const int DefaultBufferSize = 1024;
    private readonly AutoResetEvent _canReceiveNextMessage;
    private readonly Action<ArraySegment<byte>> _receiveMessage;
    private byte[] _buffer;

    /// <param name="messenger">the messenger used by the listener to receive messages</param>
    /// <param name="receiveMessage">
    ///     The action that deals with a received message, usually always in the same recycle byte
    ///     buffer.
    /// </param>
    /// <param name="canReceiveNextMessage">
    ///     An event that is always awaited before reading the next message into the buffer
    ///     that is passed in each call to _receiveMessage.
    /// </param>
    /// <param name="bufferSize">initial size of the message buffer, </param>
    public ActiveListener
    (
        Messenger messenger,
        Action<ArraySegment<byte>> receiveMessage,
        AutoResetEvent canReceiveNextMessage,
        int bufferSize = DefaultBufferSize
    ) : base(messenger)
    {
        _receiveMessage = receiveMessage;
        _canReceiveNextMessage = canReceiveNextMessage;
        _buffer = new byte[bufferSize];
    }

    protected override void ReceiveMessage(ArraySegment<byte> message)
    {
        _receiveMessage.Invoke(message);
    }

    protected override byte[] Allocate(int count)
    {
        _canReceiveNextMessage.WaitOne();

        if (_buffer.Length < count)
        {
            _buffer = new byte[count];
        }

        return _buffer;
    }
}