using System.CodeDom.Compiler;
using MsbRpc.Generator.CodeWriters.Files.Base;
using MsbRpc.Generator.GenerationTree;
using static MsbRpc.Generator.Utility.Names;

namespace MsbRpc.Generator.CodeWriters.Files;

internal class ServerConfigurationBuilderWriter : ConfigurationBuilderWriter
{
    private const string EndPointRegistryPostFix = "EndPointRegistry";

    private readonly string _endPointRegistryLoggingName;

    protected override string BaseClass => Types.ServerConfigurationBuilder;

    public ServerConfigurationBuilderWriter(ContractNode contract) : base
        (contract, $"{contract.ServerName}{ConfigurationBuilderPostfix}")
        => _endPointRegistryLoggingName = contract.ServerName + EndPointRegistryPostFix;

    protected override void WriteConstructorBody(IndentedTextWriter writer)
    {
        writer.WriteLine($"{Properties.LoggingName} = \"{Contract.ServerName}\";");
        writer.WriteLine($"{Properties.ThreadName} = \"{Contract.ServerName}\";");
        writer.WriteLine($"{Properties.EndPointRegistryConfiguration}.{Properties.LoggingName} = \"{_endPointRegistryLoggingName}\";");
        writer.WriteLine($"{Properties.EndPointConfiguration} = new {Contract.ServerEndPoint.ConfigurationBuilderFullName}();");
    }
}