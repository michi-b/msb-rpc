using System;
using System.CodeDom.Compiler;
using MsbRpc.Generator.Extensions;
using MsbRpc.Generator.GenerationTree;
using static MsbRpc.Generator.IndependentNames;

namespace MsbRpc.Generator.CodeWriters.Files;

internal class ProcedureEnumExtensionsFileWriter : CodeFileWriter
{
    private readonly string _className;
    private readonly string _procedureParameterOutOfRangeLine;

    private readonly ProcedureCollection _procedures;
    private readonly string _returnProcedureSwitchExpressionLine;
    protected override string FileName { get; }

    public ProcedureEnumExtensionsFileWriter(ContractNode contract, ProcedureCollection procedures)
        : base(contract)
    {
        _procedures = procedures;
        _className = $"{_procedures.Names.EnumType}{ExtensionsPostFix}";
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

            WriteGetInvertsDirectionExtension(writer);
        }
    }

    private void WriteGetNameExtension(IndentedTextWriter writer)
    {
        string GetCaseCode(Procedure procedure) => $"nameof({procedure.Names.EnumValue})";
        WriteExtension(writer, "string", Methods.GetNameProcedureEnumExtension, GetCaseCode);
    }

    private void WriteGetInvertsDirectionExtension(IndentedTextWriter writer)
    {
        string GetCaseCode(Procedure procedure) => procedure.InvertsDirection ? "true" : "false";
        WriteExtension(writer, "bool", Methods.GetInvertsDirectionProcedureExtension, GetCaseCode);
    }

    private void WriteExtension(IndentedTextWriter writer, string returnType, string extensionMethodName, Func<Procedure, string> getCaseExpression)
    {
        writer.WriteLine($"public static {returnType} {extensionMethodName}(this {_procedures.Names.EnumType} {Parameters.Procedure})");
        using (writer.InBlock())
        {
            writer.WriteLine(_returnProcedureSwitchExpressionLine);
            using (writer.InBlock(Appendix.SemicolonAndNewline))
            {
                foreach (Procedure procedure in _procedures)
                {
                    writer.WriteLine($"{procedure.Names.EnumValue} => {getCaseExpression(procedure)},");
                }

                writer.WriteLine(_procedureParameterOutOfRangeLine);
            }
        }
    }
}