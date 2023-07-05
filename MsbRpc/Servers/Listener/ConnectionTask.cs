using System;
using System.Diagnostics;
using System.Threading;
using MsbRpc.Messaging;

namespace MsbRpc.Servers.Listener.ConnectionTask;

public class ConnectionTask
{
    public const int DefaultMillisecondsTimeout = 10000;
    private Messenger? _result;
    private readonly ManualResetEventSlim _completed;

    public ConnectionTask()
    {
        _completed = new ManualResetEventSlim(false);
        _result = null;
    }

    public Messenger Await(int millisecondsTimeOut = DefaultMillisecondsTimeout)
    {
        if (!_completed.Wait(millisecondsTimeOut))
        {
            throw new TimeoutException($"{nameof(ConnectionTask)}.{nameof(Await)} timed out.");
        }

        if (_result == null)
        {
            throw new NullReferenceException($"{nameof(_result)} is null though the {nameof(ConnectionTask)}.{nameof(_completed)} has been signaled.");
        }

        _completed.Dispose();
        return _result;
    }

    public void Complete(Messenger result)
    {
        Debug.Assert(!_completed.IsSet);
        _result = result;
        _completed.Set();
    }
}