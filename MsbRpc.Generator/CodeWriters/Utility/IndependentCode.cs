using MsbRpc.Generator.GenerationTree;
using static MsbRpc.Generator.CodeWriters.Utility.IndependentNames;

namespace MsbRpc.Generator.CodeWriters.Utility;

internal static class IndependentCode
{
    public const string ReturnProcedureSwitch = $"return {Parameters.Procedure} switch";

    public const string MessengerParameter = $"{Types.Messenger} {Parameters.Messenger}";

    public const string NullableLoggerFactoryParameter = $"{Types.LoggerFactoryInterface}? {Parameters.LoggerFactory} = null";

    public const string RequestParameter = $"{Types.Response} {Parameters.Request}";

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

    public static string GetInboundProcedureMethodSignature(ProcedureNode procedure)
        => procedure.HasParameters
            ? $"{Types.Response} {procedure.Name}({RequestParameter})"
            : $"{Types.Response} {procedure.Name}()";
}