#region

using System;
using System.Diagnostics;
using MsbRpc.Attributes;
using MsbRpc.Disposable;
using MsbRpc.EndPoints.Interfaces;
using MsbRpc.Exceptions;
using MsbRpc.Messaging;
using MsbRpc.Serialization.Buffers;

#endregion

namespace MsbRpc.EndPoints;

[DebuggerDisplay("{DebuggerDisplay,nq}")]
public abstract class EndPoint<TProcedure> : MarkedDisposable, IEndPoint where TProcedure : Enum
{
    public string LoggingName { get; }

    protected Messenger Messenger { get; }
    protected RpcBuffer Buffer { get; }
    public bool RanToCompletion { get; protected set; }

    private string DebuggerDisplay => $"{LoggingName} ({GetType().Name})";

    protected EndPoint(Messenger messenger, RpcBuffer buffer, string loggingName)
    {
        LoggingName = loggingName;
        Messenger = messenger;
        Buffer = buffer;
    }

    protected EndPoint
    (
        Messenger messenger,
        int initialBufferSize,
        string loggingName
    )
    {
        LoggingName = loggingName;

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

            throw new ObjectDisposedException(LoggingName, $"{LoggingName} is disposed");
        }
#if DEBUG
        if (RanToCompletion)
        {
            throw new InvalidOperationException
            (
                $"{LoggingName} has run to completion but is not disposed."
                + " This is supposed to be impossible to happen."
            );
        }
#endif
    }

    protected abstract string GetName(TProcedure procedure);

    protected abstract TProcedure GetProcedure(int procedureId);
}