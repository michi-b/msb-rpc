using System;
using MsbRpc.Attributes;
using MsbRpc.Disposable;
using MsbRpc.Exceptions;
using MsbRpc.Messaging;
using MsbRpc.Serialization.Buffers;

namespace MsbRpc.EndPoints;

public abstract class EndPoint<TProcedure> : MarkedDisposable where TProcedure : Enum
{
    protected Messenger Messenger { get; }
    protected RpcBuffer Buffer { get; }
    public bool RanToCompletion { get; protected set; }

    protected EndPoint
    (
        Messenger messenger,
        int initialBufferSize
    )
    {
        Messenger = messenger;
        Buffer = new RpcBuffer(initialBufferSize);
    }

    protected override void Dispose(bool disposing)
    {
        if (disposing)
        {
            Messenger.Dispose();
        }
    }

    [MayBeUsedByGeneratedCode]
    protected void AssertIsOperable()
    {
        if (IsDisposed)
        {
            if (RanToCompletion)
            {
                throw new EndPointRanToCompletionException(GetType().Name);
            }

            throw new ObjectDisposedException(GetType().Name, $"endpoint {GetType().Name} is disposed");
        }
#if DEBUG
        if (RanToCompletion)
        {
            throw new InvalidOperationException
            (
                $"Endpoint {GetType().Name} has run to completion but is not disposed."
                + " This is supposed to be impossible to happen."
            );
        }
#endif
    }

    protected abstract string GetName(TProcedure procedure);

    protected abstract TProcedure GetProcedure(int procedureId);
}