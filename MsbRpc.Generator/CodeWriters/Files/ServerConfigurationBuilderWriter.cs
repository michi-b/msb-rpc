using System.CodeDom.Compiler;
using MsbRpc.Generator.CodeWriters.Files.Base;
using MsbRpc.Generator.GenerationTree;
using static MsbRpc.Generator.Utility.Names;

namespace MsbRpc.Generator.CodeWriters.Files;

internal class ServerConfigurationBuilderWriter : ConfigurationBuilderWriter
{
    private const string EndPointRegistryPostFix = "EndPointRegistry";

    private readonly string _endPointRegistryLoggingName;

    private readonly ServerNode _server;

    protected override string BaseClass => Types.ServerConfigurationBuilder;

    public ServerConfigurationBuilderWriter(ServerNode server) : base(server.Contract, $"{server.Name}{ConfigurationBuilderPostfix}")
    {
        _server = server;
        _endPointRegistryLoggingName = server.Name + EndPointRegistryPostFix;
    }

    protected override void WriteConstructorBody(IndentedTextWriter writer)
    {
        string listenerConfiguration = Properties.MessengerListenerConfiguration;
        string listenerName = $"{_server.Name}Listener";

        writer.WriteLine($"{listenerConfiguration}.{Properties.LoggingName} = \"{listenerName}\";");
        writer.WriteLine($"{listenerConfiguration}.{Properties.ThreadName} = \"{listenerName}\";");
        writer.WriteLine($"{Properties.EndPointRegistryConfiguration}.{Properties.LoggingName} = \"{_endPointRegistryLoggingName}\";");
        writer.WriteLine($"{Properties.EndPointConfiguration} = new {Contract.ServerEndPoint.ConfigurationBuilderFullName}();");
    }
}