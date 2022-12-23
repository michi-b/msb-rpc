using System.CodeDom.Compiler;
using MsbRpc.EndPoints;
using MsbRpc.Generator.Extensions;
using MsbRpc.Generator.GenerationHelpers.Code;
using MsbRpc.Generator.GenerationHelpers.Names;
using MsbRpc.Generator.Info;
using static MsbRpc.Generator.GenerationHelpers.Names.EndPointNames;

namespace MsbRpc.Generator.GenerationHelpers;

public class EndPointGenerator
{
    private readonly EndPointDirection _initialDirection;
    private readonly EndPointNames _names;
    private readonly ProcedureGenerator[] _procedures;

    public EndPointGenerator? Remote
    {
        // ReSharper disable once UnusedAutoPropertyAccessor.Local
        // todo: use in code generation
        private get;
        set;
    }

    public bool HasInboundProcedures { get; }
    private bool HasOutboundProcedures => Remote!.HasInboundProcedures;

    private ProcedureGenerator[] OutBoundProcedures => Remote!._procedures;

    public EndPointGenerator(EndPointInfo info, EndPointNames names, EndPointDirection initialDirection)
    {
        _names = names;
        _initialDirection = initialDirection;

        HasInboundProcedures = info.Procedures.Length > 0;

        _procedures = HasInboundProcedures
            ? info.Procedures.Select(p => new ProcedureGenerator(p, names.InboundProcedureEnum)).ToArray()
            : Array.Empty<ProcedureGenerator>();

        InboundProcedureName = HasInboundProcedures ? _names.InboundProcedureEnum : Types.UndefinedProcedureEnum;
    }

    private string GetOutboundProcedureName => Remote!.InboundProcedureName;

    private string InboundProcedureName { get; }

    public void GenerateInterface(IndentedTextWriter writer)
    {
        writer.WriteLine("public interface {0}", _names.Interface);

        using (writer.EncloseInBlock(BlockOptions.None))
        {
            foreach (ProcedureGenerator procedure in _procedures)
            {
                procedure.GenerateInterfaceMethod(writer);
            }
        }
    }

    public void GenerateProcedureEnum(IndentedTextWriter writer)
    {
        writer.WriteLine("public enum {0}", _names.InboundProcedureEnum);

        using (writer.EncloseInBlock(BlockOptions.None))
        {
            int proceduresCount = _procedures.Length;
            int lastIndex = proceduresCount - 1;
            for (int i = 0; i < proceduresCount; i++)
            {
                _procedures[i].GenerateEnumField(writer, i, i < lastIndex);
            }
        }
    }

    public void GenerateProcedureEnumExtensions(IndentedTextWriter writer)
    {
        writer.WriteLine("public static class {0}", _names.ProcedureEnumExtensions);

        const string procedureParameterName = "procedure";

        using (writer.EncloseInBlock(BlockOptions.None))
        {
            GenerateProcedureEnumGetNameExtension(writer, procedureParameterName);
            writer.WriteLine();
            GenerateProcedureEnumGetInvertsDirectionExtension(writer, procedureParameterName);
        }
    }

    public void GenerateEndPoint(IndentedTextWriter writer)
    {
        writer.Write($"public class {_names.EndPointType} : {EndPointNames.Types.EndPointBaseType}");
        writer.WriteLine($"<{InboundProcedureName}, {GetOutboundProcedureName}>");

        using (writer.EncloseInBlock(BlockOptions.None))
        {
            if (HasInboundProcedures)
            {
                writer.WriteLine($"private readonly {_names.Interface} {_names.InterfaceField};\n");
            }

            GenerateEndpointConstructor(writer);

            foreach (ProcedureGenerator outBoundProcedure in OutBoundProcedures)
            {
                writer.WriteLine();
                outBoundProcedure.GenerateRequestMethod(writer);
            }

            if (HasInboundProcedures)
            {
                writer.WriteLine();
                GenerateHandleRequest(writer);
            }
        }
    }

