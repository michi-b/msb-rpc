using System.CodeDom.Compiler;
using MsbRpc.Generator.Extensions;
using MsbRpc.Generator.Info;

namespace MsbRpc.Generator.GenerationHelpers;

public readonly ref struct ContractGenerator
{
    private readonly List<ProcedureGenerator> _procedures;

    public ContractNames Names { get; }

    public ContractGenerator(ref ContractInfo info)
    {
        Names = new ContractNames(info.Namespace, info.InterfaceName);

        _procedures = new List<ProcedureGenerator>(info.ServerProcedures.Length);

        foreach (ProcedureInfo procedureInfo in info.ServerProcedures)
        {
            _procedures.Add(new ProcedureGenerator(procedureInfo, Names.ServerProcedure));
        }
    }

    public string GenerateServerInterface()
    {
        using IndentedTextWriter writer = CreateCodeWriter();

        writer.WriteLine("public interface {0}", Names.ServerInterface);

        using (writer.EncloseInBlock(false))
        {
            foreach (ProcedureGenerator procedure in _procedures)
            {
                procedure.GenerateInterface(writer);
            }
        }

        return writer.GetResult();
    }

    public string GenerateServerProcedureEnum()
    {
        using IndentedTextWriter writer = CreateCodeWriter();

        writer.WriteLine("public enum {0}", Names.ServerProcedure);

        using (writer.EncloseInBlock(false))
        {
            int lastIndex = _procedures.Count - 1;
            for (int i = 0; i < _procedures.Count; i++)
            {
                _procedures[i].GenerateEnumField(writer, i, i < lastIndex);
            }
        }

        return writer.GetResult();
    }

    public string GenerateServerProcedureEnumExtensions()
    {
        using IndentedTextWriter writer = CreateCodeWriter();

        writer.WriteLine("public static class {0}", Names.ServerProcedureEnumExtensions);

        const string procedureParameterName = "procedure";

        using (writer.EncloseInBlock(false))
        {
            GenerateServerProcedureGetNameExtension(writer, procedureParameterName);
            writer.WriteLine();
            GenerateServerProcedureGetInvertsDirectionExtension(writer, procedureParameterName);
        }

        return writer.GetResult();
    }

    // ReSharper disable once MemberCanBeMadeStatic.Global

    private void GenerateServerProcedureGetInvertsDirectionExtension(IndentedTextWriter writer, string procedureParameterName)
    {
        writer.WriteLine("public static bool GetInvertsDirection(this {0} {1})", Names.ServerProcedure, procedureParameterName);

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

    private void GenerateServerProcedureGetNameExtension(IndentedTextWriter writer, string procedureParameterName)
    {
        writer.WriteLine("public static string GetProcedureName(this {0} {1})", Names.ServerProcedure, procedureParameterName);
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

    public string GenerateServerEndpoint()
    {
        using IndentedTextWriter writer = CreateCodeWriter();

        writer.WriteLine("public class {0}", Names.ServerEndPoint);

        using (writer.EncloseInBlock(false))
        {
            writer.WriteLine($"private readonly {Names.ServerInterface} {Names.ServerField};");
            writer.WriteLine();
            GenerateServerEndpointConstructor(writer);
        }

        return writer.GetResult();
    }

    private void GenerateServerEndpointConstructor(IndentedTextWriter writer)
    {
        //header
        writer.WriteLine($"public {Names.ServerEndPoint}");
        using (writer.EncloseInParenthesesBlock())
        {
            writer.WriteLine(GeneralCode.MessengerParameterLine);
            writer.WriteLine($"{Names.ServerInterface} {Names.ServerParameter},");
            writer.WriteLine(GeneralCode.LoggerFactoryInterfaceParameter);
            writer.WriteLine(RpcEndPointCode.BufferSizeParameterWithDefaultLine);
        }

        //base constructor invocation
        writer.Indent++;
        {
            writer.WriteLine(": base");
            using (writer.EncloseInParenthesesBlock())
            {
                writer.WriteLine($"{GeneralNames.MessengerParameter},");
                writer.WriteLine($"{RpcEndPointNames.BufferSizeParameter},");
                writer.WriteLoggerArgumentFromFactoryParameterLines(Names.ServerEndPoint);
                writer.WriteLine(RpcEndPointNames.BufferSizeParameter);
            }

            //body
            writer.WriteLine($"=> {Names.ServerField} = {Names.ServerParameter};");
        }
        writer.Indent--;
    }

    private static void GenerateProcedureOutOfRangeCase(TextWriter writer, string procedureParameterName)
    {
        writer.WriteLine($"_ => throw new ArgumentOutOfRangeException(nameof({procedureParameterName}), {procedureParameterName}, null)");
    }

    private IndentedTextWriter CreateCodeWriter()
    {
        IndentedTextWriter writer = new(new StringWriter());

        writer.WriteFileHeader(Names.GeneratedNamespace);

        return writer;
    }
}