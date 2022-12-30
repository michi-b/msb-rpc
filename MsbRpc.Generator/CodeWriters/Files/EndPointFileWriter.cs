using System.CodeDom.Compiler;
using System.IO;
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
                RpcInvocationWriter.Write(writer, _outboundProcedures);
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
                RpcResolverWriter.Write(writer, _inboundProcedures, _endPoint, _className);
            }
        }
    }

    private static void WriteProcedureEnumUtilityOverrides(TextWriter writer, string enumTypeName)
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
}