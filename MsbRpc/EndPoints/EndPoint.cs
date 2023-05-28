using System;
using JetBrains.Annotations;
using MsbRpc.Attributes;
using MsbRpc.Disposable;
using MsbRpc.Exceptions;
using MsbRpc.Messaging;
using MsbRpc.Serialization.Buffers;

namespace MsbRpc.EndPoints;

public abstract class EndPoint<TProcedure> : MarkedDisposable where TProcedure : Enum
{
    [PublicAPI] public readonly int Id;

    protected readonly string LoggingNameWithId;

    protected Messenger Messenger { get; }
    protected RpcBuffer Buffer { get; }
    public bool RanToCompletion { get; protected set; }

    protected EndPoint(Messenger messenger, int initialBufferSize, string loggingName)
    {
        Messenger = messenger;
        Buffer = new RpcBuffer(initialBufferSize);
        LoggingNameWithId = loggingName;
        Id = -1;
    }

    protected EndPoint
    (
        Messenger messenger,
        int id,
        int initialBufferSize,
        string loggingName
    )
    {
        Messenger = messenger;
        Buffer = new RpcBuffer(initialBufferSize);
        LoggingNameWithId = $"{loggingName}[{id}]";
        Id = id;
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

            throw new ObjectDisposedException(LoggingNameWithId, $"{LoggingNameWithId} with id is disposed");
        }
#if DEBUG
        if (RanToCompletion)
        {
            throw new InvalidOperationException
            (
                $"{LoggingNameWithId} has run to completion but is not disposed."
                + " This is supposed to be impossible to happen."
            );
        }
#endif
    }

    protected abstract string GetName(TProcedure procedure);

    protected abstract TProcedure GetProcedure(int procedureId);
}