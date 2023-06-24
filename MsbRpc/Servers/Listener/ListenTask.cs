using System;
using System.Threading;
using MsbRpc.Messaging;

namespace MsbRpc.Servers.Listener;

public struct ListenTask
{
    private Semaphore _isCompletedSemaphore;
    private Messenger? _result;

    public ListenTask()
    {
        _isCompletedSemaphore = new Semaphore(0, 1);
        _result = null;
    }

    public Messenger Await(int millisecondsTimeOut = 10000)
    {
        if (!_isCompletedSemaphore.WaitOne(millisecondsTimeOut))
        {
            throw new TimeoutException($"{nameof(ListenTask)}.{nameof(Await)} timed out.");
        }

        if (_result == null)
        {
            throw new NullReferenceException($"{nameof(_result)} is null though the {nameof(ListenTask)}.{nameof(_isCompletedSemaphore)} has been signaled.");
        }

        return _result;
    }

    public void Fullfill(Messenger result)
    {
        if (_result != null)
        {
            throw new InvalidOperationException($"{nameof(ListenTask)}.{nameof(Fullfill)} has been called more than once.");
        }

        _result = result;
        _isCompletedSemaphore.Release();
    }
}