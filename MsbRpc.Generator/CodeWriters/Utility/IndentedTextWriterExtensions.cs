using System;
using System.CodeDom.Compiler;
using MsbRpc.Generator.GenerationTree;
using static MsbRpc.Generator.CodeWriters.Utility.IndependentCode;
using static MsbRpc.Generator.CodeWriters.Utility.IndependentNames;

namespace MsbRpc.Generator.CodeWriters.Utility;

internal static class IndentedTextWriterExtensions
{
    public static string GetResult(this IndentedTextWriter writer) => writer.InnerWriter.ToString();

    public static ParenthesesBlockScope GetParenthesesBlock(this IndentedTextWriter writer, Appendix additions = Appendix.NewLine) => new(writer, additions);

    public static BlockScope GetBlock(this IndentedTextWriter writer, Appendix additions = Appendix.NewLine) => new(writer, additions);

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
        writer.WriteLine(GetResultSizeResponseStatement);
        writer.WriteLine(GetResponseWriterStatement);
        writer.WriteLine(writeResultToResponseStatement);
        writer.WriteLine(ReturnResponseStatement);
    }

    /// <summary>
    ///     Configuration configuration = new Configuration();
    ///     configure?.Invoke(configuration);
    /// </summary>
    public static void WriteDeclareAndConfigureLocalConfigurationVariable(this IndentedTextWriter writer)
    {
        writer.WriteLine($"{Types.LocalConfiguration} {Variables.Configuration} = new {Types.LocalConfiguration}();");
        writer.WriteLine($"{Parameters.ConfigureAction}?.{Methods.Invoke}({Variables.Configuration});");
    }
}