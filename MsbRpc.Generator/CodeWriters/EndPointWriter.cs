using System.CodeDom.Compiler;
using MsbRpc.Generator.Extensions;
using MsbRpc.Generator.HelperTree;
using MsbRpc.Generator.HelperTree.Names;
using static MsbRpc.Generator.IndependentNames;

namespace MsbRpc.Generator.CodeWriters;

internal class EndPointWriter : CodeWriter
{
    private readonly string _className;
    private readonly EndPoint _endPoint;

    private readonly ProcedureCollection? _inboundProcedures;

    private readonly string _inboundProceduresEnumTypeName;
    private readonly ProcedureCollection? _outboundProcedures;
    private readonly string _outboundProceduresEnumTypeName;

    protected override string FileName { get; }

    public EndPointWriter(ContractNode contract, EndPoint endPoint, ProcedureCollection? inboundProcedures, ProcedureCollection? outboundProcedures)
        : base(contract)
    {
        _endPoint = endPoint;

        _inboundProcedures = inboundProcedures;
        _outboundProcedures = outboundProcedures;

        _inboundProceduresEnumTypeName = _inboundProcedures != null
            ? _inboundProcedures.Names.EnumType
            : Types.UndefinedProcedureEnum;

        _outboundProceduresEnumTypeName = _outboundProcedures != null
            ? _outboundProcedures.Names.EnumType
            : Types.UndefinedProcedureEnum;

        _className = $"{endPoint.Names.PascalCaseName}EndPoint";
        FileName = $"{GeneratedNamespace}.{_className}{GeneratedFilePostfix}";
    }

    protected override async ValueTask WriteAsync(IndentedTextWriter writer)
    {
        //header
        await writer.WriteAsync($"public class {_className} : {Types.EndPoint}");
        await writer.WriteLineAsync($"<{_inboundProceduresEnumTypeName}, {_outboundProceduresEnumTypeName}>");

        //body
        await writer.EnterBlockAsync();
        {
            //fields
            if (_inboundProcedures != null)
            {
                await writer.WriteLineAsync($"private readonly {_inboundProcedures.Names.InterfaceType} {_inboundProcedures.Names.InterfaceField};");
                await writer.WriteLineAsync();
            }

            await WriteConstructorAsync(writer);

            if (_outboundProcedures != null)
            {
                foreach (Procedure outboundProcedure in _outboundProcedures)
                {
                    await writer.WriteLineAsync();
                    await WriteProcedureCallAsync(writer, _outboundProcedures, outboundProcedure);
                }
            }
        }
        await writer.ExitBlockAsync(BlockAdditions.None);
    }

    private async ValueTask WriteConstructorAsync(IndentedTextWriter writer)
    {
        //header
        await writer.WriteLineAsync($"public {_className}");
        await writer.EnterParenthesesBlockAsync();
        {
            await writer.WriteLineAsync($"{Types.Messenger} {Parameters.Messenger},");

            if (_inboundProcedures != null)
            {
                await writer.WriteLineAsync($"{_inboundProcedures.Names.InterfaceType} {_inboundProcedures.Names.InterfaceParameter},");
            }

            await writer.WriteLineAsync($"{Types.LoggerFactoryInterface}? {Parameters.LoggerFactory} = null,");

            await writer.WriteLineAsync($"int {Parameters.InitialBufferSize} = {EndPointNames.DefaultBufferSizeConstant}");
        }
        await writer.ExitParenthesesBlockAsync();

        //base constructor call
        writer.Indent++;
        {
            await writer.WriteLineAsync(": base");
            await writer.EnterParenthesesBlockAsync();
            {
                await writer.WriteLineAsync($"{Parameters.Messenger},");
                await writer.WriteLineAsync($"{_endPoint.InitialDirectionEnumValue},");
                await writer.WriteLineAsync($"{Parameters.LoggerFactory} != null");
                writer.Indent++;
                {
                    await writer.WriteLineAsync($"? {Methods.CreateLogger}<{_className}>({Parameters.LoggerFactory})");
                    await writer.WriteLineAsync($": new {Types.NullLogger}<{_className}>(),");
                }
                writer.Indent--;
                await writer.WriteLineAsync($"{Parameters.InitialBufferSize}");
            }

            await writer.ExitParenthesesBlockAsync(_inboundProcedures == null ? BlockAdditions.None : BlockAdditions.NewLine);

            if (_inboundProcedures == null)
            {
                await writer.WriteLineAsync(" { }");
            }
        }
        writer.Indent--;

        if (_inboundProcedures != null)
        {
            await writer.EnterBlockAsync();
            {
                await writer.WriteLineAsync($"{_inboundProcedures.Names.InterfaceField} = {_inboundProcedures.Names.InterfaceParameter};");
            }
            await writer.ExitBlockAsync();
        }
    }

    private async ValueTask WriteProcedureCallAsync(IndentedTextWriter writer, ProcedureCollection procedures, Procedure procedure)
    {
        //header
        string returnType = procedure.ReturnType.Names.Name;
        await writer.WriteAsync($"public async {Types.Task}<{returnType}> {procedure.Names.CallMethod}");

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

        await writer.WriteLineAsync($"{Types.CancellationToken} {Parameters.CancellationToken})");

        //body
        await writer.EnterBlockAsync();
        {
            await WriteProcedureCallBodyAsync(writer, procedures, procedure, parameters);
        }
        await writer.ExitBlockAsync();
    }

    private async Task WriteProcedureCallBodyAsync
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
            + $" = {Methods.GetEndPointRequestWriter}({Variables.ParametersSizeSum});"
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
        await writer.WriteLineAsync
        (
            $"{Types.BufferReader} {Variables.ResultReader} "
            + $"= await {Methods.SendEndPointRequest}("
            + $"{Variables.Procedure}, "
            + $"{Variables.ArgumentsWriter}.{Properties.BufferWriterBuffer}, "
            + $"{Parameters.CancellationToken});"
        );
        await writer.WriteLineAsync();
        await writer.WriteLineAsync(procedure.ReadResultLine);
        await writer.WriteLineAsync();
        await writer.WriteLineAsync($"{Methods.EndPointExitCalling}({Variables.Procedure});");
        await writer.WriteLineAsync();
        await writer.WriteLineAsync($"return {Variables.Result};");
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

            await writer.WriteAsync($"const int {Variables.ConstantSizeParametersSize} = ");
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
                await writer.WriteLineAsync($"const int {Variables.ParametersSizeSum} = {Variables.ConstantSizeParametersSize};");
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