    private void GenerateHandleRequest(IndentedTextWriter writer)
    {
        string procedureParameterName = _names.InboundProcedureEnumParameter;
        
        //header
        writer.WriteLine($"protected override {GeneralNames.Types.BufferWriter} HandleRequest");
        using (writer.EncloseInParenthesesBlock())
        {
            writer.WriteLine($"{InboundProcedureName} {procedureParameterName},");
            writer.WriteLine($"{GeneralNames.Types.BufferReader} {Parameters.ArgumentsBufferReader}");
        }

        //body
        using (writer.EncloseInBlock(BlockOptions.None))
        {
            writer.WriteLine($"return {procedureParameterName} switch");
            
              
        }
        
    }

    private void GenerateProcedureEnumGetNameExtension(IndentedTextWriter writer, string procedureParameterName)
    {
        writer.WriteLine("public static string GetProcedureName(this {0} {1})", _names.InboundProcedureEnum, procedureParameterName);
        using (writer.EncloseInBlock())
        {
            writer.WriteLine("return procedure switch");
            using (writer.EncloseInBlock(BlockOptions.WithTrailingSemicolonAndNewline))
            {
                foreach (ProcedureGenerator procedure in _procedures)
                {
                    procedure.GenerateEnumToNameCase(writer);
                }

                GenerateProcedureOutOfRangeCase(writer, procedureParameterName);
            }
        }
    }

    private void GenerateProcedureEnumGetInvertsDirectionExtension(IndentedTextWriter writer, string procedureParameterName)
    {
        writer.WriteLine("public static bool GetInvertsDirection(this {0} {1})", _names.InboundProcedureEnum, procedureParameterName);

        using (writer.EncloseInBlock())
        {
            writer.WriteLine("return {0} switch", procedureParameterName);

            using (writer.EncloseInBlock(BlockOptions.WithTrailingSemicolonAndNewline))
            {
                foreach (ProcedureGenerator procedure in _procedures)
                {
                    procedure.GenerateGetInvertsDirectionCase(writer);
                }

                GenerateProcedureOutOfRangeCase(writer, procedureParameterName);
            }
        }
    }

    private void GenerateEndpointConstructor(IndentedTextWriter writer)
    {
        //header
        writer.WriteLine($"public {_names.EndPointType}");
        using (writer.EncloseInParenthesesBlock())
        {
            writer.WriteLine(GeneralCode.MessengerParameterLine);
            if (HasInboundProcedures)
            {
                writer.WriteLine($"{_names.Interface} {_names.InterfaceParameter},");
            }

            writer.WriteLine(GeneralCode.LoggerFactoryInterfaceParameter);
            writer.WriteLine(EndPointCode.BufferSizeParameterWithDefaultLine);
        }

        //base constructor invocation
        writer.Indent++;
        {
            writer.WriteLine(": base");
            using (writer.EncloseInParenthesesBlock(false))
            {
                writer.WriteLine($"{GeneralNames.Parameters.Messenger},");
                writer.WriteLine(EndPointCode.GetInitialDirectionArgumentLine(_initialDirection));
                GenerateLoggerArgumentCreation(writer, _names.EndPointType);
                writer.WriteLine(EndPointNames.Parameters.BufferSize);
            }

            //body
            writer.WriteLine
            (
                HasInboundProcedures
                    ? $" => {_names.InterfaceField} = {_names.InterfaceParameter};"
                    : " { }"
            );
        }
        writer.Indent--;
    }

    private static void GenerateLoggerArgumentCreation(IndentedTextWriter writer, string categoryTypeName)
    {
        writer.WriteLine($"{GeneralNames.Parameters.LoggerFactory} != null");
        writer.Indent++;
        writer.WriteLine($"? {GeneralNames.Methods.CreateLogger}<{categoryTypeName}>({GeneralNames.Parameters.LoggerFactory})");
        writer.WriteLine($": new {GeneralNames.Types.NullLogger}<{categoryTypeName}>(),");
        writer.Indent--;
    }

    private static void GenerateProcedureOutOfRangeCase(TextWriter writer, string procedureParameterName)
    {
        writer.WriteLine
            ($"_ => throw new {GeneralNames.Types.ArgumentOutOfRangeException}(nameof({procedureParameterName}), {procedureParameterName}, null)");
    }
}