﻿using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.IO;
using MsbRpc.Generator.Extensions;
using MsbRpc.Generator.GenerationTree;
using MsbRpc.Generator.Info;
using static MsbRpc.Generator.IndependentNames;

namespace MsbRpc.Generator.CodeWriters;

internal static class RpcInvocationWriter
{
    public static void Write(IndentedTextWriter writer, ProcedureCollection procedures)
    {
        foreach (Procedure procedure in procedures)
        {
            writer.WriteLine();
            WriteInvocation(writer, procedures, procedure);
        }
    }

    private static string GetSendAndAwaitRequestExpression(bool hasParameters)
        => hasParameters
            ? $"await this.{Methods.EndPointSendRequestAsync}("
              + $"{Variables.Procedure}, "
              + $"{Variables.ArgumentsWriter}.{Properties.BufferWriterBuffer}, "
              + $"{Parameters.CancellationToken})"
            : $"await this.{Methods.EndPointSendRequestAsync}("
              + $"{Variables.Procedure}, "
              + $"{IndependentCode.EmptyBufferExpression}, "
              + $"{Parameters.CancellationToken})";

    private static void WriteInvocation(IndentedTextWriter writer, ProcedureCollection procedures, Procedure procedure)
    {
        //header
        ParameterCollection? parameters = procedure.Parameters;

        writer.Write($"public async {Types.VaLueTask}");
        if (!procedure.ReturnType.IsVoid)
        {
            writer.Write($"<{procedure.ReturnType.Names.Name}>");
        }

        writer.Write($" {procedure.Names.Async}(");

        //parameters
        if (parameters != null)
        {
            foreach (Parameter t in parameters)
            {
                WriteProcedureCallParameter(writer, t);
                writer.Write(", ");
            }
        }

        writer.WriteLine($"{Types.CancellationToken} {Parameters.CancellationToken})");

        //body
        using (writer.InBlock())
        {
            WriteProcedureCallBody(writer, procedures, procedure, parameters);
        }
    }

    private static void WriteProcedureCallBody
    (
        IndentedTextWriter writer,
        ProcedureCollection procedures,
        Procedure procedure,
        ParameterCollection? parameters
    )
    {
        writer.WriteLine($"this.{Methods.EndPointEnterCalling}();");
        writer.WriteLine();

        if (parameters != null)
        {
            WriteParametersSizeSumCalculation(writer, parameters);

            writer.WriteLine();

            writer.WriteLine
            (
                $"{Types.BufferWriter} {Variables.ArgumentsWriter}"
                + $" = this.{Methods.EndPointGetRequestWriter}({Variables.ParametersSizeSum});"
            );
            writer.WriteLine();
            foreach (Parameter parameter in parameters)
            {
                writer.WriteLine(IndependentCode.GetBufferWrite(parameter.Names.Name));
            }

            writer.WriteLine();
        }

        writer.WriteLine($"const {procedures.Names.EnumType} {Variables.Procedure} = {procedure.Names.EnumValue};");
        writer.WriteLine();

        bool hasReturnValue = !procedure.ReturnType.IsVoid;

        if (hasReturnValue)
        {
            WriteSendRequestWithReturn(writer, procedure);
        }
        else
        {
            writer.WriteLine($"{GetSendAndAwaitRequestExpression(parameters != null)};");
        }

        writer.WriteLine();
        writer.WriteLine($"this.{Methods.EndPointExitCalling}({Variables.Procedure});");
        if (hasReturnValue)
        {
            writer.WriteLine();
            writer.WriteLine($"return {Variables.ProcedureResult};");
        }
    }

    private static void WriteSendRequestWithReturn(TextWriter writer, Procedure procedure)
    {
        writer.WriteLine($"{Types.BufferReader} {Variables.ResultReader} = new({GetSendAndAwaitRequestExpression(procedure.Parameters != null)});");
        writer.WriteLine();

        TypeNode returnType = procedure.ReturnType;
        string? bufferReadMethodName = returnType.SerializationKind.GetBufferReadMethodName();
        if (bufferReadMethodName != null)
        {
            writer.Write($"{returnType.Names.Name} {Variables.ProcedureResult} ");
            writer.WriteLine($"= {Variables.ResultReader}.{bufferReadMethodName}();");
        }
        else
        {
            throw new InvalidOperationException($"there is not buffer read method for the return type {returnType}");
        }
    }

    private static void WriteParametersSizeSumCalculation(IndentedTextWriter writer, ParameterCollection parameters)
    {
        foreach (Parameter parameter in parameters)
        {
            string? constantSizeExpression = parameter.Type.ConstantSizeExpression;
            if (constantSizeExpression != null)
            {
                writer.WriteLine($"const int {parameter.Names.SizeVariable} = {constantSizeExpression};");
            }
            else
            {
                throw new InvalidOperationException
                (
                    "there is no constant size expression"
                    + $"for the parameter of type {parameter.Type.Names.FullName}"
                );
            }
        }

        IReadOnlyList<Parameter> constantSizeParameters = parameters.ConstantSizeParameters;
        int constantSizeParametersCount = constantSizeParameters.Count;
        if (constantSizeParametersCount > 0)
        {
            writer.WriteLine();

            writer.Write($"const int {Variables.ConstantSizeRpcParametersSize} = ");
            writer.Write(constantSizeParameters[0].Names.SizeVariable);
            for (int i = 1; i < constantSizeParametersCount; i++)
            {
                writer.Write($" + {constantSizeParameters[i].Names.SizeVariable}");
            }

            writer.WriteLine(';');
        }

        writer.WriteLine();

        if (parameters.HasOnlyConstantSizeParameters)
        {
            if (constantSizeParameters.Count > 0)
            {
                writer.WriteLine($"const int {Variables.ParametersSizeSum} = {Variables.ConstantSizeRpcParametersSize};");
            }
            else
            {
                writer.WriteLine($"const int {Variables.ParametersSizeSum} = 0;");
            }
        }
        else
        {
            throw new InvalidOperationException("procedure collection has non-constant size parameters");
        }
    }

    private static void WriteProcedureCallParameter(TextWriter writer, Parameter parameter)
    {
        writer.Write($"{parameter.Type.Names.Name} {parameter.Names.Name}");
    }
}