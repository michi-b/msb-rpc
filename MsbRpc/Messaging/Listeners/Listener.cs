using JetBrains.Annotations;
using MsbRpc.Utility.Generic;

namespace MsbRpc.Messaging.Listeners;

using StateUtility = StateUtility<Listener.State>;

[PublicAPI]
public abstract class Listener : IDisposable
{
    public enum ReturnCode
    {
        ConnectionClosed = 1,
        ConnectionClosedUnexpectedly = 2,
        OperationCanceled = 3
    }

    private static readonly State[] DisposableStates = { State.Initial, State.Finished, State.Disposed };
    private static readonly State[] ExtractableStates = { State.Initial, State.Finished };

    private readonly Action _dispose;

    private readonly Messenger _messenger;

    private readonly AutoResetEvent _stateLock = new(true);
    private Func<int, Task<byte[]>> _allocate;

    private State _state = State.Initial;

    protected Listener(Messenger messenger)
    {
        _messenger = messenger;

        _allocate = Allocate;

        _dispose = () => { _messenger.Dispose(); };
    }

    /// <exception cref="InvalidStateException{TEnum}"></exception>
    public void Dispose()
    {
        StateUtility.Transition(ref _state, DisposableStates, State.Disposed, _stateLock, _dispose);
    }

    /// <exception cref="InvalidStateException{TEnum}"></exception>
    public void ExtractMessenger()
    {
        StateUtility.Transition(ref _state, ExtractableStates, State.Disposed, _stateLock, _dispose);
    }

    /// <exception cref="ArgumentOutOfRangeException"></exception>
    /// <exception cref="InvalidStateException{TEnum}"></exception>
    public async Task<ReturnCode> Listen(CancellationToken cancellationToken)
    {
        //impossible to cancel socket reads without closing the socket ... sad
        cancellationToken.Register(Dispose);

        StateUtility.Transition(ref _state, State.Initial, State.Listening, _stateLock);

        while (!cancellationToken.IsCancellationRequested) //listens until the connection is closed by the remote
        {
            ReceiveMessageResult receiveMessageResult = await _messenger.ReceiveMessageAsync(_allocate);
            switch (receiveMessageResult.MessageReturnCode)
            {
                case ReceiveMessageReturnCode.Success:
                    ReceiveMessage(receiveMessageResult.Message);
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

        return ReturnCode.OperationCanceled;
    }

    protected abstract void ReceiveMessage(ArraySegment<byte> message);

    protected abstract Task<byte[]> Allocate(int count);

    internal enum State
    {
        Initial,
        Listening,
        Finished,
        Disposed
    }
}