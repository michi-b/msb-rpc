using static MsbRpc.Generator.CodeWriters.Utility.IndependentNames;

namespace MsbRpc.Generator.CodeWriters.Utility;

internal static class IndependentCode
{
    public const string ReturnProcedureParameterSwitchExpressionLine = $"return {Parameters.Procedure} switch";

    public const string MessengerParameterLine = $"{Types.Messenger} {Parameters.Messenger}";

    public const string NullableLoggerFactoryParameterLine = $"{Types.LoggerFactoryInterface}? {Parameters.LoggerFactory} = null";

    /// <remarks>NO trailing comma</remarks>
    /// >
    public const string InitialBufferSizeParameterLine = $"int {Parameters.InitialBufferSize} = {Constants.EndPointDefaultInitialBufferSize}";

    public static readonly string ProcedureParameterOutOfRangeSwitchExpressionCase
        = GetArgumentOutOfRangeSwitchExpressionCase(Parameters.Procedure);

    public static string GetCreateLoggerArgumentLine(string category)
        => $"{StaticMethods.CreateLoggerOptional}<{category}>({Parameters.LoggerFactory})";

    public static string GetCreateLoggerParameterLine(string category) => $"{Types.LoggerInterface}<{category}> {Parameters.Logger}";

    public static string GetArgumentOutOfRangeSwitchExpressionCase(string variableName)
        => $"_ => throw new {Types.ArgumentOutOfRangeException}(nameof({variableName}), {variableName}, null)";
}