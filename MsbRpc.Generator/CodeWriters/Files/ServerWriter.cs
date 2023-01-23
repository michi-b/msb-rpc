using System.CodeDom.Compiler;
using MsbRpc.Generator.CodeWriters.Utility;
using MsbRpc.Generator.GenerationTree;
using static MsbRpc.Generator.CodeWriters.Utility.IndependentNames;

namespace MsbRpc.Generator.CodeWriters.Files;

internal class ServerWriter : CodeFileWriter
{
    private readonly ContractNode _contract;
    private readonly string _contractImplementationFactoryType;
    private readonly EndPointNode _endPoint;
    private readonly string _endPointConfigurationType;
    private readonly ProcedureCollectionNode _procedures;
    private readonly ServerNode _server;

    protected override string FileName { get; }

    public ServerWriter(ServerNode server) : base(server.Contract)
    {
        _contract = server.Contract;
        _server = server;
        _contractImplementationFactoryType = $"{Types.Func}<{_contract.InterfaceType}>";
        _endPoint = server.EndPoint;
        _procedures = _contract.Procedures;
        _endPointConfigurationType = _server.EndPointConfigurationTypeFullName;
        FileName = $"{server.Name}{GeneratedFilePostfix}";
    }

    protected override void Write(IndentedTextWriter writer)
    {
        WriteClassHeader(writer);

        using (writer.GetBlock(Appendix.None))
        {
            WriteFields(writer);

            writer.WriteLine();

            WriteConstructor(writer);

            writer.WriteLine();

            WriteConfiguration(writer);

            writer.WriteLine();

            WriteStartMethod(writer);

            writer.WriteLine();

            WriteCreateEndPointOverride(writer);
        }
    }

    private void WriteClassHeader(IndentedTextWriter writer)
    {
        string serverName = _server.Name;
        writer.Write($"public class {serverName} : {Types.Server}");
        writer.WriteLine($"<{serverName}, {_endPoint.Name}, {_procedures.ProcedureEnumType}, {_contract.InterfaceType}>");
    }

    private void WriteFields(IndentedTextWriter writer)
    {
        writer.WriteLine($"private readonly {_contractImplementationFactoryType} {Fields.ImplementationFactory};");
        writer.WriteLine($"private readonly {_endPointConfigurationType} {Fields.EndPointConfiguration};");
        writer.WriteLine($"private readonly {Types.LocalConfiguration} {Fields.Configuration};");
    }

    private void WriteConstructor(IndentedTextWriter writer)
    {
        writer.WriteLine($"private {_server.Name}");
        using (writer.GetParenthesesBlock(Appendix.None))
        {
            writer.WriteLine($"{_contractImplementationFactoryType} {Parameters.ContractImplementationFactory},");
            writer.WriteLine($"{Types.LocalConfiguration} {Parameters.Configuration},");
            writer.WriteLine($"{_endPointConfigurationType} {Parameters.EndPointConfiguration}");
        }

        writer.WriteLine($": base({Parameters.Configuration})");
        using (writer.GetBlock())
        {
            writer.WriteLine($"{Fields.ImplementationFactory} = {Parameters.ContractImplementationFactory};");
            writer.WriteLine($"{Fields.Configuration} = {Parameters.Configuration};");
            writer.WriteLine($"{Fields.EndPointConfiguration} = {Parameters.EndPointConfiguration};");
        }
    }

    // ReSharper disable once SuggestBaseTypeForParameter
    private static void WriteConfiguration(IndentedTextWriter writer)
    {
        writer.WriteLine($"public class {Types.LocalConfiguration} : {Types.ServerConfiguration} {{}}");
    }

    private void WriteStartMethod(IndentedTextWriter writer)
    {
        writer.WriteLine($"public static {_server.Name} {Methods.Start}");
        using (writer.GetParenthesesBlock())
        {
            writer.WriteLine($"{_contractImplementationFactoryType} {Parameters.ContractImplementationFactory},");
            writer.WriteLine($"{Types.LocalConfigurationConfigureAction}? {Parameters.ConfigureAction},");
            writer.WriteLine($"{Types.Action}<{_endPointConfigurationType}> {Parameters.EndPointConfigureAction}");
        }

        using (writer.GetBlock())
        {
            writer.WriteLine($"{Types.LocalConfiguration} {Variables.Configuration} = new {Types.LocalConfiguration}();");
            writer.WriteLine($"{Parameters.ConfigureAction}?.{Methods.Invoke}({Variables.Configuration});");
            writer.WriteLine();
            writer.WriteLine($"{_endPointConfigurationType} {Variables.EndPointConfiguration} = new {_endPointConfigurationType}();");
            writer.WriteLine($"{Parameters.EndPointConfigureAction}?.{Methods.Invoke}({Variables.EndPointConfiguration});");
            writer.WriteLine();
            writer.Write($"{_server.Name} {Variables.Server} = new {_server.Name}(");
            writer.WriteLine($"{Parameters.ContractImplementationFactory}, {Variables.Configuration}, {Variables.EndPointConfiguration});");
            writer.WriteLine($"{Variables.Server}.{Methods.Start}();");
            writer.WriteLine($"return {Variables.Server};");
        }
    }

    private void WriteCreateEndPointOverride(IndentedTextWriter writer)
    {
        writer.WriteLine($"protected override {_endPoint.Name} CreateEndPoint({Types.Messenger} {Parameters.Messenger})");
        writer.Indent++;
        writer.WriteLine($"=> new {_endPoint.Name}({Parameters.Messenger}, {Fields.ImplementationFactory}(), {Fields.EndPointConfiguration});");
        writer.Indent--;
    }
}