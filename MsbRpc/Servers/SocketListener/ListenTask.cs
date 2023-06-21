using System;
using System.Net.Sockets;
using System.Threading;

namespace MsbRpc.Servers.SocketListener;

public class ListenTask
{
    private Semaphore _isCompletedSemaphore = new(0, 1);
    private Socket? _result;

    public Socket Await(int millisecondsTimeOut = 10000)
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

    public void Complete(Socket result)
    {
        if (_result != null)
        {
            throw new InvalidOperationException($"{nameof(ListenTask)}.{nameof(Complete)} has been called more than once.");
        }

        _result = result;
        _isCompletedSemaphore.Release();
    }
}