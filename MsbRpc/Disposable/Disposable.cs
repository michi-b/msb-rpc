using System;
using JetBrains.Annotations;

namespace MsbRpc.Disposable;

public abstract class Disposable : IDisposable
{
    [PublicAPI] public bool IsDisposed { get; private set; }

    public void Dispose()
    {
        GC.SuppressFinalize(this);
        Dispose(true);
    }

    ~Disposable()
    {
        Dispose(false);
    }

    private void Dispose(bool disposing)
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

    protected virtual void DisposeUnmanagedResources() { }

    protected virtual void DisposeManagedResources() { }
}