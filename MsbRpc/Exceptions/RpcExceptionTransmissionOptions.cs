using System;

namespace MsbRpc.Exceptions;

[Flags]
public enum RpcExceptionTransmissionOptions : byte
{
    None = 0,
    ExceptionTypeName = 1 << 0,
    SourceExecutionStage = 1 << 1,
    ExceptionMessage = 1 << 2,
    RemoteContinuation = 1 << 3,
    AllExceptionDetails = ExceptionTypeName |  ExceptionMessage,
    All = AllExceptionDetails | SourceExecutionStage | RemoteContinuation
}