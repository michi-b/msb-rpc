namespace MsbRpc.Generator;

public class IndependentCode
{
    private const string ArgumentsBufferWrite = IndependentNames.Variables.ArgumentsWriter + "." + IndependentNames.Methods.BufferWrite;

    public static string GetBufferWrite(string variableName) => $"{ArgumentsBufferWrite}({variableName});";
}