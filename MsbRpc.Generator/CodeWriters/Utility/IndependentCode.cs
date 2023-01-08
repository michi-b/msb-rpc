using MsbRpc.Generator.GenerationTree;
using static MsbRpc.Generator.CodeWriters.Utility.IndependentNames;

namespace MsbRpc.Generator.CodeWriters.Utility;

internal static class IndependentCode
{
    public const string ReturnProcedureSwitch = $"return {Parameters.Procedure} switch";

    public const string MessengerParameter = $"{Types.Messenger} {Parameters.Messenger}";

    public const string LoggerFactoryNullableParameter = $"{Types.LoggerFactoryInterface}? {Parameters.LoggerFactory} = null";

    public const string RequestParameter = $"{Types.Request} {Parameters.Request}";

    public const string ReturnEmptyResponseLine = $"return {Fields.EndPointBuffer}.{Methods.GetResponse}"
                                                  + $"({Fields.InboundEndpointImplementation}.{Properties.RanToCompletion});";

    public const string GetResultSizeResponseLine = $"{Types.Response} {Variables.Response} = {Fields.EndPointBuffer}.{Methods.GetResponse}("
                                                    + $"{Fields.InboundEndpointImplementation}.{Properties.RanToCompletion}, "
                                                    + $"{Variables.ResultSize});";

    public const string GetResponseWriterLine = $"{Types.BufferWriter} {Variables.ResponseWriter} = {Variables.Response}.{Methods.GetWriter}();";

    public const string WriteResultToResponseLine = $"{Variables.ResponseWriter}.{Methods.BufferWrite}({Variables.Result});";

    public const string ReturnResponseLine = $"return {Variables.Response};";

    public const string IPAddressParameter = $"{Types.IPAddress} {Parameters.IPAddress}";
    public const string PortParameter = $"int {Parameters.Port}";

    /// <remarks>NO trailing comma</remarks>
    /// >
    public const string InitialBufferSizeParameterLine = $"int {Parameters.InitialBufferSize} = {Constants.EndPointDefaultInitialBufferSize}";

    public static readonly string ProcedureParameterOutOfRangeSwitchExpressionCase
        = GetArgumentOutOfRangeSwitchExpressionCase(Parameters.Procedure);

    public static string GetCreateLoggerArgumentLine(string category)
        => $"{StaticMethods.CreateLoggerOptional}<{category}>({Parameters.LoggerFactory})";

    public static string GetLoggerParameterLine(string category) => $"{Types.LoggerInterface}<{category}> {Parameters.Logger}";

    public static string GetArgumentOutOfRangeSwitchExpressionCase(string variableName)
        => $"_ => throw new {Types.ArgumentOutOfRangeException}(nameof({variableName}), {variableName}, null)";

    public static string GetInboundProcedureMethodSignature(ProcedureNode procedure)
        => procedure.HasParameters
            ? $"{Types.Response} {procedure.Name}({RequestParameter})"
            : $"{Types.Response} {procedure.Name}()";
}