namespace MsbRpc.Generator.GenerationHelpers;

public class RpcEndPointCode
{
    public const string BufferSizeParameterWithDefaultLine = $"int {RpcEndPointNames.BufferSizeParameter} = {RpcEndPointNames.DefaultBufferSizeConstant}";
}