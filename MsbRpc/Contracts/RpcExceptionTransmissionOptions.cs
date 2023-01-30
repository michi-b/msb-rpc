using System;

namespace MsbRpc.Contracts;

[Flags]
public enum RpcExceptionTransmissionOptions : byte
{
    None = 0,
    TypeName = 1 << 0,
    ExecutionStage = 1 << 1,
    Message = 1 << 2
}