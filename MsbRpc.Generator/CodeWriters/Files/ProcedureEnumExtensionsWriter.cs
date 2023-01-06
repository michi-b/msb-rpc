using System;
using System.CodeDom.Compiler;
using MsbRpc.Generator.CodeWriters.Utility;
using MsbRpc.Generator.GenerationTree;
using static MsbRpc.Generator.IndependentNames;

namespace MsbRpc.Generator.CodeWriters.Files;

internal class ProcedureEnumExtensionsWriter : CodeFileWriter
{
    private readonly string _className;
    private readonly string _procedureParameterOutOfRangeLine;
    private readonly ProcedureCollection _procedures;
    private readonly string _returnProcedureSwitchExpressionLine;
    protected override string FileName { get; }

    public ProcedureEnumExtensionsWriter(ContractNode contract, ProcedureCollection procedures)
        : base(contract)
    {
        _procedures = procedures;
        _className = $"{procedures.EnumName}{ExtensionsPostFix}";
        FileName = $"{_className}{GeneratedFilePostfix}";
        _procedureParameterOutOfRangeLine = GetArgumentOutOfRangeExceptionSwitchExpressionCase(Parameters.Procedure);
        _returnProcedureSwitchExpressionLine = $"return {Parameters.Procedure} switch";
    }

    protected override void Write(IndentedTextWriter writer)
    {
        writer.WriteLine($"public static class {_className}");
        using (writer.InBlock(Appendix.None))
        {
            WriteGetNameExtension(writer);

            writer.WriteLine();

            WriteGetIdExtension(writer);

            writer.WriteLine();

            //FromId method
            writer.WriteLine($"public static {_procedures.EnumName} {Methods.FromIdProcedureExtension}(int {Parameters.ProcedureId})");
            using (writer.InBlock())
            {
                writer.WriteLine($"return {Parameters.ProcedureId} switch");
                using (writer.InBlock(Appendix.SemicolonAndNewline))
                {
                    foreach (ProcedureNode procedure in _procedures)
                    {
                        writer.WriteLine($"{procedure.IntValueString} => {procedure.EnumValueString},");
                    }
                    writer.WriteLine(GetArgumentOutOfRangeExceptionSwitchExpressionCase(Parameters.ProcedureId));
                }
            }
        }
    }

    private void WriteGetNameExtension(IndentedTextWriter writer)
    {
        string GetCaseCode(ProcedureNode procedure) => $"nameof({procedure.EnumValueString})";
        WriteExtension(writer, "string", Methods.GetNameProcedureExtension, GetCaseCode);
    }

    private void WriteGetIdExtension(IndentedTextWriter writer)
    {
        string GetCaseCode(ProcedureNode procedure) => procedure.IntValueString;
        WriteExtension(writer, "int", Methods.GetIdProcedureExtension, GetCaseCode);
    }

    private void WriteExtension
        (IndentedTextWriter writer, string returnType, string extensionMethodName, Func<ProcedureNode, string> getCaseExpression)
    {
        writer.WriteLine($"public static {returnType} {extensionMethodName}(this {_procedures.EnumName} {Parameters.Procedure})");
        using (writer.InBlock())
        {
            writer.WriteLine(_returnProcedureSwitchExpressionLine);
            using (writer.InBlock(Appendix.SemicolonAndNewline))
            {
                foreach (ProcedureNode procedure in _procedures)
                {
                    writer.WriteLine($"{procedure.EnumValueString} => {getCaseExpression(procedure)},");
                }

                writer.WriteLine(_procedureParameterOutOfRangeLine);
            }
        }
    }
}