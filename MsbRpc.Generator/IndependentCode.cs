namespace MsbRpc.Generator;

internal static class IndependentCode
{
    private const string ArgumentsBufferWrite = IndependentNames.Variables.ArgumentsWriter + "." + IndependentNames.Methods.BufferWrite;

    public static string GetBufferWrite(string variableName) => $"{ArgumentsBufferWrite}({variableName});";
}