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

    protected override async ValueTask Write(IndentedTextWriter writer)
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

            await WriteConstructor(writer);

            if (_outboundProcedures != null)
            {
                foreach (Procedure outboundProcedure in _outboundProcedures)
                {
                    await writer.WriteLineAsync();
                    await WriteProcedureCall(writer, outboundProcedure);
                }
            }
        }
        await writer.ExitBlockAsync(BlockAdditions.None);
    }

    private async ValueTask WriteConstructor(IndentedTextWriter writer)
    {
        //header
        await writer.WriteLineAsync($"public {_className}");
        await writer.EnterParenthesesBlockAsync();
        {
            await writer.WriteLineAsync($"{Types.Messenger} {Parameters.Messenger},");

            if (_inboundProcedures != null)
            {
                await writer.WriteLineAsync($"{_inboundProcedures.Names.InterfaceType} {_inboundProcedures.Names.InterfaceField},");
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
            await writer.ExitParenthesesBlockAsync(BlockAdditions.None);
            await writer.WriteLineAsync(" { }");
        }
        writer.Indent--;
    }

    private async ValueTask WriteProcedureCall(IndentedTextWriter writer, Procedure procedure)
    {
        //header
        string returnType = procedure.ReturnType.Names.FullName;
        await writer.WriteAsync($"public async {Types.Task}<{returnType}> {procedure.Names.CallMethod}");

        await writer.WriteAsync('(');
        if (procedure.TryGetParameters(out Parameter[]? parameters) && parameters != null)
        {
            await WriteProcedureCallParameter(writer, parameters[0]);
            for (int i = 1; i < parameters.Length; i++)
            {
                await writer.WriteAsync(", ");
                await WriteProcedureCallParameter(writer, parameters[i]);
            }
        }

        await writer.WriteLineAsync(')');
        await writer.WriteLineAsync("{ }");
    }

    private async Task WriteProcedureCallParameter(IndentedTextWriter writer, Parameter parameter)
    {
        await writer.WriteAsync($"{parameter.Type.Names.FullName} {parameter.Names.CamelCaseName}");
    }
}