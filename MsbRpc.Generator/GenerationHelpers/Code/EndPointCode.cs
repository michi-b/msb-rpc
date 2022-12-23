using MsbRpc.EndPoints;
using MsbRpc.Generator.GenerationHelpers.ReusedNames;

namespace MsbRpc.Generator.GenerationHelpers.Code;

public static class EndPointCode
{
    public const string BufferSizeParameterWithDefaultLine =
        $"int {EndPointNames.Parameters.BufferSize} = {EndPointNames.Fields.DefaultBufferSizeConstant}";

    public static string GetInitialDirectionArgumentLine(EndPointDirection direction) => $"MsbRpc.EndPoints.EndPointDirection.{direction.GetName()},";

    public static string GetRequestWriterCodeLine(string variableName, string sizeArgument)
        => $"{ProcedureNames.Types.BufferWriter} {variableName} = {IndependentNames.Methods.GetEndPointRequestWriter}({sizeArgument});";
}