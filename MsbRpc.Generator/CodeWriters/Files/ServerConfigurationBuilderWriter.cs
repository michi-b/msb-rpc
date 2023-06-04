using System.CodeDom.Compiler;
using MsbRpc.Generator.CodeWriters.Files.Base;
using MsbRpc.Generator.GenerationTree;
using static MsbRpc.Generator.Utility.Names;

namespace MsbRpc.Generator.CodeWriters.Files;

internal class ServerConfigurationBuilderWriter : ConfigurationBuilderWriter
{
    protected override string BaseClass => Types.ServerConfigurationBuilder;

    //todo: proper constructor
    public ServerConfigurationBuilderWriter(ContractNode contract, string configurationTargetClassName) : base(contract, configurationTargetClassName) { }
    // public ServerConfigurationBuilderWriter(ContractNode contract, EndPointConfigurationBuilderWriter serverEndPointConfigurationBuilderWriter) : base(contract) { }

    protected override void WriteConstructorBody(IndentedTextWriter writer)
    {
        writer.WriteLine($"{Fields.LoggingName} = \"{ClassName}\";");
        writer.WriteLine($"{Fields.ThreadName} = \"{ClassName}\";");
    }
}