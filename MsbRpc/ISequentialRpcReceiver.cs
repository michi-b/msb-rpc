using MsbRpc.Serialization.Buffers;

namespace MsbRpc;

public interface ISequentialRpcReceiver
{
    /// <param name="procedureId">id of the procedure to call</param>
    /// <param name="arguments">bytes of the arguments fore the procedure to call</param>
    /// <param name="argumentsBuffer">
    ///     recycled buffer that is used both for the arguments and the return value,
    ///     therefore do not read arguments after writing to it
    /// </param>
    /// <returns>bytes of the return value of the called procedure</returns>
    ArraySegment<byte> Receive(int procedureId, ArraySegment<byte> arguments, RecycledBuffer argumentsBuffer);
}