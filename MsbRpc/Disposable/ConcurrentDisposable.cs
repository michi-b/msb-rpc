using System;
using JetBrains.Annotations;

namespace MsbRpc.Disposable;

/// <summary>
///     Base implementation of <see cref="IDisposable" /> that ensures that <see cref="Dispose" /> is only called
///     once even if called from multiple threads by guarding an "IsDisposed" flag with a lock.
///     Also provides a method to execute a function if not disposed, and ensures it is not disposed during execution.
/// </summary>
public abstract class ConcurrentDisposable : IDisposable
{
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

    /// <summary>
    ///     executes provided function if this object is not disposed, and ensures it is not disposed during execution
    /// </summary>
    /// <param name="action">action to execute if not disposes</param>
    /// <param name="throwObjectDisposedExceptionOtherwise">whether an exception should be thrown if disposed</param>
    /// <param name="alternativeAction">optional alternative action to execute if disposed</param>
    /// <exception cref="ObjectDisposedException"></exception>
    /// <remarks>
    ///     there is still some danker in this, because any trigger for disposing this object may still occur while performing
    ///     the action, but actual disposal will be delayed until after the action returned
    /// </remarks>
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

        // early exit without locking if this is already disposed on this thread
        if (IsDisposed)
        {
            OnDisposed();
            return;
        }

        lock (_lock)
        {
            if (IsDisposed)
            {
                OnDisposed();
                return;
            }

            action();
        }
    }

    /// <summary>
    ///     executes provided function if this object is not disposed, and ensures it is not disposed during execution
    /// </summary>
    /// <exception cref="ObjectDisposedException"></exception>
    /// <remarks>
    ///     there is still some danger in this, because any trigger for disposing this object may still occur while performing
    ///     the action, but actual disposal will be delayed until after the action returned
    /// </remarks>
    protected T ExecuteIfNotDisposed<T>(Func<T> func)
    {
        // early exit without locking if this is already disposed on this thread
        if (IsDisposed)
        {
            throw new ObjectDisposedException(GetType().Name);
        }

        lock (_lock)
        {
            if (IsDisposed)
            {
                throw new ObjectDisposedException(GetType().Name);
            }

            return func();
        }
    }

    private void Dispose(bool disposing)
    {
        // early exit without locking if this is already disposed on this thread
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