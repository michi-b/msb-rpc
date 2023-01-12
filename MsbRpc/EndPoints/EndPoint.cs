﻿using System;
using JetBrains.Annotations;
using Microsoft.Extensions.Logging;
using MsbRpc.EndPoints.Configuration;
using MsbRpc.Messaging;
using MsbRpc.Serialization.Buffers;
using MsbRpc.Utility;

namespace MsbRpc.EndPoints;

public abstract class EndPoint<TEndPoint, TProcedure> : IDisposable
    where TEndPoint : EndPoint<TEndPoint, TProcedure>
    where TProcedure : Enum
{
    protected Messenger Messenger { get; }
    protected RpcBuffer Buffer { get; }
    protected ILogger<TEndPoint>? Logger { get; }

    [PublicAPI] public bool IsDisposed { get; private set; }

    public bool RanToCompletion { get; protected set; }

    protected EndPoint
    (
        Messenger messenger,
        EndPointConfiguration configuration
    )
    {
        Messenger = messenger;
        Logger = configuration.LoggerFactory?.CreateLogger<TEndPoint>();
        Buffer = new RpcBuffer(configuration.InitialBufferSize);
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

    [PublicAPI(Messages.ForUseInGeneratedCode)]
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