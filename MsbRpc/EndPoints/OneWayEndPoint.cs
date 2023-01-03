using System;
using Microsoft.Extensions.Logging;
using MsbRpc.Messaging;
using MsbRpc.Serialization.Buffers;

namespace MsbRpc.EndPoints;

public abstract class OneWayEndPoint<TProcedure> : IDisposable where TProcedure : Enum
{
    protected Messenger Messenger { get; }
    protected RpcBuffer Buffer { get; }
    protected ILogger Logger { get; }
    protected string TypeName { get; }
    public int Port { get; }
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
        Buffer = new RpcBuffer(initialBufferSize);
        TypeName = GetType().Name;
        Port = Messenger.Port;
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
    
    protected abstract int GetId(TProcedure procedure);
    
    protected abstract bool GetClosesCommunication(TProcedure procedure);
}