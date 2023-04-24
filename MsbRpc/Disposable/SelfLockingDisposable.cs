using System;
using System.Threading;
using JetBrains.Annotations;

namespace MsbRpc.Disposable;

public abstract class SelfLockingDisposable : IDisposable
{
    private const int LockTimeOutMilliseconds = 60000; // 1 minute

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

    protected void ExecuteIfNotDisposed(Action action, bool throwObjectDisposedExceptionOtherwise = true, Action? alternativeAction = null)
    {
        void OnDisposed()
        {
            alternativeAction?.Invoke();

            if (throwObjectDisposedExceptionOtherwise)
            {
                throw new ObjectDisposedException(GetType().Name);
            }
        }

        if (IsDisposed)
        {
            OnDisposed();
            return;
        }

        if (Monitor.TryEnter(this, LockTimeOutMilliseconds))
        {
            try
            {
                if (IsDisposed)
                {
                    OnDisposed();
                    return;
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

        if (Monitor.TryEnter(this, LockTimeOutMilliseconds))
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
            if (Monitor.TryEnter(this, LockTimeOutMilliseconds))
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

            DisposeManagedResourcesAfterUnlocking();
        }
    }

    [PublicAPI]
    protected virtual void DisposeUnmanagedResources() { }

    [PublicAPI]
    protected virtual void DisposeManagedResourcesAfterUnlocking() { }

    [PublicAPI]
    protected virtual void DisposeManagedResources() { }
}