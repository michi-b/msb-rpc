using System;
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
    private readonly string _interfaceName;
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
        _interfaceName = _contract.InterfaceName;
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

            // WriteStartMethod(writer);

            writer.WriteLine();

            // WriteCreateEndPointOverride(writer);
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
        writer.WriteLine($"private readonly {_server.EndPointConfigurationTypeFullName} {Fields.EndPointConfiguration};");
        writer.WriteLine($"private readonly {Types.LocalConfiguration} {Fields.Configuration};");
    }

    private void WriteConstructor(IndentedTextWriter writer)
    {
        writer.WriteLine($"private {_server.Name}");
        using (writer.GetParenthesesBlock(Appendix.None))
        {
            writer.WriteLine($"{_contractImplementationFactoryType} {Parameters.ContractImplementationFactory},");
            writer.WriteLine($"{Types.LocalConfiguration} {Parameters.Configuration},");
            writer.WriteLine($"{_server.EndPointConfigurationTypeFullName} {Parameters.EndPointConfiguration}");
        }

        writer.WriteLine($": base({Parameters.Configuration})");
        using (writer.GetBlock())
        {
            writer.WriteLine($"{Fields.ImplementationFactory} = {Parameters.ContractImplementationFactory};");
            writer.WriteLine($"{Fields.Configuration} = {Parameters.Configuration};");
            writer.WriteLine($"{Fields.EndPointConfiguration} = {Parameters.EndPointConfiguration};");
        }
    }

    private void WriteConfiguration(IndentedTextWriter writer)
    {
        writer.WriteLine($"public class {Types.LocalConfiguration} : {Types.ServerConfiguration} {{}}");
    }

    private void WriteStartMethod(IndentedTextWriter writer)
    {
        // writer.WriteLine($"{Types.Action}<{Types.LocalConfiguration}? {Parameters.ConfigureAction} = null,");
        // writer.WriteLine($"{Types.Action}<{_server.EndPointConfigurationTypeFullName}>? {Parameters.ConfigureEndPointAction} = null");
        throw new NotImplementedException();
    }

    private void WriteCreateEndPointOverride(IndentedTextWriter writer)
    {
        throw new NotImplementedException();
    }
}