using System;
using System.CodeDom.Compiler;
using MsbRpc.Generator.GenerationTree;
using MsbRpc.Generator.Serialization;
using MsbRpc.Generator.Utility;
using static MsbRpc.Generator.Utility.IndependentCode;
using static MsbRpc.Generator.Utility.Names.Types;

namespace MsbRpc.Generator.Extensions;

public static class IndentedTextWriterExtensions
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
                writer.WriteLine($"{Names.Variables.Exception},");
                writer.WriteLine($"{procedure.FullName},");
                writer.WriteLine(executionStageFullName);
            }
        }
    }

    public static void WriteSerializationSizeExpression(this IndentedTextWriter writer, ISerialization serialization, string targetExpression)
    {
        serialization.WriteSizeExpression(writer, targetExpression);
    }

    public static void WriteDeserializationExpression(this IndentedTextWriter writer, ISerialization serialization, string bufferReaderExpression)
    {
        serialization.WriteDeserializationExpression(writer, bufferReaderExpression);
    }

    /// <summary>
    ///     same as <see cref="MsbRpc.Generator.Serialization.ISerialization.WriteSerializationStatement" />, but with trailing
    ///     semicolon and new line
    /// </summary>
    public static void WriteFinalizedSerializationStatement
    (
        this IndentedTextWriter writer,
        ISerialization serialization,
        string bufferWriterExpression,
        string valueExpression
    )
    {
        serialization.WriteSerializationStatement(writer, bufferWriterExpression, valueExpression);
        writer.FinalizeSerializationStatement(serialization);
    }

    /// <summary>
    ///     same as <see cref="WriteDeserializationExpression" />, but with trailing semicolon and new line
    /// </summary>
    public static void WriteFinalizedDeserializationStatement
    (
        this IndentedTextWriter writer,
        ISerialization serialization,
        string bufferReaderExpression
    )
    {
        serialization.WriteDeserializationExpression(writer, bufferReaderExpression);
        writer.WriteSemicolonLineBreak();
    }
}