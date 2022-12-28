using System.CodeDom.Compiler;
using System.Threading.Tasks;
using MsbRpc.Generator.Extensions;
using MsbRpc.Generator.GenerationTree;
using MsbRpc.Generator.GenerationTree.Names;
using static MsbRpc.Generator.IndependentNames;

namespace MsbRpc.Generator.CodeWriters.Files;

internal class EndPointFileWriter : CodeFileWriter
{
    private readonly string _className;
    private readonly EndPoint _endPoint;

    private readonly ProcedureCollection? _inboundProcedures;

    private readonly string _inboundProceduresEnumTypeName;
    private readonly ProcedureCollection? _outboundProcedures;
    private readonly string _outboundProceduresEnumTypeName;

    protected override string FileName { get; }

    public EndPointFileWriter
    (
        ContractNode contract,
        EndPoint endPoint,
        ProcedureCollection? inboundProcedures,
        ProcedureCollection? outboundProcedures
    ) : base(contract)
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
            await WriteConstructorAsync(writer);

            if (_outboundProcedures != null)
            {
                await OutboundRpcWriter.WriteAsync(writer, _outboundProcedures);
            }

            if (_inboundProcedures != null)
            {
                await WriteProcedureEnumUtilityOverrides(writer, _inboundProceduresEnumTypeName);
            }

            if (_outboundProcedures != null)
            {
                await WriteProcedureEnumUtilityOverrides(writer, _outboundProceduresEnumTypeName);
            }

            //write inbound procedures
            if (_inboundProcedures != null)
            {
                await WriteRpcReceivingAsync(writer, _inboundProcedures);
            }
        }
        await writer.ExitBlockAsync(BlockAdditions.None);
    }

    private async Task WriteProcedureEnumUtilityOverrides(IndentedTextWriter writer, string enumTypeName)
    {
        await writer.WriteLineAsync();
        await writer.WriteAsync($"protected override string {Methods.EndpointGetProcedureName}({enumTypeName} {Parameters.Procedure})");
        await writer.WriteLineAsync($" => {Parameters.Procedure}.{Methods.GetNameProcedureEnumExtension}();");
        await writer.WriteLineAsync();
        await writer.WriteAsync($"protected override bool {Methods.EndPointGetProcedureInvertsDirection}({enumTypeName} {Parameters.Procedure})");
        await writer.WriteLineAsync($" => {Parameters.Procedure}.{Methods.GetInvertsDirectionProcedureExtension}();");
    }

    private async ValueTask WriteConstructorAsync(IndentedTextWriter writer)
    {
        //header
        await writer.WriteLineAsync($"public {_className}");
        await writer.EnterParenthesesBlockAsync();
        {
            await writer.WriteLineAsync($"{Types.Messenger} {Parameters.Messenger},");
            await writer.WriteLineAsync($"{Types.LoggerFactoryInterface}? {Parameters.LoggerFactory} = null,");
            await writer.WriteLineAsync($"int {Parameters.InitialBufferSize} = {EndPointNames.DefaultBufferSizeConstant}");
        }
        await writer.ExitParenthesesBlockAsync(BlockAdditions.None);

        //base constructor call
        writer.Indent++;
        {
            await writer.WriteLineAsync(" : base");
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

    private async Task WriteRpcReceivingAsync(IndentedTextWriter writer, ProcedureCollection procedures)
    {
        await writer.WriteLineAsync();

        const string implementation = Parameters.EndPointImplementation;
        const string listen = Methods.EndPointListen;
        const string listenReturnCode = Types.MessengerListenReturnCode;
        const string createResolver = Methods.EndPointCreateResolver;
        const string resolver = Types.LocalEndPointResolver;
        const string procedure = Parameters.Procedure;
        const string arguments = Parameters.ArgumentsBufferReader;

        string procedureType = procedures.Names.EnumType;
        string implementationInterface = _endPoint.Names.ImplementationInterface;

        string implementationParameterDeclaration = $"{implementationInterface} {implementation}";
        string resolverInterface = $"{Types.RpcResolverInterface}<{procedureType}>";

        //listen override
        await writer.WriteAsync($"public {listenReturnCode} {listen}({implementationParameterDeclaration})");
        await writer.WriteLineAsync($" => base.{listen}({createResolver}({implementation}));");
        await writer.WriteLineAsync();

        //create resolver
        await writer.WriteAsync($"public {resolver} {createResolver}({implementationParameterDeclaration})");
        await writer.WriteLineAsync($" => new {resolver}(this, {implementation});");
        await writer.WriteLineAsync();

        //resolver class
        await writer.WriteLineAsync($"public class {resolver} : {resolverInterface}");
        await writer.EnterBlockAsync();
        {
            //fields
            await writer.WriteLineAsync($"private readonly {_className} {Fields.RpcResolverEndPoint};");
            await writer.WriteLineAsync($"private readonly {implementationInterface} {Fields.RpcImplementation};");
            await writer.WriteLineAsync();

            //constructor
            await writer.WriteLineAsync($"public {resolver}({_className} {Parameters.RpcEndPoint}, {implementationParameterDeclaration})");
            await writer.EnterBlockAsync();
            {
                await writer.WriteLineAsync($"{Fields.RpcResolverEndPoint} = {Parameters.RpcEndPoint};");
                await writer.WriteLineAsync($"{Fields.RpcImplementation} = {implementation};");
            }
            await writer.ExitBlockAsync();
            await writer.WriteLineAsync();

            //execute header
            await writer.WriteAsync($"{Types.BufferWriter} {resolverInterface}.{Methods.RpcResolverExecute}");
            await writer.WriteLineAsync($"({procedureType} {procedure}, {Types.BufferReader} {arguments})");
            //execute body
            await writer.EnterBlockAsync();
            {
                await writer.WriteLineAsync($"return {procedure} switch");
                await writer.EnterBlockAsync();
                {
                    foreach (Procedure currentProcedure in procedures)
                    {
                        string switchCase =
                            $"{currentProcedure.Names.EnumValue} => {currentProcedure.Names.Name}({Parameters.ArgumentsBufferReader}),";
                        await writer.WriteLineAsync(switchCase);
                    }

                    await writer.WriteLineAsync($"_ => throw new {Types.ArgumentOutOfRangeException}(nameof({procedure}), {procedure}, null)");
                }
                await writer.ExitBlockAsync(BlockAdditions.SemicolonAndNewline);
            }
            await writer.ExitBlockAsync();

            await InboundRpcWriter.WriteAsync(writer, procedures);
        }
        await writer.ExitBlockAsync();
    }
}