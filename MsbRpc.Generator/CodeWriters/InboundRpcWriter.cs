using System;
using System.CodeDom.Compiler;
using System.IO;
using System.Linq;
using MsbRpc.Generator.Extensions;
using MsbRpc.Generator.GenerationTree;
using MsbRpc.Generator.Info;
using static MsbRpc.Generator.IndependentNames;

namespace MsbRpc.Generator.CodeWriters;

internal static class InboundRpcWriter
{
    public static void Write(IndentedTextWriter writer, ProcedureCollection procedures)
    {
        foreach (Procedure procedure in procedures)
        {
            writer.WriteLine();
            WriteProcedureCall(writer, procedure);
        }
    }

    private static void WriteProcedureCall(IndentedTextWriter writer, Procedure procedure)
    {
        //header
        writer.Write($"private {Types.BufferWriter} {procedure.Names.Name}");
        writer.WriteLine($"({Types.BufferReader} {Parameters.ArgumentsBufferReader})");

        //body
        using (writer.InBlock())
        {
            WriteProcedureCallBody(writer, procedure);
        }
    }

    private static void WriteProcedureCallBody(IndentedTextWriter writer, Procedure procedure)
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
                    WriteReadParameter(writer, parameter, bufferReadMethodName);
                }
                else
                {
                    throw new NotImplementedException();
                }
            }
        }

        writer.WriteLine();

        //invoke implementation
        const string procedureResultVariable = Variables.ProcedureResult;
        string invocationWithoutParameters = $"{procedure.ReturnType.Names.Name} {procedureResultVariable}"
                                             + $" = this.{Fields.RpcImplementation}.{procedure.Names.Name}";
        if (parameters != null)
        {
            writer.Write($"{invocationWithoutParameters}(");
            writer.Write(string.Join(", ", parameters.Select(p => p.Names.ReceivedArgument)));
            writer.WriteLine(");");
        }
        else
        {
            writer.WriteLine($"{invocationWithoutParameters}();");
        }

        writer.WriteLine();

        const string resultSizeVariable = Variables.RpcResultSize;
        string? constantResultSizeExpression = procedure.ReturnType.ConstantSizeExpression;
        if (constantResultSizeExpression != null)
        {
            writer.WriteLine($"const int {resultSizeVariable} = {constantResultSizeExpression};");
        }
        else
        {
            throw new NotImplementedException();
        }

        const string resultWriterVariable = Variables.RpcResultWriter;
        writer.Write($"{Types.BufferWriter} {resultWriterVariable} = ");
        writer.WriteLine($"this.{Fields.RpcResolverEndPoint}.{Methods.EndPointGetResponseWriter}({resultSizeVariable});");
        writer.WriteLine($"{resultWriterVariable}.{Methods.BufferWrite}({procedureResultVariable});");
        writer.WriteLine($"return {resultWriterVariable};");
    }

    private static void WriteReadParameter(TextWriter writer, Parameter parameter, string bufferReadMethodName)
    {
        writer.Write($"{parameter.Type.Names.Name} {parameter.Names.ReceivedArgument} ");
        writer.WriteLine($"= {Parameters.ArgumentsBufferReader}.{bufferReadMethodName}();");
    }
}