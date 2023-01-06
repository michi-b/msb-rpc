using System.CodeDom.Compiler;
using MsbRpc.Generator.GenerationTree;
using static MsbRpc.Generator.IndependentNames;

namespace MsbRpc.Generator.CodeWriters.Files;

internal class ServerEndPointWriter : CodeFileWriter
{
    private readonly EndPointNode _endPoint;
    protected override string FileName { get; }

    public ServerEndPointWriter(EndPointNode endPoint) : base(endPoint.Contract)
    {
        _endPoint = endPoint;
        FileName = $"{_endPoint.EndPointName}{GeneratedFilePostfix}";
    }

    protected override void Write(IndentedTextWriter writer) { }
}