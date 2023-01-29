using System;

namespace MsbRpc.Contracts;

public static class RpcExecutionStageExtensions
{
    public static string GetName(this RpcExecutionStage target)
    {
        return target switch
        {
            RpcExecutionStage.ArgumentDeserialization => nameof(RpcExecutionStage.ArgumentDeserialization),
            RpcExecutionStage.ImplementationCall => nameof(RpcExecutionStage.ImplementationCall),
            RpcExecutionStage.ResultSerialization => nameof(RpcExecutionStage.ResultSerialization),
            _ => throw new ArgumentOutOfRangeException(nameof(target), target, null)
        };
    }
}