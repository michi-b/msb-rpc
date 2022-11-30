using System.Diagnostics;
using System.Runtime.CompilerServices;
using JetBrains.Annotations;
using MsbRpc.Messaging;
using MsbRpc.Serialization.Buffers;
using MsbRpc.Serialization.Primitives;

namespace MsbRpc.EndPoints;

public abstract class RpcEndPoint<TInboundProcedure, TOutboundProcedure> : IDisposable
    where TInboundProcedure : Enum
    where TOutboundProcedure : Enum
{
    [PublicAPI] public const int DefaultBufferSize = BufferUtility.DefaultSize;

    private readonly RecycledBuffer _buffer;

    private readonly Messenger _messenger;
    private State _state;

    [PublicAPI] public State State => _state;

    protected RpcEndPoint(Messenger messenger, Direction direction, int bufferSize = DefaultBufferSize)
    {
        _messenger = messenger;
        _buffer = new RecycledBuffer(bufferSize);
        _state = direction switch
        {
            Direction.Inbound => State.IdleInbound,
            Direction.Outbound => State.IdleOutbound,
            _ => throw new ArgumentOutOfRangeException(nameof(direction), direction, null)
        };
    }

    public async Task<Messenger.ListenReturnCode> ListenAsync(CancellationToken cancellationToken)
    {
        _state.Transition(State.IdleInbound, State.ListeningForRequests);
        Messenger.ListenReturnCode listenReturnCode = await _messenger.ListenAsync(_buffer, ReceiveMessageAsync, cancellationToken);

        // receive message callback will only discontinue listening if procedure results in outbound state
        if (listenReturnCode == Messenger.ListenReturnCode.OperationDiscontinued)
        {
            _state.Transition(State.ListeningForRequests, State.IdleOutbound);
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

    /// <summary>
    ///     Get a buffer with specified size from endpoint memory for sending an rpc response.
    ///     Do not use this in a rpc request handling method before you finished reading the arguments,
    ///     as they are placed in the same buffer.
    /// </summary>
    /// <param name="size">number of bytes the segment is required to contain</param>
    /// <returns>the requested buffer that points into the endpoint memory</returns>
    protected ArraySegment<byte> GetResponseMemory(int size)
    {
        _state.AssertIsResponding();
        return _buffer.Get(size);
    }

    /// <summary>
    ///     Get a buffer with specified size from endpoint memory for sending an rpc request.
    ///     Is offset so <see cref="SendRequest" /> can prefix it with the procedure id.
    /// </summary>
    /// <param name="size">number of bytes the segment is required to contain</param>
    /// <returns>the requested buffer that points into the endpoint memory</returns>
    protected ArraySegment<byte> GetRequestMemory(int size)
    {
        _state.AssertIsRequesting();
        ArraySegment<byte> buffer = _buffer.Get(size + PrimitiveSerializer.Int32Size);
        return buffer.GetOffsetSubSegment(PrimitiveSerializer.Int32Size);
    }

    /// <summary>
    ///     Sends an rpc request.
    /// </summary>
    /// <param name="procedure">Which procedure is to be invoked remotely.</param>
    /// <param name="request">Bytes of the request, formerly retrieved via <see cref="GetRequestMemory" />.</param>
    /// <param name="cancellationToken">Token for operation cancellation.</param>
    /// <returns>the response message</returns>
    protected async Task<ArraySegment<byte>> SendRequest
        (TOutboundProcedure procedure, ArraySegment<byte> request, CancellationToken cancellationToken)
    {
        //get request memory makes sure to leave space for the procedure id in front of the buffer
        request = new ArraySegment<byte>
        (
            request.Array!,
            request.Offset - PrimitiveSerializer.Int32Size,
            request.Count + PrimitiveSerializer.Int32Size
        );

        request.WriteInt32(GetProcedureIdValue(procedure));

        await _messenger.SendMessageAsync(request, cancellationToken);

        ReceiveMessageResult result = await _messenger.ReceiveMessageAsync(_buffer, cancellationToken);

        return result.ReturnCode switch
        {
            ReceiveMessageReturnCode.Success => result.Message,
            ReceiveMessageReturnCode.ConnectionClosed => throw new RpcRequestException<TOutboundProcedure>
                (procedure, "connection closed while waiting for the response"),
            _ => throw new ArgumentOutOfRangeException()
        };
    }

    protected void EnterSending()
    {
        _state.Transition(State.IdleOutbound, State.SendingRequest);
    }

    protected void ExitSending()
    {
        _state.Transition(State.SendingRequest, State.IdleOutbound);
    }

    private async Task<bool> ReceiveMessageAsync(ArraySegment<byte> message, CancellationToken cancellationToken)
    {
        int procedureIdValue = message.ReadInt32();

        TInboundProcedure procedure = GetInboundProcedure(procedureIdValue);

        ArraySegment<byte> response = HandleRequest(procedure, message.GetOffsetSubSegment(PrimitiveSerializer.Int32Size));

        await _messenger.SendMessageAsync(response, cancellationToken);

        return GetDirectionAfterRequest(procedure) == Direction.Outbound;
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

    /// <param name="procedure">id of the procedure to call</param>
    /// <param name="argumentsBuffer">bytes of the arguments in recycled memory for the procedure to call</param>
    /// <returns>bytes of the return value of the called procedure, may be in recycled memory as well</returns>
    protected abstract ArraySegment<byte> HandleRequest(TInboundProcedure procedure, ArraySegment<byte> argumentsBuffer);

    protected abstract Direction GetDirectionAfterRequest(TInboundProcedure procedure);
}