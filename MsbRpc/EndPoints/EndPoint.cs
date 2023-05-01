using System;
using Microsoft.Extensions.Logging;
using MsbRpc.Attributes;
using MsbRpc.Configuration;
using MsbRpc.Exceptions;
using MsbRpc.Messaging;
using MsbRpc.Serialization.Buffers;

namespace MsbRpc.EndPoints;

public abstract class EndPoint<TProcedure> : Disposable.Disposable where TProcedure : Enum
{
    protected Messenger Messenger { get; }
    protected RpcBuffer Buffer { get; }
    protected ILogger<EndPoint<TProcedure>>? Logger { get; }

    public bool RanToCompletion { get; protected set; }

    protected EndPoint
    (
        Messenger messenger,
        EndPointConfiguration configuration
    )
    {
        Messenger = messenger;
        Logger = configuration.LoggerFactory?.CreateLogger<EndPoint<TProcedure>>();
        Buffer = new RpcBuffer(configuration.InitialBufferSize);
    }

    protected override void DisposeManagedResources()
    {
        Messenger.Dispose();
        base.DisposeManagedResources();
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