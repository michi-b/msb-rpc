#region

using System.CodeDom.Compiler;
using System.IO;
using MsbRpc.Generator.CodeWriters.Files.Base;
using MsbRpc.Generator.Extensions;
using MsbRpc.Generator.GenerationTree;
using MsbRpc.Generator.Utility;
using static MsbRpc.Generator.Utility.Names;

#endregion

namespace MsbRpc.Generator.CodeWriters.Files;

internal class ServerWriter : CodeFileWriter
{
    private const string ImplementationFactoryFieldName = "_implementationFactory";
    private readonly ServerNode _server;

    protected override string FileName { get; }

    public ServerWriter(ServerNode server) : base(server.Contract)
    {
        _server = server;
        FileName = $"{server.Name}{GeneratedFilePostfix}";
    }

    protected override void Write(IndentedTextWriter writer)
    {
        writer.WriteLine($"{Contract.AccessibilityKeyword} class {_server.Name} : {Types.RegistryServer}");

        using (writer.GetBlock(Appendix.None))
        {
            WriteClassBody(writer);
        }
    }

    private void WriteClassBody(IndentedTextWriter writer)
    {
        writer.WriteLine($"private readonly {Interfaces.Factory}<{_server.Contract.Interface}> {ImplementationFactoryFieldName};");
        writer.WriteLine();
        WriteConstructor(writer);
        writer.WriteLine();
        WriteRunMethod(writer);
        writer.WriteLine();
        WriteCreateEndPointMethod(writer);
    }

    private void WriteConstructor(IndentedTextWriter writer)
    {
        writer.Write($"private {_server.Name}");
        WriteConstructorParameters(writer);

        writer.Indent++;
        writer.WriteLine($": base(ref {Parameters.Configuration})");
        writer.WriteLine($"=> {ImplementationFactoryFieldName} = {Parameters.ContractImplementationFactory};");
        writer.Indent--;
    }

    private void WriteRunMethod(IndentedTextWriter writer)
    {
        writer.Write($"public static {_server.Name} {Methods.Run}");
        WriteConstructorParameters(writer);
        using (writer.GetBlock())
        {
            writer.WriteLine($"{_server.Name} {Variables.Server} = new {_server.Name}({Parameters.ContractImplementationFactory}, ref {Parameters.Configuration});");
            writer.WriteLine($"{Variables.Server}.{Methods.ServerListen}();");
            writer.WriteLine($"return {Variables.Server};");
        }
    }

    private void WriteConstructorParameters(TextWriter writer)
    {
        writer.Write($"({_server.ImplementationFactoryType} {Parameters.ContractImplementationFactory}");
        writer.WriteLine($", ref {Types.ServerConfiguration} {Parameters.Configuration})");
    }

    private void WriteCreateEndPointMethod(IndentedTextWriter writer)
    {
        writer.WriteLine($"protected override {Interfaces.InboundEndPoint} {Methods.CreateEndPoint}({Types.Messenger} {Parameters.Messenger})");
        writer.Indent++;
        writer.WriteLine($"=> new {_server.EndPoint.FullName}");
        using (writer.GetParenthesesBlock(Appendix.SemicolonAndNewline))
        {
            writer.WriteLine($"{Parameters.Messenger}, ");
            writer.WriteLine($"{ImplementationFactoryFieldName}.{Methods.Create}(),");
            writer.WriteLine($"{Properties.ServerConfiguration}.{Properties.EndPointConfiguration}");
        }

        writer.Indent--;
    }
}