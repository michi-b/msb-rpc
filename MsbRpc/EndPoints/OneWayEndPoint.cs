using System;
using Microsoft.Extensions.Logging;
using MsbRpc.Messaging;
using MsbRpc.Serialization.Buffers;

namespace MsbRpc.EndPoints;

public abstract class OneWayEndPoint<TProcedure> : IDisposable where TProcedure : Enum
{
    protected Messenger Messenger { get; }
    protected RecycledBuffer Buffer { get; }
    protected ILogger Logger { get; }
    protected string TypeName { get; }
    private bool IsDisposed { get; set; }

    protected const int DefaultInitialBufferSize = 1024;
    
    protected OneWayEndPoint
    (
        Messenger messenger,
        ILogger logger,
        int initialBufferSize = BufferUtility.DefaultInitialSize
    )
    {
        Messenger = messenger;
        Logger = logger;
        Buffer = new RecycledBuffer(initialBufferSize);
        TypeName = GetType().Name;
    }

    public void Dispose()
    {
        if (!IsDisposed)
        {
            GC.SuppressFinalize(this);
            Messenger.Dispose();
            IsDisposed = true;
        }
    }
    
    protected static ILogger CreateLogger<TEndPoint>(ILoggerFactory? loggerFactory) => loggerFactory != null
        ? loggerFactory.CreateLogger<TEndPoint>()
        : new Microsoft.Extensions.Logging.Abstractions.NullLogger<TEndPoint>();

    protected abstract TProcedure GetProcedure(int procedureId);
    protected abstract string GetName(TProcedure procedure);
    protected abstract bool GetStopsListening(TProcedure procedure);
}