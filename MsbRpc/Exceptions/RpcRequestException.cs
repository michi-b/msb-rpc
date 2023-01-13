using System;
using JetBrains.Annotations;

namespace MsbRpc.Exceptions;

public class RpcRequestException<TProcedureId> : Exception where TProcedureId : Enum
{
    [PublicAPI] public TProcedureId ProcedureId { get; }

    public RpcRequestException(TProcedureId procedureId, string message)
        : base($"Error while executing procedure {procedureId}: {message}")
        => ProcedureId = procedureId;
}