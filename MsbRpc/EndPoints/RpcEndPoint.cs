using System.Diagnostics;
using System.Runtime.CompilerServices;
using JetBrains.Annotations;
using Microsoft.Extensions.Logging;
using MsbRpc.Messaging;
using MsbRpc.Serialization.Buffers;
using MsbRpc.Serialization.Primitives;

namespace MsbRpc.EndPoints;

public abstract partial class RpcEndPoint<TInboundProcedure, TOutboundProcedure> : IDisposable
    where TInboundProcedure : Enum
    where TOutboundProcedure : Enum
{
    protected const int DefaultBufferSize = BufferUtility.DefaultSize;
    private readonly RecycledBuffer _buffer;

    private readonly Messenger _messenger;
    private readonly string _typeName;

    [PublicAPI] protected readonly ILogger<RpcEndPoint<TInboundProcedure, TOutboundProcedure>> Logger;

    private State _state;

    [PublicAPI] public State State => _state;

    protected RpcEndPoint
    (
        Messenger messenger,
        Direction direction,
        ILogger<RpcEndPoint<TInboundProcedure, TOutboundProcedure>> logger,
        int bufferSize = DefaultBufferSize
    )
    {
        _typeName = GetType().Name;
        Logger = logger;
        _messenger = messenger;
        _buffer = new RecycledBuffer(bufferSize);
        _state = direction switch
        {
            Direction.Inbound => State.IdleInbound,
            Direction.Outbound => State.IdleOutbound,
            _ => throw new ArgumentOutOfRangeException(nameof(direction), direction, null)
        };
    }

    public async ValueTask<Messenger.ListenReturnCode> ListenAsync(CancellationToken cancellationToken)
    {
        _state.Transition(State.IdleInbound, State.Listening);
        Messenger.ListenReturnCode listenReturnCode = await _messenger.ListenAsync
            (_buffer, (message, cancellationToken1) => ReceiveMessageAsync(message, cancellationToken1), cancellationToken);

        // receive message callback will only discontinue listening if procedure results in outbound state
        if (listenReturnCode == Messenger.ListenReturnCode.OperationDiscontinued)
        {
            _state.Transition(State.Listening, State.IdleOutbound);
        }
        // otherwise, connection has got to be lost
        else
        {
            Dispose();
        }

        return listenReturnCode;
    }

    public void Dispose()
    {
        _messenger.Dispose();
        _state = State.Disposed;
    }

    /// <returns>whether listening should stop</returns>
    private async ValueTask<bool> ReceiveMessageAsync(ArraySegment<byte> message, CancellationToken cancellationToken)
    {
        int procedureIdValue = message.ReadInt();

        TInboundProcedure procedure = GetInboundProcedure(procedureIdValue);

        ArraySegment<byte> arguments = message.GetOffsetSubSegment(PrimitiveSerializer.Int32Size);

        LogReceivedCall(_typeName, GetName(procedure), arguments.Count);

        BufferWriter responseWriter = HandleRequest(procedure, new BufferReader(arguments));

        await _messenger.SendMessageAsync(responseWriter.Buffer, cancellationToken);

        bool invertsDirection = GetInvertsDirection(procedure);
        if (invertsDirection)
        {
            _state = State.IdleOutbound;
        }

        return invertsDirection;
    }

    /// <summary>
    ///     Get a buffer with specified size from endpoint memory for sending an rpc response.
    ///     Do not use this in a rpc request handling method before you finished reading the arguments,
    ///     as they are placed in the same buffer.
    /// </summary>
    /// <param name="size">number of bytes the segment is required to contain</param>
    /// <returns>the requested buffer that points into the endpoint memory</returns>
    protected BufferWriter GetResponseWriter(int size)
    {
        _state.AssertIsResponding();
        return new BufferWriter(_buffer.Get(size));
    }

    /// <summary>
    ///     Get a buffer with specified size from endpoint memory for sending an rpc request.
    ///     Is offset so <see cref="SendRequest" /> can prefix it with the procedure id.
    /// </summary>
    /// <param name="size">number of bytes the segment is required to contain</param>
    /// <returns>the requested buffer that points into the endpoint memory</returns>
    protected BufferWriter GetRequestWriter(int size)
    {
        _state.AssertIsRequesting();
        ArraySegment<byte> buffer = _buffer.Get(size + PrimitiveSerializer.Int32Size);
        return new BufferWriter(buffer, PrimitiveSerializer.Int32Size);
    }

    /// <summary>
    ///     Sends an rpc request.
    /// </summary>
    /// <param name="procedure">Which procedure is to be invoked remotely.</param>
    /// <param name="request">Bytes of the request, formerly retrieved via <see cref="GetRequestWriter" />.</param>
    /// <param name="cancellationToken">Token for operation cancellation.</param>
    /// <returns>the response message</returns>
    protected async ValueTask<BufferReader> SendRequest(TOutboundProcedure procedure, ArraySegment<byte> request, CancellationToken cancellationToken)
    {
        int argumentByteCount = request.Count;

        //GetRequestWriter makes sure to leave space for the procedure id in front of the buffer
        request = new ArraySegment<byte>
        (
            request.Array!,
            0,
            argumentByteCount + PrimitiveSerializer.Int32Size
        );

        request.WriteInt(GetProcedureIdValue(procedure));

        await _messenger.SendMessageAsync(request, cancellationToken);

        LogSentCall(_typeName, GetName(procedure), argumentByteCount);

        ReceiveMessageResult result = await _messenger.ReceiveMessageAsync(_buffer, cancellationToken);

        return result.ReturnCode switch
        {
            ReceiveMessageReturnCode.Success => new BufferReader(result.Message),
            ReceiveMessageReturnCode.ConnectionClosed => throw new RpcRequestException<TOutboundProcedure>
                (procedure, "connection closed while waiting for the response"),
            _ => throw new ArgumentOutOfRangeException()
        };
    }

    protected void EnterCalling()
    {
        _state.Transition(State.IdleOutbound, State.Calling);
    }

    protected void ExitCalling(TOutboundProcedure procedure)
    {
        _state.Transition
        (
            State.Calling,
            GetInvertsDirection(procedure) ? State.IdleInbound : State.IdleOutbound
        );
    }

    private static TInboundProcedure GetInboundProcedure(int id)
    {
        Debug.Assert(Enum.GetUnderlyingType(typeof(TInboundProcedure)) == typeof(int));
        return Unsafe.As<int, TInboundProcedure>(ref id);
    }

    private static int GetProcedureIdValue(TOutboundProcedure value)
    {
        Debug.Assert(Enum.GetUnderlyingType(typeof(TOutboundProcedure)) == typeof(int));
        return Unsafe.As<TOutboundProcedure, int>(ref value);
    }

    protected virtual string GetName(TInboundProcedure procedure) => throw CreateUndefinedProcedureException();

    protected virtual string GetName(TOutboundProcedure procedure) => throw CreateUndefinedProcedureException();

    protected virtual bool GetInvertsDirection(TInboundProcedure procedure) => throw CreateUndefinedProcedureException();

    protected virtual bool GetInvertsDirection(TOutboundProcedure procedure) => throw CreateUndefinedProcedureException();

    /// <param name="procedure">id of the procedure to call</param>
    /// <param name="argumentsBuffer">bytes of the arguments in recycled memory for the procedure to call</param>
    /// <returns>bytes of the return value of the called procedure, may be in recycled memory as well</returns>
    protected virtual BufferWriter HandleRequest(TInboundProcedure procedure, BufferReader argumentsBuffer)
        => throw CreateUndefinedProcedureException();

    protected UndefinedProcedureException<TInboundProcedure, TOutboundProcedure> CreateUndefinedProcedureException
        ([CallerMemberName] string? callerMemberName = null)
        => new(this, callerMemberName);
}