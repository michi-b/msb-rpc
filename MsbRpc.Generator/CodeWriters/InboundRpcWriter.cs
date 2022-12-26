using System.CodeDom.Compiler;
using MsbRpc.Generator.Extensions;
using MsbRpc.Generator.HelperTree;
using static MsbRpc.Generator.IndependentNames;

namespace MsbRpc.Generator.CodeWriters;

public static class InboundRpcWriter
{
    public static async ValueTask WriteAsync(IndentedTextWriter writer, ProcedureCollection procedures)
    {
        await writer.WriteLineAsync();

        await WriteHandleRequestOverrideAsync(writer, procedures);

        foreach (Procedure procedure in procedures)
        {
            await writer.WriteLineAsync();
            // await WriteProcedureCallAsync(writer, procedures, procedure);
        }
    }

    private static async ValueTask WriteHandleRequestOverrideAsync(IndentedTextWriter writer, ProcedureCollection procedures)
    {
        string procedureName = Parameters.Procedure;

        //header
        await writer.WriteLineAsync($"protected override {Types.BufferWriter} {Methods.EndPointHandleRequest}");
        await writer.EnterParenthesesBlockAsync();
        {
            await writer.WriteLineAsync($"{procedures.Names.EnumType} {procedureName},");
            await writer.WriteLineAsync($"{Types.BufferReader} {Parameters.ArgumentsBufferReader}");
        }
        await writer.ExitParenthesesBlockAsync();

        //body
        await writer.EnterBlockAsync();
        {
            await writer.WriteLineAsync($"return {procedureName} switch");
            await writer.EnterBlockAsync();
            {
                foreach (Procedure procedure in procedures)
                {
                    await writer.WriteLineAsync
                        ($"{procedure.Names.EnumValue} => {procedure.Names.PascalCaseName}({Parameters.ArgumentsBufferReader}),");
                }

                await writer.WriteLineAsync($"_ => throw new {Types.ArgumentOutOfRangeException}(nameof({procedureName}), {procedureName}, null)");
            }
            await writer.ExitBlockAsync(BlockAdditions.SemicolonAndNewline);
        }
        await writer.ExitBlockAsync();
    }
}