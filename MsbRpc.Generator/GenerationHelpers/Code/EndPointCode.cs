using MsbRpc.Generator.GenerationHelpers.Names;

namespace MsbRpc.Generator.GenerationHelpers.Code;

public static class EndPointCode
{
    public const string BufferSizeParameterWithDefaultLine = $"int {EndPointNames.BufferSizeParameter} = {EndPointNames.DefaultBufferSizeConstant}";
}