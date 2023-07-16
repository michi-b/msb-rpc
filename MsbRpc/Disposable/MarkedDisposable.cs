#region

using System;
using JetBrains.Annotations;

#endregion

namespace MsbRpc.Disposable;

public abstract class MarkedDisposable : IDisposable
{
    [PublicAPI] public bool IsDisposed { get; private set; }

    public void Dispose()
    {
        GC.SuppressFinalize(this);
        MarDisposedAndDispose(true);
    }

    ~MarkedDisposable()
    {
        MarDisposedAndDispose(false);
    }

    private void MarDisposedAndDispose(bool disposing)
    {
        if (!IsDisposed)
        {
            IsDisposed = true;
            Dispose(disposing);
        }
    }

    protected abstract void Dispose(bool disposing);
}