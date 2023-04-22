using System;
using JetBrains.Annotations;

namespace MsbRpc.Disposable;

public abstract class SelfLockingDisposable : IDisposable
{
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

    private void Dispose(bool disposing)
    {
        if (!IsDisposed)
        {
            lock (this)
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
        }
    }

    [PublicAPI]
    protected virtual void DisposeUnmanagedResources() { }

    protected virtual void DisposeManagedResources() { }
}