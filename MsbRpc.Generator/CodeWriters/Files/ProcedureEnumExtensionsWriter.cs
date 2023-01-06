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
        _className = $"{procedures.ProcedureEnumName}{ExtensionsPostFix}";
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
            writer.WriteLine($"public static {_procedures.ProcedureEnumName} {Methods.FromIdProcedureExtension}(int {Parameters.ProcedureId})");
            using (writer.GetBlock())
            {
                writer.WriteLine($"return {Parameters.ProcedureId} switch");
                using (writer.GetBlock(Appendix.SemicolonAndNewline))
                {
                    foreach (ProcedureNode procedure in _procedures)
                    {
                        writer.WriteLine($"{procedure.ProcedureEnumIntValue} => {procedure.ProcedureEnumValue},");
                    }

                    writer.WriteLine(GetArgumentOutOfRangeSwitchExpressionCase(Parameters.ProcedureId));
                }
            }
        }
    }

    private void WriteGetNameExtension(IndentedTextWriter writer)
    {
        WriteExtension(writer, "string", Methods.GetNameProcedureExtension, procedure => $"nameof({procedure.ProcedureEnumValue})");
    }

    private void WriteGetIdExtension(IndentedTextWriter writer)
    {
        WriteExtension(writer, "int", Methods.GetIdProcedureExtension, procedure => procedure.ProcedureEnumIntValue);
    }

    private void WriteExtension(IndentedTextWriter writer, string returnType, string methodName, Func<ProcedureNode, string> getCase)
    {
        writer.WriteLine($"public static {returnType} {methodName}(this {_procedures.ProcedureEnumName} {Parameters.Procedure})");
        using (writer.GetBlock())
        {
            writer.WriteProcedureReturnSwitch(_procedures, getCase);
        }
    }
}