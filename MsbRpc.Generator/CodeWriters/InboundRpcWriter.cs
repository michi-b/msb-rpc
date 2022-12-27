using System.CodeDom.Compiler;
using MsbRpc.Generator.Extensions;
using MsbRpc.Generator.GenerationTree;
using MsbRpc.Generator.Info;
using static MsbRpc.Generator.IndependentNames;

namespace MsbRpc.Generator.CodeWriters;

public static class InboundRpcWriter
{
    public static async ValueTask WriteAsync(IndentedTextWriter writer, ProcedureCollection procedures)
    {
        foreach (Procedure procedure in procedures)
        {
            await writer.WriteLineAsync();
            await WriteProcedureCallAsync(writer, procedure);
        }
    }

    private static async ValueTask WriteProcedureCallAsync(IndentedTextWriter writer, Procedure procedure)
    {
        //header
        await writer.WriteAsync($"private {Types.BufferWriter} {procedure.Names.Name}");
        await writer.WriteLineAsync($"({Types.BufferReader} {Parameters.ArgumentsBufferReader})");

        //body
        await writer.EnterBlockAsync();
        {
            await WriteProcedureCallBodyAsync(writer, procedure);
        }
        await writer.ExitBlockAsync();
    }

    private static async Task WriteProcedureCallBodyAsync(IndentedTextWriter writer, Procedure procedure)
    {
        ParameterCollection? parameters = procedure.Parameters;

        //read arguments
        if (parameters != null)
        {
            foreach (Parameter parameter in parameters)
            {
                string? bufferReadMethodName = parameter.Type.SerializationKind.GetBufferReadMethodName();
                if (bufferReadMethodName != null)
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
                                             + $" = {Fields.RpcImplementation}.{procedure.Names.Name}";
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
        await writer.WriteAsync($"{Types.BufferWriter} {resultWriterVariable} = ");
        await writer.WriteLineAsync($"{Fields.RpcResolverEndPoint}.{Methods.EndPointGetResponseWriter}({resultSizeVariable});");
        await writer.WriteLineAsync($"{resultWriterVariable}.{Methods.BufferWrite}({procedureResultVariable});");
        await writer.WriteLineAsync($"return {resultWriterVariable};");
    }

    private static async ValueTask WriteReadParameterAsync(TextWriter writer, Parameter parameter, string bufferReadMethodName)
    {
        await writer.WriteAsync($"{parameter.Type.Names.Name} {parameter.Names.ReceivedArgument} ");
        await writer.WriteLineAsync($"= {Parameters.ArgumentsBufferReader}.{bufferReadMethodName}();");
    }
}