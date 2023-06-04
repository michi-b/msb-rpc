using System.CodeDom.Compiler;
using MsbRpc.Generator.CodeWriters.Files.Base;
using MsbRpc.Generator.Extensions;
using MsbRpc.Generator.GenerationTree;
using MsbRpc.Generator.Utility;
using static MsbRpc.Generator.Utility.Names;

namespace MsbRpc.Generator.CodeWriters.Files;

internal class ServerWriter : CodeFileWriter
{
    private const string ImplementationFactoryFieldName = "_implementationFactory";
    private readonly EndPointNode _serverEndPoint;

    protected override string FileName { get; }

    public ServerWriter(EndPointNode serverEndPoint) : base(serverEndPoint.Contract)
    {
        _serverEndPoint = serverEndPoint;
        FileName = $"{Contract.ServerName}{GeneratedFilePostfix}";
    }

    protected override void Write(IndentedTextWriter writer)
    {
        writer.WriteLine($"{Contract.AccessibilityKeyword} class {Contract.ServerName} : {Types.RegistryServer}");
        using (writer.GetBlock(Appendix.None))
        {
            WriteClassBody(writer);
        }
    }

    private void WriteClassBody(IndentedTextWriter writer)
    {
        writer.WriteLine($"private readonly {Contract.ImplementationFactoryInterface} {ImplementationFactoryFieldName}");
        writer.WriteLine();
        WriteConstructor(writer);
        writer.WriteLine();
        WriteCreateEndPointMethod(writer);
    }

    private void WriteConstructor(IndentedTextWriter writer)
    {
        writer.Write($"public {Contract.ServerName}");
        writer.Write($"({Contract.ImplementationFactoryInterface} {Parameters.ContractImplementationFactory}");
        writer.WriteLine($", ref {Types.ServerConfiguration} {Parameters.Configuration})");

        writer.WriteLine();

        writer.Indent++;
        writer.WriteLine($": base(ref {Parameters.Configuration}");
        writer.WriteLine($"=> {ImplementationFactoryFieldName} = {Parameters.ContractImplementationFactory};");
        writer.Indent--;
    }

    private void WriteCreateEndPointMethod(IndentedTextWriter writer)
    {
        writer.WriteLine
            ($"protected override {Interfaces.InboundEndPoint} {Methods.ServerCreateEndPoint}({Types.Messenger} {Parameters.Messenger}, int {Parameters.Id})");
        writer.Indent++;
        writer.Write($"=> new {_serverEndPoint.FullName}(");
        writer.Write($"{Parameters.Messenger}, ");
        writer.Write($"{ImplementationFactoryFieldName}.{Methods.ImplementationFactoryCreate}(), ");
        writer.Write($"{Parameters.Id}, ");
        writer.WriteLine($"{Properties.ServerConfiguration}.{Properties.ServerConfigurationInboundEndPointConfiguration});");
        writer.Indent--;
    }
}