namespace MsbRpc.Contracts;

public enum RpcExecutionStage
{
    ArgumentDeserialization,
    ImplementationCall,
    ResultSerialization
}