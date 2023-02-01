namespace MsbRpc.Contracts;

public enum RpcExecutionStage : byte
{
    None,
    ArgumentDeserialization,
    ImplementationExecution,
    ResponseSerialization
}