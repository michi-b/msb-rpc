using System.CodeDom.Compiler;
using MsbRpc.Generator.Extensions;
using MsbRpc.Generator.HelperTree;
using MsbRpc.Generator.Info;
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
            await WriteProcedureCallAsync(writer, procedures.Names.InterfaceField, procedure);
        }
    }

    private static async ValueTask WriteHandleRequestOverrideAsync(IndentedTextWriter writer, ProcedureCollection procedures)
    {
        const string procedureName = Parameters.Procedure;

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
                    string switchCase = $"{procedure.Names.EnumValue} => {procedure.Names.Name}({Parameters.ArgumentsBufferReader}),";
                    await writer.WriteLineAsync(switchCase);
                }

                await writer.WriteLineAsync($"_ => throw new {Types.ArgumentOutOfRangeException}(nameof({procedureName}), {procedureName}, null)");
            }
            await writer.ExitBlockAsync(BlockAdditions.SemicolonAndNewline);
        }
        await writer.ExitBlockAsync();
    }

    private static async ValueTask WriteProcedureCallAsync(IndentedTextWriter writer, string implementationFieldName, Procedure procedure)
    {
        //header
        await writer.WriteAsync($"private {Types.BufferWriter} {procedure.Names.Name}");
        await writer.WriteLineAsync($"({Types.BufferReader} {Parameters.ArgumentsBufferReader})");

        //body
        await writer.EnterBlockAsync();
        {
            await WriteProcedureCallBodyAsync(writer, implementationFieldName, procedure);
        }
        await writer.ExitBlockAsync();
    }

    private static async Task WriteProcedureCallBodyAsync(IndentedTextWriter writer, string implementationFieldName, Procedure procedure)
    {
        procedure.TryGetParameters(out ParameterCollection? parameters);

        //read arguments
        if (parameters != null)
        {
            foreach (Parameter parameter in parameters)
            {
                if (parameter.Type.SerializationKind.TryGetBufferReadMethodName(out string? bufferReadMethodName) && bufferReadMethodName != null)
                {
                    await WriteReadParameterAsync(writer, parameter, bufferReadMethodName);
                }
                else
                {
                    throw new NotImplementedException();
                }
            }
        }

        await writer.WriteLineAsync();

        //invoke implementation
        const string procedureResultVariable = Variables.ProcedureResult;
        string invocationWithoutParameters = $"{procedure.ReturnType.Names.Name} {procedureResultVariable}"
                                             + $" = {implementationFieldName}.{procedure.Names.Name}";
        if (parameters != null)
        {
            await writer.WriteAsync($"{invocationWithoutParameters}(");
            await writer.WriteAsync(string.Join(", ", parameters.Select(p => p.Names.ReceivedArgument)));
            await writer.WriteLineAsync(");");
        }
        else
        {
            await writer.WriteLineAsync($"{invocationWithoutParameters}();");
        }

        await writer.WriteLineAsync();

        const string resultSizeVariable = Variables.RpcResultSize;
        string? constantResultSizeExpression = procedure.ReturnType.ConstantSizeExpression;
        if (constantResultSizeExpression != null)
        {
            await writer.WriteLineAsync($"const int {resultSizeVariable} = {constantResultSizeExpression};");
        }
        else
        {
            throw new NotImplementedException();
        }

        const string resultWriterVariable = Variables.RpcResultWriter;
        await writer.WriteLineAsync($"{Types.BufferWriter} {resultWriterVariable} = {Methods.GetEndPointResultWriter}({resultSizeVariable});");
        await writer.WriteLineAsync($"{resultWriterVariable}.{Methods.BufferWrite}({procedureResultVariable});");
        await writer.WriteLineAsync($"return {resultWriterVariable};");
    }

    private static async ValueTask WriteReadParameterAsync(TextWriter writer, Parameter parameter, string bufferReadMethodName)
    {
        await writer.WriteAsync($"{parameter.Type.Names.Name} {parameter.Names.ReceivedArgument} ");
        await writer.WriteLineAsync($"= {Parameters.ArgumentsBufferReader}.{bufferReadMethodName}();");
    }
}