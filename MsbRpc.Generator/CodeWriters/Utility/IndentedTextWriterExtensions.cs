using System;
using System.CodeDom.Compiler;
using MsbRpc.Generator.GenerationTree;

namespace MsbRpc.Generator.CodeWriters.Utility;

internal static class IndentedTextWriterExtensions
{
    public static string GetResult(this IndentedTextWriter writer) => writer.InnerWriter.ToString();

    public static ParenthesesBlockScope GetParenthesesBlock(this IndentedTextWriter writer, Appendix additions = Appendix.NewLine)
        => new(writer, additions);

    public static BlockScope GetBlock(this IndentedTextWriter writer, Appendix additions = Appendix.NewLine) => new(writer, additions);

    public static void WriteProcedureReturnSwitch
    (
        this IndentedTextWriter writer,
        ProcedureCollectionNode procedures,
        Func<ProcedureNode, string> getCaseExpression
    )
    {
        writer.WriteLine(IndependentCode.ReturnProcedureSwitch);
        using (writer.GetBlock(Appendix.SemicolonAndNewline))
        {
            foreach (ProcedureNode procedure in procedures)
            {
                writer.WriteLine($"{procedure.ProcedureEnumValue} => {getCaseExpression(procedure)},");
            }

            writer.WriteLine(IndependentCode.ProcedureParameterOutOfRangeSwitchExpressionCase);
        }
    }
}