using System;
using System.Threading;
using MsbRpc.Messaging;

namespace MsbRpc.Servers.Listener;

public struct MessengerListenTask
{
    private Semaphore _isCompletedSemaphore;
    private Messenger? _result;

    public MessengerListenTask()
    {
        _isCompletedSemaphore = new Semaphore(0, 1);
        _result = null;
    }

    public Messenger Await(int millisecondsTimeOut = 10000)
    {
        if (!_isCompletedSemaphore.WaitOne(millisecondsTimeOut))
        {
            throw new TimeoutException($"{nameof(MessengerListenTask)}.{nameof(Await)} timed out.");
        }

        if (_result == null)
        {
            throw new NullReferenceException($"{nameof(_result)} is null though the {nameof(MessengerListenTask)}.{nameof(_isCompletedSemaphore)} has been signaled.");
        }

        return _result;
    }

    public void Fullfill(Messenger result)
    {
        if (_result != null)
        {
            throw new InvalidOperationException($"{nameof(MessengerListenTask)}.{nameof(Fullfill)} has been called more than once.");
        }

        _result = result;
        _isCompletedSemaphore.Release();
    }
}