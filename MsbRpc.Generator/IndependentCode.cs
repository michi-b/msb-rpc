using static MsbRpc.Generator.IndependentNames;

namespace MsbRpc.Generator;

internal static class IndependentCode
{
    private const string ArgumentsBufferWrite = Variables.ArgumentsWriter + "." + Methods.BufferWrite;
    public const string EmptyBufferExpression = Types.BufferUtility + "." + Fields.BufferEmpty;

    public static string GetBufferWrite(string variableName) => $"{ArgumentsBufferWrite}({variableName});";
}