using System.CodeDom.Compiler;
using MsbRpc.Generator.Extensions;
using MsbRpc.Generator.GenerationHelpers.Code;
using MsbRpc.Generator.GenerationHelpers.Names;
using MsbRpc.Generator.Info;

namespace MsbRpc.Generator.GenerationHelpers;

public class EndPointGenerator
{
    private readonly EndPointNames _names;
    private readonly ProcedureGenerator[] _procedures;

    public EndPointGenerator? Remote
    {
        // ReSharper disable once UnusedAutoPropertyAccessor.Local
        // todo: use in code generation
        private get;
        set;
    }

    public bool HasProcedures { get; }

    public EndPointGenerator(EndPointInfo info, EndPointNames names)
    {
        _names = names;

        HasProcedures = info.Procedures.Length > 0;

        _procedures = HasProcedures
            ? info.Procedures.Select(p => new ProcedureGenerator(p, names.ProcedureEnum)).ToArray()
            : Array.Empty<ProcedureGenerator>();
    }

    public void GenerateInterface(IndentedTextWriter writer)
    {
        writer.WriteLine("public interface {0}", _names.InterfaceType);

        using (writer.EncloseInBlock(false))
        {
            foreach (ProcedureGenerator procedure in _procedures)
            {
                procedure.GenerateInterface(writer);
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
        writer.WriteLine("public class {0}", _names.EndPointType);

        using (writer.EncloseInBlock(false))
        {
            writer.WriteLine($"private readonly {_names.InterfaceType} {_names.InterfaceField};");
            writer.WriteLine();
            GenerateEndpointConstructor(writer);
        }
    }

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
            writer.WriteLine($"{_names.InterfaceType} {_names.InterfaceParameter},");
            writer.WriteLine(GeneralCode.LoggerFactoryInterfaceParameter);
            writer.WriteLine(EndPointCode.BufferSizeParameterWithDefaultLine);
        }

        //base constructor invocation
        writer.Indent++;
        {
            writer.WriteLine(": base");
            using (writer.EncloseInParenthesesBlock())
            {
                writer.WriteLine($"{GeneralNames.MessengerParameter},");
                writer.WriteLine($"{EndPointNames.BufferSizeParameter},");
                GenerateLoggerArgumentCreation(writer, _names.EndPointType);
                writer.WriteLine(EndPointNames.BufferSizeParameter);
            }

            //body
            writer.WriteLine($"=> {_names.InterfaceField} = {_names.InterfaceParameter};");
        }
        writer.Indent--;
    }

    private static void GenerateLoggerArgumentCreation(IndentedTextWriter writer, string categoryTypeName)
    {
        writer.WriteLine($"{GeneralNames.LoggerFactoryParameter} != null");
        writer.Indent++;
        writer.WriteLine($"? {GeneralNames.CreateLoggerMethod}.CreateLogger<{categoryTypeName}>({GeneralNames.LoggerFactoryParameter})");
        writer.WriteLine($": new {GeneralNames.NullLoggerType}<{categoryTypeName}>(),");
        writer.Indent--;
    }

    private static void GenerateProcedureOutOfRangeCase(TextWriter writer, string procedureParameterName)
    {
        writer.WriteLine($"_ => throw new ArgumentOutOfRangeException(nameof({procedureParameterName}), {procedureParameterName}, null)");
    }
}