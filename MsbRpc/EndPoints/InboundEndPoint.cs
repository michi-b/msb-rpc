using System;
using JetBrains.Annotations;
using Microsoft.Extensions.Logging;
using MsbRpc.Messaging;
using MsbRpc.Serialization.Buffers;
using MsbRpc.Serialization.Primitives;
using MsbRpc.Utility;

namespace MsbRpc.EndPoints;

[PublicAPI(Messages.ForUseInGeneratedCode)]
public abstract partial class InboundEndPoint<TProcedure, TImplementation> : OneWayEndPoint<TProcedure> where TProcedure : Enum
{
    protected readonly TImplementation Implementation;
    
    protected InboundEndPoint
    (
        Messenger messenger,
        TImplementation implementation,
        ILogger logger,
        int initialBufferSize = BufferUtility.DefaultInitialSize
    )
        : base
        (
            messenger,
            logger,
            initialBufferSize
        )
        => Implementation = implementation;

    public Messenger.ListenReturnCode Listen()
    {
        return Messenger.Listen(Buffer, message => ReceiveMessage(message));
    }

    private bool ReceiveMessage(ArraySegment<byte> message)
    {
        int procedureIdValue = message.ReadInt();
        TProcedure procedure = GetProcedure(procedureIdValue);
        ArraySegment<byte> arguments = message.GetOffsetSubSegment(PrimitiveSerializer.IntSize);
        LogReceivedCall(Logger, TypeName, GetName(procedure), arguments.Count);
        ArraySegment<byte> response = Execute(procedure, new BufferReader(arguments));
        Messenger.SendMessage(response);
        return GetStopsListening(procedure);
    }

    protected BufferWriter GetResultWriter(int size) => new BufferWriter(Buffer.Get(size));

    [LoggerMessage
    (
        EventId = (int)LogEventIds.RpcEndPointReceivedCall,
        Level = LogLevel.Trace,
        Message = "{endPointTypeName} received a call to {procedureName} with {argumentsByteCount} argument bytes"
    )]
    partial void LogReceivedCall(ILogger logger, string endPointTypeName, string procedureName, int argumentsByteCount);

    protected abstract ArraySegment<byte> Execute(TProcedure procedure, BufferReader arguments);
}