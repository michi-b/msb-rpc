using System.Collections.Concurrent;
using MsbRpc.Utility;

namespace MsbRpc.Messaging;

public class Listener : IDisposable
{
    private SocketWrapper _socket;
    private readonly BlockingCollection<byte[]> _available = new(new ConcurrentQueue<byte[]>());
    private AutoResetEvent _stateLock = new(true);
    private State _state;

    private enum State
    {
        Initial,
        Listening,
        Finished,
        Disposed
    }
    
    public Listener(SocketWrapper socket)
    {
        _socket = socket;
    }
    
    /// <exception cref="ArgumentOutOfRangeException"></exception>
    protected async Task<ListenReturnCode> Listen()
    {
        _stateLock.WaitOne();
        try
        {
            EnumUtility<State>.Transition(ref _state, State.Initial, State.Listening);
        }
        finally
        {
            _stateLock.Set();
        }
        
        while (true) //listens until the connection is closed by the remote
        {
            ReceiveMessageResult receiveMessageResult = await _socket.ReceiveMessageAsync();
            switch (receiveMessageResult.MessageReturnCode)
            {
                case ReceiveMessageReturnCode.Success:
                    _available.Enqueue(receiveMessageResult.Bytes);
                    _receivedCount.Release();
                    continue;
                case ReceiveMessageReturnCode.ConnectionClosed:
                    _listeningFinished.Set();
                    return ListenReturnCode.ConnectionClosed;
                case ReceiveMessageReturnCode.ConnectionClosedUnexpectedly:
                    _listeningFinished.Set();
                    return ListenReturnCode.ConnectionClosedUnexpectedly;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        throw new AlreadyListeningException();
    }

    public void Dispose()
    {
        _socket.Dispose();
        _available.Dispose();
    }
}