﻿using System;
using System.Threading;
using System.Threading.Tasks;
using JetBrains.Annotations;
using MsbRpc.Serialization.Buffers;
using MsbRpc.Serialization.Primitives;
using MsbRpc.Sockets;

namespace MsbRpc.Messaging;

[PublicAPI]
public class Messenger : IDisposable
{
    private const int CountSize = PrimitiveSerializer.IntSize;
    private static readonly ReceiveResult ClosedConnectionReceiveResult = new(Message.Empty, ReceiveReturnCode.ConnectionClosed);
    private static readonly ReceiveResult DisposedConnectionReceiveResult = new(Message.Empty, ReceiveReturnCode.ConnectionDisposed);
    private static readonly ReceiveResult EmptyReceiveResult = new(Message.Empty, ReceiveReturnCode.Success);
    private readonly RpcSocket _socket;
    public readonly int Port;
    private bool _isDisposed;

    public Messenger(RpcSocket socket)
    {
        _socket = socket;
        Port = socket.Port;
    }

    public void Dispose()
    {
        if (!_isDisposed)
        {
            GC.SuppressFinalize(this);
            _isDisposed = true;
            _socket.Dispose();
        }
    }

    public ListenReturnCode Listen(RpcBuffer buffer, Func<Message, bool> receive)
    {
        while (true)
        {
            ReceiveResult receiveResult = ReceiveMessage(buffer);
            switch (receiveResult.ReturnCode)
            {
                case ReceiveReturnCode.Success:
                    if (receive(receiveResult.Message))
                    {
                        return ListenReturnCode.OperationDiscontinued;
                    }

                    break;
                case ReceiveReturnCode.ConnectionClosed:
                    return ListenReturnCode.ConnectionClosed;
                case ReceiveReturnCode.ConnectionDisposed:
                    return ListenReturnCode.ConnectionDisposed;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }

    public async ValueTask<ListenReturnCode> ListenAsync(RpcBuffer buffer, Func<Message, bool> receive, CancellationToken cancellationToken)
    {
        while (true)
        {
            ReceiveResult receiveResult = await ReceiveMessageAsync(buffer);
            switch (receiveResult.ReturnCode)
            {
                case ReceiveReturnCode.Success:
                    if (receive(receiveResult.Message))
                    {
                        return ListenReturnCode.OperationDiscontinued;
                    }

                    break;
                case ReceiveReturnCode.ConnectionClosed:
                    return ListenReturnCode.ConnectionClosed;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }

    /// <throws>SocketReceiveException</throws>
    public ReceiveResult ReceiveMessage(RpcBuffer buffer)
    {
        try
        {
            if (TryReceiveCount(buffer, out int count))
            {
                Message message = buffer.GetMessage(count);

                if (count == 0)
                {
                    return EmptyReceiveResult;
                }

                _socket.ReceiveAll(message.Buffer);

                return new ReceiveResult(message, ReceiveReturnCode.Success);
            }
        }
        catch (ObjectDisposedException)
        {
            if (_isDisposed)
            {
                return DisposedConnectionReceiveResult;
            }

            throw;
        }

        return ClosedConnectionReceiveResult;
    }

    /// <throws>OperationCanceledException</throws>
    /// <throws>SocketReceiveException</throws>
    [PublicAPI]
    public async ValueTask<ReceiveResult> ReceiveMessageAsync(RpcBuffer buffer)
    {
        try
        {
            ArraySegment<byte> countSegment = buffer.Get(PrimitiveSerializer.IntSize);
            if (await _socket.ReceiveAllAsync(countSegment))
            {
                int count = countSegment.ReadInt();

                if (count == 0)
                {
                    return EmptyReceiveResult;
                }

                Message message = buffer.GetMessage(count);

                await _socket.ReceiveAllAsync(message.Buffer);

                return new ReceiveResult(message, ReceiveReturnCode.Success);
            }
        }
        catch (ObjectDisposedException)
        {
            if (_isDisposed)
            {
                return DisposedConnectionReceiveResult;
            }

            throw;
        }

        return ClosedConnectionReceiveResult;
    }

    /// <throws>OperationCanceledException</throws>
    /// <throws>SocketSendException</throws>
    [PublicAPI]
    public async ValueTask SendAsync(Message message)
    {
        await _socket.SendAsync(message.GetFullMessageBuffer());
    }

    /// <throws>SocketSendException</throws>
    public void Send(Message message)
    {
        _socket.Send(message.GetFullMessageBuffer());
    }

    private ReceiveResult Receive(Message message)
    {
        ArraySegment<byte> buffer = message.Buffer;

        if (buffer.Count == 0)
        {
            return EmptyReceiveResult;
        }

        _socket.ReceiveAll(buffer);

        return new ReceiveResult
        (
            message,
            ReceiveReturnCode.Success
        );
    }

    private async ValueTask<ReceiveResult> ReceiveAsync(Message message)
    {
        ArraySegment<byte> buffer = message.Buffer;

        if (buffer.Count == 0)
        {
            return EmptyReceiveResult;
        }

        await _socket.ReceiveAllAsync(buffer);

        return new ReceiveResult
        (
            message,
            ReceiveReturnCode.Success
        );
    }

    private bool TryReceiveCount(RpcBuffer buffer, out int count)
    {
        ArraySegment<byte> countBuffer = buffer.Get(PrimitiveSerializer.IntSize);
        if (_socket.ReceiveAll(countBuffer))
        {
            count = countBuffer.ReadInt();
            return true;
        }

        count = 0;
        return false;
    }
}