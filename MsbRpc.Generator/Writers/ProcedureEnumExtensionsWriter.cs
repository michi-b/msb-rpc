using System.CodeDom.Compiler;
using MsbRpc.Generator.Extensions;
using MsbRpc.Generator.HelperTree;
using static MsbRpc.Generator.IndependentNames;

namespace MsbRpc.Generator.Writers;

internal class ProcedureEnumExtensionsWriter : CodeWriter
{
    protected override string FileName { get; }

    private readonly ProcedureCollection _procedures;
    private readonly string _className;
    private readonly string _procedureParameterOutOfRangeLine;
    private readonly string _returnProcedureSwitchExpressionLine;
    
    public ProcedureEnumExtensionsWriter(ContractNode contract, ProcedureCollection procedures)
        : base(contract)
    {
        _procedures = procedures;
        _className = $"{_procedures.Names.EnumType}{ExtensionsPostfix}";
        FileName = $"{GeneratedNamespace}.{_className}{GeneratedFileEnding}";
        _procedureParameterOutOfRangeLine = GetArgumentOutOfRangeExceptionSwitchExpressionCase(Parameters.Procedure);
        _returnProcedureSwitchExpressionLine = $"return {Parameters.Procedure} switch";
    }

    protected override async ValueTask Write(IndentedTextWriter writer)
    {
        await writer.WriteLineAsync($"public static class {_className}");
        await writer.EnterBlockAsync();
        {
            await WriteGetNameExtension(writer);

            await writer.WriteLineAsync();
            
            await WriteGetInvertsDirectionExtension(writer);
        }
        await writer.ExitBlockAsync();
    }

    private async ValueTask  WriteGetNameExtension(IndentedTextWriter writer)
    {
        string GetCaseCode(Procedure procedure) => $"nameof({procedure.Names.EnumValue})";
        await WriteExtension(writer, "string", Methods.GetNameProcedureEnumExtension, GetCaseCode);
    }

    private async ValueTask WriteGetInvertsDirectionExtension(IndentedTextWriter writer)
    {
        string GetCaseCode(Procedure procedure) => procedure.InvertsDirection ? "true" : "false";
        await WriteExtension(writer, "bool", Methods.GetInvertsDirectionProcedureExtension, GetCaseCode);
    }

    private async ValueTask WriteExtension(IndentedTextWriter writer,string returnType, string extensionMethodName, Func<Procedure, string> getCaseExpression )
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
            await writer.ExitBlockAsync(BlockOptions.WithTrailingSemicolon);
        }
        await writer.ExitBlockAsync();
    }
}