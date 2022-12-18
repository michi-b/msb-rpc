using System.CodeDom.Compiler;
using MsbRpc.EndPoints;
using MsbRpc.Generator.Extensions;
using MsbRpc.Generator.GenerationHelpers.Code;
using MsbRpc.Generator.GenerationHelpers.Names;
using MsbRpc.Generator.Info;

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
            ? info.Procedures.Select(p => new ProcedureGenerator(p, names.ProcedureEnum)).ToArray()
            : Array.Empty<ProcedureGenerator>();
    }

    public void GenerateInterface(IndentedTextWriter writer)
    {
        writer.WriteLine("public interface {0}", _names.Interface);

        using (writer.EncloseInBlock(false))
        {
            foreach (ProcedureGenerator procedure in _procedures)
            {
                procedure.GenerateInterfaceMethod(writer);
            }
        }
    }

    public void GenerateProcedureEnum(IndentedTextWriter writer)
    {
        writer.WriteLine("public enum {0}", _names.ProcedureEnum);

        using (writer.EncloseInBlock(false))
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

        using (writer.EncloseInBlock(false))
        {
            GenerateProcedureEnumGetNameExtension(writer, procedureParameterName);
            writer.WriteLine();
            GenerateProcedureEnumGetInvertsDirectionExtension(writer, procedureParameterName);
        }
    }

    public void GenerateEndPoint(IndentedTextWriter writer)
    {
        writer.Write($"public class {_names.EndPointType} : {EndPointNames.Types.EndPointBaseType}");
        writer.WriteLine($"<{GetInboundProcedureName()}, {GetOutboundProcedureName()}>");

        using (writer.EncloseInBlock(false))
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
        }
    }

    private string GetOutboundProcedureName() => Remote!.GetInboundProcedureName();

    private string GetInboundProcedureName() => HasInboundProcedures ? _names.ProcedureEnum : EndPointNames.Types.UndefinedProcedureEnum;

    private void GenerateProcedureEnumGetInvertsDirectionExtension(IndentedTextWriter writer, string procedureParameterName)
    {
        writer.WriteLine("public static bool GetInvertsDirection(this {0} {1})", _names.ProcedureEnum, procedureParameterName);

        using (writer.EncloseInBlock())
        {
            writer.WriteLine("return {0} switch", procedureParameterName);

            using (writer.EncloseInBlock())
            {
                foreach (ProcedureGenerator procedure in _procedures)
                {
                    procedure.GenerateGetInvertsDirectionCase(writer);
                }

                GenerateProcedureOutOfRangeCase(writer, procedureParameterName);
            }
        }
    }

    private void GenerateProcedureEnumGetNameExtension(IndentedTextWriter writer, string procedureParameterName)
    {
        writer.WriteLine("public static string GetProcedureName(this {0} {1})", _names.ProcedureEnum, procedureParameterName);
        using (writer.EncloseInBlock())
        {
            writer.WriteLine("return procedure switch");
            using (writer.EncloseInBlock())
            {
                foreach (ProcedureGenerator procedure in _procedures)
                {
                    procedure.GenerateEnumToNameCase(writer);
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
                writer.WriteLine($"{EndPointNames.Parameters.BufferSize},");
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
        writer.WriteLine($"? {GeneralNames.Methods.CreateLogger}.CreateLogger<{categoryTypeName}>({GeneralNames.Parameters.LoggerFactory})");
        writer.WriteLine($": new {GeneralNames.Types.NullLogger}<{categoryTypeName}>(),");
        writer.Indent--;
    }

    private static void GenerateProcedureOutOfRangeCase(TextWriter writer, string procedureParameterName)
    {
        writer.WriteLine($"_ => throw new ArgumentOutOfRangeException(nameof({procedureParameterName}), {procedureParameterName}, null)");
    }
}