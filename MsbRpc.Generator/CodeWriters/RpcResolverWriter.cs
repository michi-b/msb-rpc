using System;
using System.CodeDom.Compiler;
using System.IO;
using System.Linq;
using MsbRpc.Generator.Extensions;
using MsbRpc.Generator.GenerationTree;
using MsbRpc.Generator.Info;
using static MsbRpc.Generator.IndependentNames;

namespace MsbRpc.Generator.CodeWriters;

internal static class RpcResolverWriter
{
    public static void Write(IndentedTextWriter writer, ProcedureCollection procedures, EndPoint endPoint, string className)
    {
        writer.WriteLine();

        const string implementation = Parameters.EndPointImplementation;
        const string listen = Methods.EndPointListen;
        const string listenReturnCode = Types.MessengerListenReturnCode;
        const string createResolver = Methods.EndPointCreateResolver;
        const string resolver = Types.LocalEndPointResolver;
        const string procedure = Parameters.Procedure;
        const string arguments = Parameters.ArgumentsBufferReader;

        string procedureType = procedures.Names.EnumType;
        string implementationInterface = endPoint.Names.ImplementationInterface;

        string implementationParameterDeclaration = $"{implementationInterface} {implementation}";
        string resolverInterface = $"{Types.RpcResolverInterface}<{procedureType}>";

        //listen override
        writer.Write($"public {listenReturnCode} {listen}({implementationParameterDeclaration})");
        writer.WriteLine($" => base.{listen}({createResolver}({implementation}));");
        writer.WriteLine();

        //create resolver
        writer.Write($"public {resolver} {createResolver}({implementationParameterDeclaration})");
        writer.WriteLine($" => new {resolver}(this, {implementation});");
        writer.WriteLine();

        //resolver class
        writer.WriteLine($"public class {resolver} : {resolverInterface}");
        using (writer.InBlock())
        {
            //fields
            writer.WriteLine($"private readonly {className} {Fields.RpcResolverEndPoint};");
            writer.WriteLine($"private readonly {implementationInterface} {Fields.RpcImplementation};");
            writer.WriteLine();

            //constructor
            writer.WriteLine($"public {resolver}({className} {Parameters.RpcEndPoint}, {implementationParameterDeclaration})");
            using (writer.InBlock())
            {
                writer.WriteLine($"this.{Fields.RpcResolverEndPoint} = {Parameters.RpcEndPoint};");
                writer.WriteLine($"this.{Fields.RpcImplementation} = {implementation};");
            }

            writer.WriteLine();

            //execute header
            writer.Write($"{Types.ByteArraySegment} {resolverInterface}.{Methods.RpcResolverExecute}");
            writer.WriteLine($"({procedureType} {procedure}, {Types.BufferReader} {arguments})");
            //execute body
            using (writer.InBlock())
            {
                writer.WriteLine($"return {procedure} switch");
                using (writer.InBlock(Appendix.SemicolonAndNewline))
                {
                    foreach (Procedure currentProcedure in procedures)
                    {
                        string switchCase =
                            $"{currentProcedure.Names.EnumValue} => this.{currentProcedure.Names.Name}({Parameters.ArgumentsBufferReader}),";
                        writer.WriteLine(switchCase);
                    }

                    writer.WriteLine($"_ => throw new {Types.ArgumentOutOfRangeException}(nameof({procedure}), {procedure}, null)");
                }
            }

            WriteProcedures(writer, procedures);
        }
    }

    private static void WriteProcedures(IndentedTextWriter writer, ProcedureCollection procedures)
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
        writer.Write($"private {Types.ByteArraySegment} {procedure.Names.Name}");
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
                    throw new InvalidOperationException($"there is no buffer read method for parameter {parameter}");
                }
            }

            writer.WriteLine();
        }

        //invoke implementation

        if (procedure.ReturnType.IsVoid)
        {
            WriteVoidImplementationInvocation(writer, procedure, parameters);
        }
        else
        {
            WriteInvocationWithReturn(writer, procedure, parameters);
        }
    }

    private static void WriteInvocationWithReturn(IndentedTextWriter writer, Procedure procedure, ParameterCollection? parameters)
    {
        const string procedureResultVariable = Variables.ProcedureResult;
        string invocationWithoutParameters = $"{procedure.ReturnType.Names.Name} {procedureResultVariable}"
                                             + $" = this.{Fields.RpcImplementation}.{procedure.Names.Name}";
        WriteInvocation(writer, invocationWithoutParameters, parameters);

        writer.WriteLine();

        const string resultSizeVariable = Variables.RpcResultSize;
        string? constantResultSizeExpression = procedure.ReturnType.ConstantSizeExpression;
        if (constantResultSizeExpression != null)
        {
            writer.WriteLine($"const int {resultSizeVariable} = {constantResultSizeExpression};");
        }
        else
        {
            throw new InvalidOperationException($"there is no constant size expression for return type {procedure.ReturnType}");
        }

        const string resultWriterVariable = Variables.RpcResultWriter;
        writer.Write($"{Types.BufferWriter} {resultWriterVariable} = ");
        writer.WriteLine($"this.{Fields.RpcResolverEndPoint}.{Methods.EndPointGetResponseWriter}({resultSizeVariable});");
        writer.WriteLine($"{resultWriterVariable}.{Methods.BufferWrite}({procedureResultVariable});");
        writer.WriteLine($"return {resultWriterVariable}.{Fields.BufferWriterBuffer};");
    }

    private static void WriteVoidImplementationInvocation(IndentedTextWriter writer, Procedure procedure, ParameterCollection? parameters)
    {
        string invocationWithoutParameters = $"this.{Fields.RpcImplementation}.{procedure.Names.Name}";
        WriteInvocation(writer, invocationWithoutParameters, parameters);
        writer.WriteLine($"return {IndependentCode.EmptyBufferExpression};");
    }

    private static void WriteInvocation(TextWriter writer, string invocationWithoutParameters, ParameterCollection? parameters)
    {
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
    }

    private static void WriteReadParameter(TextWriter writer, Parameter parameter, string bufferReadMethodName)
    {
        writer.Write($"{parameter.Type.Names.Name} {parameter.Names.ReceivedArgument} ");
        writer.WriteLine($"= {Parameters.ArgumentsBufferReader}.{bufferReadMethodName}();");
    }
}