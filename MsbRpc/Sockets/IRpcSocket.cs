using System;
using System.Threading;
using System.Threading.Tasks;

namespace MsbRpc.Sockets;

public interface IRpcSocket : IDisposable
{
    /// <summary>
    ///     Sends all bytes of the given array segment.
    /// </summary>
    /// <exception cref="Exceptions.RpcSocketSendException">if none or not all bytes were sent</exception>
    /// <exception cref="OperationCanceledException"></exception>
    ValueTask SendAsync(ArraySegment<byte> bytes, CancellationToken cancellationToken);

    /// <summary>
    ///     Receives exactly the bytes to fill the given array segment.
    /// </summary>
    /// <returns>whether all the bytes were received</returns>
    /// <exception cref="Exceptions.RpcSocketReceiveException">
    ///     if more than zero bytes but less than specified via the array segment were received
    /// </exception>
    /// <exception cref="OperationCanceledException"></exception>
    ValueTask<bool> ReceiveAllAsync(ArraySegment<byte> buffer, CancellationToken cancellationToken);
}