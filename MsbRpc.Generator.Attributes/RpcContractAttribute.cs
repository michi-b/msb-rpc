#region

using System;

#endregion

namespace MsbRpc.Generator.Attributes;

[GeneratorMarker]
[AttributeUsage(AttributeTargets.Interface)]
public class RpcContractAttribute : Attribute
{
    [GeneratorTarget] public readonly int DefaultInitialBufferSize;
    [GeneratorTarget] public readonly RpcDirection Direction;

    /// <summary>
    ///     Marks an interface to be an RPC contract and have it's RPC endpoints etc. generated.
    ///     Note that the interface must be note be public or internal (which is hardly useful) and not be nested in another
    ///     type, only in a namespace
    ///     Also, the interface must derive from MsbRpc.Contracts.IRpcContract,
    ///     and if any of these preconditions are not met, it will be ignored by the contract code generator.
    /// </summary>
    /// <param name="direction">
    ///     The direction of the contract, indicating which endpoint is the sender and which is the receiver of RPC
    ///     invocations.
    /// </param>
    /// <param name="defaultInitialBufferSize">
    ///     The initial size of byte buffers used for serialization and deserialization of RPC invocations.
    ///     Should be set large enough to encompass serialization of any expected RPC call or response, including parameters or
    ///     return values and RPC headers.
    ///     If the buffer is too small, it will be resized on demand, causing a memory allocation and possibly memory
    ///     fragmentation.
    /// </param>
    public RpcContractAttribute
    (
        RpcDirection direction = RpcDirection.ClientToServer,
        int defaultInitialBufferSize = 1024
    )
    {
        Direction = direction;
        DefaultInitialBufferSize = defaultInitialBufferSize;
    }
}