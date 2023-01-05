using System;
using Microsoft.Extensions.Logging;
using MsbRpc.Messaging;
using MsbRpc.Serialization.Buffers;

namespace MsbRpc.EndPoints;

public abstract class EndPoint<TEndPoint, TProcedure> : IDisposable
    where TEndPoint : EndPoint<TEndPoint, TProcedure>
    where TProcedure : Enum
{
    protected const int DefaultInitialBufferSize = 1024;
    protected Messenger Messenger { get; }
    protected RpcBuffer Buffer { get; }
    protected ILogger<TEndPoint> Logger { get; }
    private bool IsDisposed { get; set; }

    protected EndPoint
    (
        Messenger messenger,
        // ReSharper disable once ContextualLoggerProblem
        ILogger<TEndPoint> logger,
        int initialBufferSize = BufferUtility.DefaultInitialSize
    )
    {
        Messenger = messenger;
        Logger = logger;
        Buffer = new RpcBuffer(initialBufferSize);
    }

    public virtual void Dispose()
    {
        if (!IsDisposed)
        {
            GC.SuppressFinalize(this);
            Messenger.Dispose();
            IsDisposed = true;
        }
    }

    protected abstract string GetName(TProcedure procedure);

    protected abstract TProcedure GetProcedure(int procedureId);
}