using System;
using System.Threading;
using JetBrains.Annotations;

namespace MsbRpc.Disposable;

public abstract class ConcurrentDisposable : IDisposable
{
    private const int LockTimeOutMilliseconds = 60000; // 1 minute

    private readonly object _lock = new();

    [PublicAPI] public bool IsDisposed { get; private set; }

    public void Dispose()
    {
        GC.SuppressFinalize(this);
        Dispose(true);
    }

    ~ConcurrentDisposable()
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

        if (Monitor.TryEnter(_lock, LockTimeOutMilliseconds))
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
                Monitor.Exit(_lock);
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

        if (Monitor.TryEnter(_lock, LockTimeOutMilliseconds))
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
                Monitor.Exit(_lock);
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
        // early exit without locking if this is already disposed
        if (IsDisposed)
        {
            return;
        }

        bool needsDisposal = false;

        lock (_lock)
        {
            if (!IsDisposed)
            {

                IsDisposed = true;
                // after marking this as disposed, actual disposal is delayed until after the lock is released
                // this is ABSOLUTELY NECESSARY in order to avoid deadlocks
                // a deadlock could otherwise occur, if the Dispose() method in turn recursively triggers disposal of this object again from another thread
                // that may not have received the IsDisposed flag yet (non-locking early-exit fails)
                // this might be avoidable with memory barriers as well, but be sure to extensively test this race condition when refactoring this
                needsDisposal = true;
            }
        }

        if (needsDisposal)
        {
            if (disposing)
            {
                DisposeManagedResources();
            }

            DisposeUnmanagedResources();
        }
    }

    [PublicAPI]
    protected virtual void DisposeUnmanagedResources() { }

    [PublicAPI]
    protected virtual void DisposeManagedResources() { }
}