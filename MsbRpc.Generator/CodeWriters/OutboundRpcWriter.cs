using System.CodeDom.Compiler;
using MsbRpc.Generator.Extensions;
using MsbRpc.Generator.HelperTree;

namespace MsbRpc.Generator.CodeWriters;

public static class OutboundRpcWriter
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
        await writer.WriteAsync($"public async {IndependentNames.Types.Task}<{returnType}> {procedure.Names.CallMethod}");

        procedure.TryGetParameters(out ParameterCollection? parameters);

        await writer.WriteAsync('(');
        if (parameters != null)
        {
            foreach (Parameter t in parameters)
            {
                await WriteProcedureCallParameterAsync(writer, t);
                await writer.WriteAsync(", ");
            }
        }

        await writer.WriteLineAsync($"{IndependentNames.Types.CancellationToken} {IndependentNames.Parameters.CancellationToken})");

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
        await writer.WriteLineAsync($"{IndependentNames.Methods.EndPointEnterCalling}();");
        await writer.WriteLineAsync();
        if (parameters != null)
        {
            await WriteParametersSizeSumCalculationAsync(writer, parameters);
        }

        await writer.WriteLineAsync();
        await writer.WriteLineAsync
        (
            $"{IndependentNames.Types.BufferWriter} {IndependentNames.Variables.ArgumentsWriter}"
            + $" = {IndependentNames.Methods.GetEndPointRequestWriter}({IndependentNames.Variables.ParametersSizeSum});"
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
        await writer.WriteLineAsync($"const {procedures.Names.EnumType} {IndependentNames.Variables.Procedure} = {procedure.Names.EnumValue};");
        await writer.WriteLineAsync();
        await writer.WriteLineAsync
        (
            $"{IndependentNames.Types.BufferReader} {IndependentNames.Variables.ResultReader} "
            + $"= await {IndependentNames.Methods.SendEndPointRequest}("
            + $"{IndependentNames.Variables.Procedure}, "
            + $"{IndependentNames.Variables.ArgumentsWriter}.{IndependentNames.Properties.BufferWriterBuffer}, "
            + $"{IndependentNames.Parameters.CancellationToken});"
        );
        await writer.WriteLineAsync();
        await writer.WriteLineAsync(procedure.ReadResultLine);
        await writer.WriteLineAsync();
        await writer.WriteLineAsync($"{IndependentNames.Methods.EndPointExitCalling}({IndependentNames.Variables.Procedure});");
        await writer.WriteLineAsync();
        await writer.WriteLineAsync($"return {IndependentNames.Variables.Result};");
    }

    private static async Task WriteParametersSizeSumCalculationAsync(IndentedTextWriter writer, ParameterCollection parameters)
    {
        foreach (Parameter parameter in parameters)
        {
            if (parameter.Type.TryGetConstantSizeExpression(out string? constantSizeExpression) && constantSizeExpression != null)
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

            await writer.WriteAsync($"const int {IndependentNames.Variables.ConstantSizeParametersSize} = ");
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
                await writer.WriteLineAsync
                    ($"const int {IndependentNames.Variables.ParametersSizeSum} = {IndependentNames.Variables.ConstantSizeParametersSize};");
            }
            else
            {
                await writer.WriteLineAsync($"const int {IndependentNames.Variables.ParametersSizeSum} = 0;");
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