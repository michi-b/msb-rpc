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
        FileName = $"{GeneratedNamespace}.{_className}{GeneratedFilePostfix}";
        _procedureParameterOutOfRangeLine = GetArgumentOutOfRangeExceptionSwitchExpressionCase(Parameters.Procedure);
        _returnProcedureSwitchExpressionLine = $"return {Parameters.Procedure} switch";
    }

    protected override async ValueTask WriteAsync(IndentedTextWriter writer)
    {
        await writer.WriteLineAsync($"public static class {_className}");
        await writer.EnterBlockAsync();
        {
            await WriteGetNameExtensionAsync(writer);

            await writer.WriteLineAsync();

            await WriteGetInvertsDirectionExtensionAsync(writer);
        }
        await writer.ExitBlockAsync(BlockAdditions.None);
    }

    private async ValueTask WriteGetNameExtensionAsync(IndentedTextWriter writer)
    {
        string GetCaseCode(Procedure procedure) => $"nameof({procedure.Names.EnumValue})";
        await WriteExtensionAsync(writer, "string", Methods.GetNameProcedureEnumExtension, GetCaseCode);
    }

    private async ValueTask WriteGetInvertsDirectionExtensionAsync(IndentedTextWriter writer)
    {
        string GetCaseCode(Procedure procedure) => procedure.InvertsDirection ? "true" : "false";
        await WriteExtensionAsync(writer, "bool", Methods.GetInvertsDirectionProcedureExtension, GetCaseCode);
    }

    private async ValueTask WriteExtensionAsync
        (IndentedTextWriter writer, string returnType, string extensionMethodName, Func<Procedure, string> getCaseExpression)
    {
        await writer.WriteLineAsync($"public static {returnType} {extensionMethodName}(this {_procedures.Names.EnumType} {Parameters.Procedure})");
        await writer.EnterBlockAsync();
        {
            await writer.WriteLineAsync(_returnProcedureSwitchExpressionLine);
            await writer.EnterBlockAsync();
            {
                foreach (Procedure procedure in _procedures)
                {
                    await writer.WriteLineAsync($"{procedure.Names.EnumValue} => {getCaseExpression(procedure)},");
                }

                await writer.WriteLineAsync(_procedureParameterOutOfRangeLine);
            }
            await writer.ExitBlockAsync(BlockAdditions.Semicolon);
        }
        await writer.ExitBlockAsync();
    }
}