using System.Collections.Concurrent;
using JetBrains.Annotations;
using MsbRpc.Messaging.Messenger;
using MsbRpc.Utility.Generic;

namespace MsbRpc.Messaging.SocketOwners;

using StateUtility = EnumUtility<Listener.State>;

[PublicAPI]
public class Listener : MessengerOwner, IDisposable
{
    public enum ReturnCode
    {
        ConnectionClosed = 1,
        ConnectionClosedUnexpectedly = 2
    }

    private static readonly State[] DisposableStates = { State.Finished, State.Disposed };
    private readonly BlockingCollection<byte[]> _available = new(new ConcurrentQueue<byte[]>());
    private readonly AutoResetEvent _stateLock = new(true);
    private State _state = State.Initial;

    public Listener(Messenger.Messenger messenger, int capacity) : base(messenger) { }

    /// <exception cref="InvalidStateException{TEnum}"></exception>
    public void Dispose()
    {
        StateUtility.Transition(ref _state, DisposableStates, State.Disposed, _stateLock);
        _available.Dispose();
        DisposeSocket();
    }

    /// <exception cref="ArgumentOutOfRangeException"></exception>
    /// <exception cref="InvalidStateException{TEnum}"></exception>
    protected async Task<ReturnCode> Listen()
    {
        StateUtility.Transition(ref _state, State.Initial, State.Listening, _stateLock);

        while (true) //listens until the connection is closed by the remote
        {
            ReceiveMessageResult receiveMessageResult = await Messenger.ReceiveMessageAsync();
            switch (receiveMessageResult.MessageReturnCode)
            {
                case ReceiveMessageReturnCode.Success:
                    _available.Add(receiveMessageResult.Bytes);
                    continue;
                case ReceiveMessageReturnCode.ConnectionClosed:
                    StateUtility.Transition(ref _state, State.Listening, State.Finished, _stateLock);
                    return ReturnCode.ConnectionClosed;
                case ReceiveMessageReturnCode.ConnectionClosedUnexpectedly:
                    StateUtility.Transition(ref _state, State.Listening, State.Finished, _stateLock);
                    return ReturnCode.ConnectionClosedUnexpectedly;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }

    internal enum State
    {
        Initial,
        Listening,
        Finished,
        Disposed
    }
}