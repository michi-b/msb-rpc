using System;
using System.Threading;
using JetBrains.Annotations;

namespace MsbRpc.Disposable;

public abstract class SelfLockingDisposable : IDisposable
{
    private readonly int _lockTimeOutMilliseconds = 10000;
    [PublicAPI] public bool IsDisposed { get; private set; }

    public void Dispose()
    {
        GC.SuppressFinalize(this);
        Dispose(true);
    }

    ~SelfLockingDisposable()
    {
        Dispose(false);
    }

    protected void ExecuteIfNotDisposed(Action action, bool throwObjectDisposedExceptionOtherwise = true)
    {
        if (IsDisposed)
        {
            if (!throwObjectDisposedExceptionOtherwise)
            {
                return;
            }

            throw new ObjectDisposedException(GetType().Name);
        }

        if (Monitor.TryEnter(this, _lockTimeOutMilliseconds))
        {
            try
            {
                if (IsDisposed)
                {
                    if (!throwObjectDisposedExceptionOtherwise)
                    {
                        return;
                    }

                    throw new ObjectDisposedException(GetType().Name);
                }

                action();
            }
            finally
            {
                Monitor.Exit(this);
            }
        }
        else
        {
            throw GetTimeoutException();
        }
    }

    protected T ExecuteIfNotDisposed<T>(Func<T> func)
    {
        if (IsDisposed)
        {
            throw new ObjectDisposedException(GetType().Name);
        }

        if (Monitor.TryEnter(this, _lockTimeOutMilliseconds))
        {
            try
            {
                if (IsDisposed)
                {
                    throw new ObjectDisposedException(GetType().Name);
                }

                return func();
            }
            finally
            {
                Monitor.Exit(this);
            }
        }
        // ReSharper disable once RedundantIfElseBlock
        // I like this 'else'
        else
        {
            throw GetTimeoutException();
        }
    }

    private static TimeoutException GetTimeoutException() => new();

    private void Dispose(bool disposing)
    {
        if (!IsDisposed)
        {
            if (Monitor.TryEnter(this, _lockTimeOutMilliseconds))
            {
                try
                {
                    if (!IsDisposed)
                    {
                        IsDisposed = true;
                        if (disposing)
                        {
                            DisposeManagedResources();
                        }

                        DisposeUnmanagedResources();
                    }
                }
                finally
                {
                    Monitor.Exit(this);
                }
            }
            else
            {
                throw GetTimeoutException();
            }
        }
    }

    [PublicAPI]
    protected virtual void DisposeUnmanagedResources() { }

    protected virtual void DisposeManagedResources() { }
}