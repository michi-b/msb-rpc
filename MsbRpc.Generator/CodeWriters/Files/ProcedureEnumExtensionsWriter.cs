using System;
using System.CodeDom.Compiler;
using MsbRpc.Generator.CodeWriters.Utility;
using MsbRpc.Generator.GenerationTree;
using static MsbRpc.Generator.CodeWriters.Utility.IndependentCode;
using static MsbRpc.Generator.CodeWriters.Utility.IndependentNames;

namespace MsbRpc.Generator.CodeWriters.Files;

internal class ProcedureEnumExtensionsWriter : CodeFileWriter
{
    private readonly string _className;
    private readonly ProcedureCollectionNode _procedures;
    protected override string FileName { get; }

    public ProcedureEnumExtensionsWriter(ProcedureCollectionNode procedures)
        : base(procedures.Contract)
    {
        _procedures = procedures;
        _className = $"{procedures.EnumName}{ExtensionsPostFix}";
        FileName = $"{_className}{GeneratedFilePostfix}";
    }

    protected override void Write(IndentedTextWriter writer)
    {
        writer.WriteLine($"public static class {_className}");
        using (writer.GetBlock(Appendix.None))
        {
            WriteGetNameExtension(writer);

            writer.WriteLine();

            WriteGetIdExtension(writer);

            writer.WriteLine();

            //FromId method
            writer.WriteLine($"public static {_procedures.EnumName} {Methods.FromIdProcedureExtension}(int {Parameters.ProcedureId})");
            using (writer.GetBlock())
            {
                writer.WriteLine($"return {Parameters.ProcedureId} switch");
                using (writer.GetBlock(Appendix.SemicolonAndNewline))
                {
                    foreach (ProcedureNode procedure in _procedures)
                    {
                        writer.WriteLine($"{procedure.IntValueString} => {procedure.EnumValueString},");
                    }

                    writer.WriteLine(GetArgumentOutOfRangeSwitchExpressionCase(Parameters.ProcedureId));
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
        using (writer.GetBlock())
        {
            writer.WriteLine(ReturnProcedureParameterSwitchExpressionLine);
            using (writer.GetBlock(Appendix.SemicolonAndNewline))
            {
                foreach (ProcedureNode procedure in _procedures)
                {
                    writer.WriteLine($"{procedure.EnumValueString} => {getCaseExpression(procedure)},");
                }

                writer.WriteLine(ProcedureParameterOutOfRangeSwitchExpressionCase);
            }
        }
    }
}