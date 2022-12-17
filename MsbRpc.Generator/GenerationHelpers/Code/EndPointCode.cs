using MsbRpc.EndPoints;
using MsbRpc.Generator.GenerationHelpers.Names;

namespace MsbRpc.Generator.GenerationHelpers.Code;

public static class EndPointCode
{
    public const string BufferSizeParameterWithDefaultLine = $"int {EndPointNames.BufferSizeParameter} = {EndPointNames.DefaultBufferSizeConstant}";
    public static string GetInitialDirectionArgumentLine(EndPointDirection direction) => $"MsbRpc.EndPoints.EndPointDirection.{direction.GetName()},";
}