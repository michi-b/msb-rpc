using System;

namespace MsbRpc.Contracts;

public static class RpcExecutionStageExtensions
{
    public static string GetName(this RpcExecutionStage target)
    {
        return target switch
        {
            RpcExecutionStage.ArgumentDeserialization => nameof(RpcExecutionStage.ArgumentDeserialization),
            RpcExecutionStage.ImplementationExecution => nameof(RpcExecutionStage.ImplementationExecution),
            RpcExecutionStage.ResponseSerialization => nameof(RpcExecutionStage.ResponseSerialization),
            _ => throw new ArgumentOutOfRangeException(nameof(target), target, null)
        };
    }
}