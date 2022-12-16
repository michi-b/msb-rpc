using System.CodeDom.Compiler;
using MsbRpc.Generator.Extensions;
using MsbRpc.Generator.Info;

namespace MsbRpc.Generator.Generators;

public class ContractGenerator
{
    private const string InterfacePrefix = "I";
    private const string ServerPostfix = "Server";
    private const string ProcedurePostfix = "Procedure";

    private readonly List<ProcedureGenerator> _procedures;

    public string ServerProcedureEnumExtensionsFileName { get; }
    public string ServerProcedureEnumFileName { get; }
    public string ServerInterfaceFileName { get; }
    public string ServerEndpointFileName { get; }

    private string ServerProcedureEnumExtensionsName { get; }
    private string ServerInterfaceName { get; }
    private string ServerProcedureEnumName { get; }
    private string GeneratedNamespace { get; }
    private string ServerEndPointName { get; }

    public ContractGenerator(ContractInfo info)
    {
        string contractInterfaceName = info.Name;
        string namespaceName = info.Namespace;

        string contractName;
        if (contractInterfaceName.StartsWith(InterfacePrefix, StringComparison.Ordinal) && char.IsUpper(contractInterfaceName[1]))
        {
            contractName = contractInterfaceName.Substring(1);
        }
        else
        {
            contractName = contractInterfaceName;
            contractInterfaceName = InterfacePrefix + contractInterfaceName;
        }

        const string generatedFileEnding = ".g.cs";

        GeneratedNamespace = namespaceName + ".Generated";

        ServerInterfaceName = contractInterfaceName + ServerPostfix;
        ServerInterfaceFileName = $"{GeneratedNamespace}.{ServerInterfaceName}{generatedFileEnding}";

        ServerProcedureEnumName = contractName + ServerPostfix + ProcedurePostfix;
        ServerProcedureEnumFileName = $"{GeneratedNamespace}.{ServerProcedureEnumName}{generatedFileEnding}";

        ServerProcedureEnumExtensionsName = contractName + ServerPostfix + ProcedurePostfix + "Extensions";
        ServerProcedureEnumExtensionsFileName = $"{GeneratedNamespace}.{ServerProcedureEnumExtensionsName}{generatedFileEnding}";

        ServerEndPointName = contractName + ServerPostfix + "Endpoint";
        ServerEndpointFileName = $"{GeneratedNamespace}.{ServerEndPointName}{generatedFileEnding}";

        _procedures = new List<ProcedureGenerator>(info.Procedures.Length);

        foreach (ProcedureInfo procedureInfo in info.Procedures)
        {
            _procedures.Add(new ProcedureGenerator(procedureInfo, ServerProcedureEnumName));
        }
    }

    public string GenerateServerInterface()
    {
        using IndentedTextWriter writer = CreateCodeWriter();

        writer.WriteLine("public interface {0}", ServerInterfaceName);

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

        writer.WriteLine("public enum {0}", ServerProcedureEnumName);

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

        writer.WriteLine("public static class {0}", ServerProcedureEnumExtensionsName);

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
    public string GenerateServerEndpoint()
    {
        //todo: implements
        // ReSharper disable once ArrangeMethodOrOperatorBody
        return string.Empty;
    }

    private void GenerateServerProcedureGetInvertsDirectionExtension(IndentedTextWriter writer, string procedureParameterName)
    {
        writer.WriteLine("public static bool GetInvertsDirection(this {0} {1})", ServerProcedureEnumName, procedureParameterName);

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
        writer.WriteLine("public static string GetProcedureName(this {0} {1})", ServerProcedureEnumName, procedureParameterName);
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

    private static void GenerateProcedureOutOfRangeCase(TextWriter writer, string procedureParameterName)
    {
        writer.WriteLine($"_ => throw new ArgumentOutOfRangeException(nameof({procedureParameterName}), {procedureParameterName}, null)");
    }

    private IndentedTextWriter CreateCodeWriter()
    {
        IndentedTextWriter writer = new(new StringWriter());

        writer.WriteFileHeader(GeneratedNamespace);

        return writer;
    }
}