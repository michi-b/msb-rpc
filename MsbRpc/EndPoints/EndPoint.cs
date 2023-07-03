using System;
using System.Diagnostics;
using MsbRpc.Attributes;
using MsbRpc.Disposable;
using MsbRpc.EndPoints.Interfaces;
using MsbRpc.Exceptions;
using MsbRpc.Messaging;
using MsbRpc.Serialization.Buffers;

namespace MsbRpc.EndPoints;

[DebuggerDisplay("{DebuggerDisplay,nq}")]
public abstract class EndPoint<TProcedure> : MarkedDisposable, IEndPoint where TProcedure : Enum
{
    protected readonly string LoggingNameWithId;
    public int Id { get; }

    public string Name { get; }

    protected Messenger Messenger { get; }
    protected RpcBuffer Buffer { get; }
    public bool RanToCompletion { get; protected set; }

    private string DebuggerDisplay => $"{LoggingNameWithId} ({GetType().Name})";

    protected EndPoint(Messenger messenger, RpcBuffer buffer, string loggingName)
    {
        Name = loggingName;
        LoggingNameWithId = loggingName;
        Id = -1;

        Messenger = messenger;
        Buffer = buffer;
    }

    protected EndPoint
    (
        Messenger messenger,
        int id,
        int initialBufferSize,
        string loggingName
    )
    {
        Name = loggingName;
        LoggingNameWithId = $"{loggingName}[{id}]";
        Id = id;

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