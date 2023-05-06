using System;
using System.CodeDom.Compiler;
using MsbRpc.Generator.GenerationTree;
using MsbRpc.Generator.Serialization;
using static MsbRpc.Generator.CodeWriters.Utility.IndependentCode;
using static MsbRpc.Generator.CodeWriters.Utility.IndependentNames.Types;

namespace MsbRpc.Generator.CodeWriters.Utility;

internal static class IndentedTextWriterExtensions
{
    public static string GetResult(this IndentedTextWriter writer) => writer.InnerWriter.ToString();

    public static ParenthesesBlockScope GetParenthesesBlock(this IndentedTextWriter writer, Appendix additions = Appendix.NewLine) => new(writer, additions);

    public static BlockScope GetBlock(this IndentedTextWriter writer, Appendix appendix = Appendix.NewLine) => new(writer, appendix);

    public static BlockScope GetTryBlock(this IndentedTextWriter writer, Appendix appendix = Appendix.NewLine)
    {
        writer.WriteLine("try");
        return writer.GetBlock(appendix);
    }

    public static void WriteProcedureReturnSwitch
    (
        this IndentedTextWriter writer,
        ProcedureCollectionNode procedures,
        Func<ProcedureNode, string> getCaseExpression
    )
    {
        writer.WriteLine(ReturnProcedureSwitch);
        using (writer.GetBlock(Appendix.SemicolonAndNewline))
        {
            foreach (ProcedureNode procedure in procedures)
            {
                writer.WriteLine($"{procedure.ProcedureEnumValue} => {getCaseExpression(procedure)},");
            }

            writer.WriteLine(ProcedureParameterOutOfRangeSwitchExpressionCase);
        }
    }

    /// <summary>
    ///     MsbRpc.Serialization.Buffers.Response response = Buffer.GetResponse(Implementation.RanToCompletion, resultSize);
    ///     MsbRpc.Serialization.Buffers.BufferWriter responseWriter = response.GetWriter();
    ///     responseWriter.Write(result);
    ///     return response;
    /// </summary>
    public static void WriteReturnResultResponse(this IndentedTextWriter writer, string writeResultToResponseStatement)
    {
        writer.WriteLine
        (
            $"{Response} {IndependentNames.Variables.Response} = {IndependentNames.Fields.EndPointBuffer}.{IndependentNames.Methods.GetResponse}("
            + $"{IndependentNames.Fields.InboundEndpointImplementation}.{IndependentNames.Properties.RanToCompletion}, "
            + $"{IndependentNames.Variables.ResultSize});"
        );
        writer.WriteLine(GetResponseWriterStatement);
        writer.WriteLine(writeResultToResponseStatement);
        writer.WriteLine(ReturnResponseStatement);
    }

    public static void WriteRpcExecutionExceptionCatchBlock
    (
        this IndentedTextWriter writer,
        ProcedureCollectionNode procedureCollection,
        ProcedureNode procedure,
        string executionStageFullName
    )
    {
        writer.WriteLine(ExceptionCatchStatement);
        using (writer.GetBlock())
        {
            writer.WriteLine($"throw new {RpcExecutionException}<{procedureCollection.ProcedureEnumType}>");
            using (writer.GetParenthesesBlock(Appendix.SemicolonAndNewline))
            {
                writer.WriteLine($"{IndependentNames.Variables.Exception},");
                writer.WriteLine($"{procedure.FullName},");
                writer.WriteLine(executionStageFullName);
            }
        }
    }
    
    public static void WriteSerializationSizeExpression(this IndentedTextWriter writer, ISerialization serialization, string targetExpression)
    {
        serialization.WriteSizeExpression(writer, targetExpression);
    }
    
    public static void WriteSerializationStatement(this IndentedTextWriter writer, ISerialization serialization, string bufferWriterExpression, string valueExpression)
    {
        serialization.WriteSerializationStatement(writer, bufferWriterExpression, valueExpression);
    }
    
    public static void WriteDeserializationExpression(this IndentedTextWriter writer, ISerialization serialization, string bufferReaderExpression)
    {
        serialization.WriteDeserializationExpression(writer, bufferReaderExpression);
    }
}