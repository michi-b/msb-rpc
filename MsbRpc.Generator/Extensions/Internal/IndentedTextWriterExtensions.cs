#region

using System;
using System.CodeDom.Compiler;
using MsbRpc.Generator.GenerationTree;
using MsbRpc.Generator.Utility;

#endregion

namespace MsbRpc.Generator.Extensions.Internal;

internal static class IndentedTextWriterExtensions
{
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

    public static void WriteRpcExecutionExceptionCatchBlock
    (
        this IndentedTextWriter writer,
        ProcedureCollectionNode procedureCollection,
        ProcedureNode procedure,
        string executionStageFullName
    )
    {
        writer.WriteLine(IndependentCode.ExceptionCatchStatement);
        using (writer.GetBlock())
        {
            writer.WriteLine($"throw new {Names.Types.RpcExecutionException}<{procedureCollection.ProcedureEnumType}>");
            using (writer.GetParenthesesBlock(Appendix.SemicolonAndNewline))
            {
                writer.WriteLine($"{Names.Variables.Exception},");
                writer.WriteLine($"{procedure.FullName},");
                writer.WriteLine(executionStageFullName);
            }
        }
    }
}