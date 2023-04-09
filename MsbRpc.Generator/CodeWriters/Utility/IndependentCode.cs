﻿using MsbRpc.Generator.GenerationTree;
using static MsbRpc.Generator.CodeWriters.Utility.IndependentNames;

namespace MsbRpc.Generator.CodeWriters.Utility;

internal static class IndependentCode
{
    public const string ReturnProcedureSwitch = $"return {Parameters.Procedure} switch";

    public const string MessengerParameter = $"{Types.Messenger} {Parameters.Messenger}";

    public const string LoggerFactoryNullableParameter = $"{Types.LoggerFactoryInterface}? {Parameters.LoggerFactory} = null";

    public const string RequestParameter = $"{Types.Request} {Parameters.Request}";

    public const string ReturnEmptyResponseStatement = $"return {Fields.EndPointBuffer}.{Methods.GetResponse}"
                                                       + $"({Fields.InboundEndpointImplementation}.{Properties.RanToCompletion});";

    public const string GetResultSizeResponseStatement = $"{Types.Response} {Variables.Response} = {Fields.EndPointBuffer}.{Methods.GetResponse}("
                                                         + $"{Fields.InboundEndpointImplementation}.{Properties.RanToCompletion}, "
                                                         + $"{Variables.ResultSize});";

    public const string GetResponseWriterStatement = $"{Types.BufferWriter} {Variables.ResponseWriter} = {Variables.Response}.{Methods.GetWriter}();";

    public const string ReturnResponseStatement = $"return {Variables.Response};";

    public const string SendRequestStatement = $"await base.{Methods.SendRequestAsync}({Variables.Request});";

    public const string SendRequestStatementWithResponse = $"{Types.Response} {Variables.Response} = {SendRequestStatement}";

    public const string ResponseReaderInitializationStatement =
        $"{Types.BufferReader} {Variables.ResponseReader} = {Variables.Response}.{Methods.GetReader}();";

    public const string RequestInitializationWithoutParameters
        = $"{Types.Request} {Variables.Request} = base.{Fields.EndPointBuffer}.{Methods.GetRequest}";

    public const string RequestWriterInitializationStatement =
        $"{Types.BufferWriter} {Variables.RequestWriter} = {Variables.Request}.{Methods.GetWriter}();";

    public const string IPEndPointParameter = $"{Types.IPEndPoint} {Parameters.IPEndPoint}";

    public const string LocalConfigurationParameter = $"{Types.LocalConfiguration} {Parameters.Configuration}";

    public const string ConfigureLocalConfigurationActionParameter =
        $"{Types.Action}<{Types.LocalConfiguration}>? {Parameters.ConfigureAction} = null";

    public static readonly string ProcedureParameterOutOfRangeSwitchExpressionCase
        = GetArgumentOutOfRangeSwitchExpressionCase(Parameters.Procedure);

    public static string GetArgumentOutOfRangeSwitchExpressionCase
        (string variableName)
        => $"_ => throw new {Types.ArgumentOutOfRangeException}(nameof({variableName}), {variableName}, null)";

    public static string GetInboundProcedureMethodSignature(ProcedureNode procedure)
        => procedure.HasParameters
            ? $"{Types.Response} {procedure.Name}({RequestParameter})"
            : $"{Types.Response} {procedure.Name}()";
}