using JetBrains.Annotations;
using MsbRpc.Exceptions;
using MsbRpc.Utility.Generic;

namespace MsbRpc.Messaging.Listeners;

using StateUtility = StateUtility<ListenerState>;

[PublicAPI]
public abstract class Listener : IDisposable
{
    public enum ReturnCode
    {
        ConnectionClosed = 1,
        ConnectionClosedUnexpectedly = 2,
        OperationCanceled = 3
    }

    private static readonly ListenerState[] DisposableStates = { ListenerState.Initial, ListenerState.Finished, ListenerState.Disposed };
    private static readonly ListenerState[] ExtractableStates = { ListenerState.Initial, ListenerState.Finished };
    private static readonly ListenerState[] MessengerOwningStates = { ListenerState.Initial, ListenerState.Listening, ListenerState.Finished };

    private readonly Action _dispose;
    private readonly Messenger _messenger;
    private readonly AutoResetEvent _stateLock = new(true);
    private Func<int, Task<ArraySegment<byte>>> _allocate;

    private ListenerState _state = ListenerState.Initial;

    protected Listener(Messenger messenger)
    {
        _messenger = messenger;

        _allocate = Allocate;
        _dispose = () => { _messenger.Dispose(); };
    }

    /// <exception cref="InvalidStateException{TEnum}"></exception>
    public void Dispose()
    {
        StateUtility.Transition(ref _state, DisposableStates, ListenerState.Disposed, _stateLock, _dispose);
    }

    /// <exception cref="InvalidStateException{TEnum}"></exception>
    public void ExtractMessenger()
    {
        StateUtility.Transition(ref _state, ExtractableStates, ListenerState.Disposed, _stateLock, _dispose);
    }

    /// <exception cref="ArgumentOutOfRangeException"></exception>
    /// <exception cref="InvalidStateException{TEnum}"></exception>
    /// <param name="cancellationToken">token to abort the whole connection and dispose the listener when canceled</param>
    public async Task<ReturnCode> Listen(CancellationToken cancellationToken)
    {
        //impossible to cancel socket reads without closing the socket ... sad
        cancellationToken.Register(Dispose);

        StateUtility.Transition(ref _state, ListenerState.Initial, ListenerState.Listening, _stateLock);

        while (!cancellationToken.IsCancellationRequested) //listens until the connection is closed by the remote
        {
            ReceiveMessageResult receiveMessageResult = await _messenger.ReceiveMessageAsync(_allocate);
            switch (receiveMessageResult.MessageReturnCode)
            {
                case ReceiveMessageReturnCode.Success:
                    ReceiveMessage(receiveMessageResult.Message);
                    continue;
                case ReceiveMessageReturnCode.ConnectionClosed:
                    StateUtility.Transition(ref _state, ListenerState.Listening, ListenerState.Finished, _stateLock);
                    return ReturnCode.ConnectionClosed;
                case ReceiveMessageReturnCode.ConnectionClosedUnexpectedly:
                    StateUtility.Transition(ref _state, ListenerState.Listening, ListenerState.Finished, _stateLock);
                    return ReturnCode.ConnectionClosedUnexpectedly;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        return ReturnCode.OperationCanceled;
    }

    protected abstract void ReceiveMessage(ArraySegment<byte> message);

    protected abstract Task<ArraySegment<byte>> Allocate(int count);
}