using System.CodeDom.Compiler;
using MsbRpc.Generator.Extensions;
using MsbRpc.Generator.GenerationTree;
using MsbRpc.Generator.Info;
using static MsbRpc.Generator.IndependentNames;

namespace MsbRpc.Generator.CodeWriters;

internal static class OutboundRpcWriter
{
    public static async ValueTask WriteAsync(IndentedTextWriter writer, ProcedureCollection procedures)
    {
        foreach (Procedure procedure in procedures)
        {
            await writer.WriteLineAsync();
            await WriteProcedureCallAsync(writer, procedures, procedure);
        }
    }

    private static async ValueTask WriteProcedureCallAsync(IndentedTextWriter writer, ProcedureCollection procedures, Procedure procedure)
    {
        //header
        string returnType = procedure.ReturnType.Names.Name;
        await writer.WriteAsync($"public async {Types.Task}<{returnType}> {procedure.Names.Async}");

        ParameterCollection? parameters = procedure.Parameters;

        await writer.WriteAsync('(');
        if (parameters != null)
        {
            foreach (Parameter t in parameters)
            {
                await WriteProcedureCallParameterAsync(writer, t);
                await writer.WriteAsync(", ");
            }
        }

        await writer.WriteLineAsync($"{Types.CancellationToken} {Parameters.CancellationToken})");

        //body
        await writer.EnterBlockAsync();
        {
            await WriteProcedureCallBodyAsync(writer, procedures, procedure, parameters);
        }
        await writer.ExitBlockAsync();
    }

    private static async Task WriteProcedureCallBodyAsync
    (
        IndentedTextWriter writer,
        ProcedureCollection procedures,
        Procedure procedure,
        ParameterCollection? parameters
    )
    {
        await writer.WriteLineAsync($"{Methods.EndPointEnterCalling}();");
        await writer.WriteLineAsync();
        if (parameters != null)
        {
            await WriteParametersSizeSumCalculationAsync(writer, parameters);
        }

        await writer.WriteLineAsync();
        await writer.WriteLineAsync
        (
            $"{Types.BufferWriter} {Variables.ArgumentsWriter}"
            + $" = {Methods.EndPointGetRequestWriter}({Variables.ParametersSizeSum});"
        );
        await writer.WriteLineAsync();
        if (parameters != null)
        {
            foreach (Parameter parameter in parameters)
            {
                await writer.WriteLineAsync(IndependentCode.GetBufferWrite(parameter.Names.Name));
            }
        }

        await writer.WriteLineAsync();
        await writer.WriteLineAsync($"const {procedures.Names.EnumType} {Variables.Procedure} = {procedure.Names.EnumValue};");
        await writer.WriteLineAsync();

        //send request
        await writer.WriteLineAsync
        (
            $"{Types.BufferReader} {Variables.ResultReader} "
            + $"= await {Methods.EndPointSendRequestAsync}("
            + $"{Variables.Procedure}, "
            + $"{Variables.ArgumentsWriter}.{Properties.BufferWriterBuffer}, "
            + $"{Parameters.CancellationToken});"
        );
        await writer.WriteLineAsync();

        //read result
        TypeNode returnType = procedure.ReturnType;
        string? bufferReadMethodName = returnType.SerializationKind.GetBufferReadMethodName();
        if (bufferReadMethodName != null)
        {
            await writer.WriteAsync($"{returnType.Names.Name} {Variables.ProcedureResult} ");
            await writer.WriteLineAsync($"= {Variables.ResultReader}.{bufferReadMethodName}();");
        }
        else
        {
            throw new NotImplementedException();
        }

        await writer.WriteLineAsync();
        await writer.WriteLineAsync($"{Methods.EndPointExitCalling}({Variables.Procedure});");
        await writer.WriteLineAsync();
        await writer.WriteLineAsync($"return {Variables.ProcedureResult};");
    }

    private static async Task WriteParametersSizeSumCalculationAsync(IndentedTextWriter writer, ParameterCollection parameters)
    {
        foreach (Parameter parameter in parameters)
        {
            string? constantSizeExpression = parameter.Type.ConstantSizeExpression;
            if (constantSizeExpression != null)
            {
                await writer.WriteLineAsync($"const int {parameter.Names.SizeVariable} = {constantSizeExpression};");
            }
            else
            {
                throw new NotImplementedException();
            }
        }

        IReadOnlyList<Parameter> constantSizeParameters = parameters.ConstantSizeParameters;
        int constantSizeParametersCount = constantSizeParameters.Count;
        if (constantSizeParametersCount > 0)
        {
            await writer.WriteLineAsync();

            await writer.WriteAsync($"const int {Variables.ConstantSizeRpcParametersSize} = ");
            await writer.WriteAsync(constantSizeParameters[0].Names.SizeVariable);
            for (int i = 1; i < constantSizeParametersCount; i++)
            {
                await writer.WriteAsync($" + {constantSizeParameters[i].Names.SizeVariable}");
            }

            await writer.WriteLineAsync(';');
        }

        await writer.WriteLineAsync();

        if (parameters.HasOnlyConstantSizeParameters)
        {
            if (constantSizeParameters.Count > 0)
            {
                await writer.WriteLineAsync($"const int {Variables.ParametersSizeSum} = {Variables.ConstantSizeRpcParametersSize};");
            }
            else
            {
                await writer.WriteLineAsync($"const int {Variables.ParametersSizeSum} = 0;");
            }
        }
        else
        {
            throw new NotImplementedException();
        }
    }

    private static async Task WriteProcedureCallParameterAsync(TextWriter writer, Parameter parameter)
    {
        await writer.WriteAsync($"{parameter.Type.Names.Name} {parameter.Names.Name}");
    }
}