using System;

namespace MsbRpc.Exceptions;

[Flags]
public enum RpcExceptionTransmissionOptions : byte
{
    None = 0,
    TypeName = 1 << 0,
    ExecutionStage = 1 << 1,
    Message = 1 << 2,
    Continuation = 1 << 3
}