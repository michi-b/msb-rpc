namespace MsbRpc.Generator.GenerationHelpers;

public class RpcEndPointCode
{
    public const string BufferSizeParameterWithDefaultLine = $"int {EndPointNames.BufferSizeParameter} = {EndPointNames.DefaultBufferSizeConstant}";
}