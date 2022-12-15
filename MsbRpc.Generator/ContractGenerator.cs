using System.CodeDom.Compiler;
using MsbRpc.Generator.Extensions;
using MsbRpc.Generator.Info;

namespace MsbRpc.Generator;

public struct ContractGenerator
{
    private const string InterfacePrefix = "I";
    private const string ServerPostfix = "Server";
    private const string ProcedurePostfix = "Procedure";

    private readonly ContractInfo _info;

    public string ServerProcedureEnumExtensionsFileName { get; }
    public string ServerProcedureEnumFileName { get; }
    public string ServerInterfaceFileName { get; }

    private string ServerProcedureEnumExtensionsName { get; }
    private string ServerProcedureEnumName { get; }
    private string ServerInterfaceName { get; }
    private string GeneratedNamespace { get; }

    public ContractGenerator(ContractInfo info)
    {
        _info = info;

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
    }

    public string GenerateServerInterface()
    {
        using IndentedTextWriter writer = CreateCodeWriter();

        writer.WriteLine("public interface {0}", ServerInterfaceName);

        using (writer.EncloseInBlock(false))
        {
            foreach (ProcedureInfo procedure in _info.Procedures)
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
            int lastIndex = _info.Procedures.Length - 1;
            for (int i = 0; i < _info.Procedures.Length; i++)
            {
                ProcedureInfo procedureInfo = _info.Procedures[i];

                writer.Write("{0} = {1}", procedureInfo.Name, i);

                if (i < lastIndex)
                {
                    writer.WriteCommaDelimiter();
                }

                writer.WriteLine();
            }
        }

        return writer.GetResult();
    }

    public string GenerateServerProcedureEnumExtensions()
    {
        using IndentedTextWriter writer = CreateCodeWriter();

        writer.WriteLine("public static class {0}", ServerProcedureEnumExtensionsName);

        using (writer.EncloseInBlock(false))
        {
            const string procedureParameterName = "procedure";
            writer.WriteLine
            (
                "public static string GetProcedureName(this {0} {1})",
                ServerProcedureEnumName,
                procedureParameterName
            );
            using (writer.EncloseInBlock())
            {
                writer.WriteLine("return procedure switch");
                using (writer.EncloseInBlock())
                {
                    foreach (ProcedureInfo procedure in _info.Procedures)
                    {
                        writer.WriteLine("{0} => nameof({0}),", ServerProcedureEnumName + "." + procedure.Name);
                    }

                    writer.WriteLine("_ => throw new ArgumentOutOfRangeException(nameof(procedure), procedure, null)");
                }
            }
        }

        return writer.GetResult();
    }

    private IndentedTextWriter CreateCodeWriter()
    {
        IndentedTextWriter writer = new(new StringWriter());

        writer.WriteFileHeader(GeneratedNamespace);

        return writer;
    }
}