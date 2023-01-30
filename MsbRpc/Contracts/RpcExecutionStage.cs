namespace MsbRpc.Contracts;

public enum RpcExecutionStage : byte
{
    None,
    ArgumentDeserialization,
    ImplementationCall,
    ResultSerialization
}