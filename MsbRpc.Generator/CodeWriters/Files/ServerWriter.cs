using System.CodeDom.Compiler;
using MsbRpc.Generator.CodeWriters.Utility;
using MsbRpc.Generator.GenerationTree;
using static MsbRpc.Generator.CodeWriters.Utility.IndependentNames;


namespace MsbRpc.Generator.CodeWriters.Files;

internal class ServerWriter : CodeFileWriter
{
    private readonly ContractNode _contract;
    private readonly ServerNode _server;
    private readonly EndPointNode _endPoint;
    private readonly ProcedureCollectionNode _procedures;
    private readonly string _interfaceName;

    protected override string FileName { get; }

    public ServerWriter(ServerNode server) : base(server.Contract)
    {
        _contract = server.Contract;
        _server = server;
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
            // WriteFields(writer);
            
            writer.WriteLine();
            
            // WriteConstructor(writer);
            
            writer.WriteLine();

            // WriteConfiguration(writer);
            
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
        throw new System.NotImplementedException();
    }

    private void WriteConstructor(IndentedTextWriter writer)
    {
        throw new System.NotImplementedException();
    }

    private void WriteConfiguration(IndentedTextWriter writer)
    {
        throw new System.NotImplementedException();
    }

    private void WriteStartMethod(IndentedTextWriter writer)
    {
        throw new System.NotImplementedException();
    }

    private void WriteCreateEndPointOverride(IndentedTextWriter writer)
    {
        throw new System.NotImplementedException();
    }
}