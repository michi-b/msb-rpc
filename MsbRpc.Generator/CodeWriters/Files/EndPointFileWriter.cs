using System.CodeDom.Compiler;
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
        FileName = $"{_className}{GeneratedFilePostfix}";
    }

    protected override void Write(IndentedTextWriter writer)
    {
        //header
        writer.Write($"public class {_className} : {Types.EndPoint}");
        writer.WriteLine($"<{_inboundProceduresEnumTypeName}, {_outboundProceduresEnumTypeName}>");

        //body
        using (writer.InBlock())
        {
            WriteConstructor(writer);

            if (_outboundProcedures != null)
            {
                OutboundRpcWriter.Write(writer, _outboundProcedures);
            }

            if (_inboundProcedures != null)
            {
                WriteProcedureEnumUtilityOverrides(writer, _inboundProceduresEnumTypeName);
            }

            if (_outboundProcedures != null)
            {
                WriteProcedureEnumUtilityOverrides(writer, _outboundProceduresEnumTypeName);
            }

            //write inbound procedures
            if (_inboundProcedures != null)
            {
                WriteRpcReceiving(writer, _inboundProcedures);
            }
        }
    }

    private void WriteProcedureEnumUtilityOverrides(IndentedTextWriter writer, string enumTypeName)
    {
        writer.WriteLine();
        writer.Write($"protected override string {Methods.EndpointGetProcedureName}({enumTypeName} {Parameters.Procedure})");
        writer.WriteLine($" => {Parameters.Procedure}.{Methods.GetNameProcedureEnumExtension}();");
        writer.WriteLine();
        writer.Write($"protected override bool {Methods.EndPointGetProcedureInvertsDirection}({enumTypeName} {Parameters.Procedure})");
        writer.WriteLine($" => {Parameters.Procedure}.{Methods.GetInvertsDirectionProcedureExtension}();");
    }

    private void WriteConstructor(IndentedTextWriter writer)
    {
        //header
        writer.WriteLine($"public {_className}");
        using (writer.InParenthesesBlock())
        {
            writer.WriteLine($"{Types.Messenger} {Parameters.Messenger},");
            writer.WriteLine($"{Types.LoggerFactoryInterface}? {Parameters.LoggerFactory} = null,");
            writer.WriteLine($"int {Parameters.InitialBufferSize} = {EndPointNames.DefaultBufferSizeConstant}");
        }

        //base constructor call
        writer.Indent++;
        {
            writer.WriteLine(" : base");
            using (writer.InParenthesesBlock(Appendix.None))
            {
                writer.WriteLine($"{Parameters.Messenger},");
                writer.WriteLine($"{_endPoint.InitialDirectionEnumValue},");
                writer.WriteLine($"{Parameters.LoggerFactory} != null");
                writer.Indent++;
                {
                    writer.WriteLine($"? {Methods.CreateLogger}<{_className}>({Parameters.LoggerFactory})");
                    writer.WriteLine($": new {Types.NullLogger}<{_className}>(),");
                }
                writer.Indent--;
                writer.WriteLine($"{Parameters.InitialBufferSize}");
            }

            writer.WriteLine(" { }");
        }
        writer.Indent--;
    }

    private void WriteRpcReceiving(IndentedTextWriter writer, ProcedureCollection procedures)
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
        string implementationInterface = _endPoint.Names.ImplementationInterface;

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
            writer.WriteLine($"private readonly {_className} {Fields.RpcResolverEndPoint};");
            writer.WriteLine($"private readonly {implementationInterface} {Fields.RpcImplementation};");
            writer.WriteLine();

            //constructor
            writer.WriteLine($"public {resolver}({_className} {Parameters.RpcEndPoint}, {implementationParameterDeclaration})");
            using (writer.InBlock())
            {
                writer.WriteLine($"this.{Fields.RpcResolverEndPoint} = {Parameters.RpcEndPoint};");
                writer.WriteLine($"this.{Fields.RpcImplementation} = {implementation};");
            }

            writer.WriteLine();

            //execute header
            writer.Write($"{Types.BufferWriter} {resolverInterface}.{Methods.RpcResolverExecute}");
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

            InboundRpcWriter.Write(writer, procedures);
        }
    }
}