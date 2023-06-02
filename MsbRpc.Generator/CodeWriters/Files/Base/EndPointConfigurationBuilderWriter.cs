using System.CodeDom.Compiler;
using MsbRpc.Generator.GenerationTree;
using static MsbRpc.Generator.Utility.IndependentNames;

namespace MsbRpc.Generator.CodeWriters.Files.Base;

internal class EndPointConfigurationBuilderWriter : CodeFileWriter
{
    protected override string FileName { get; }
    
    protected EndPointConfigurationBuilderWriter(EndPointNode endPoint) : base(endPoint.Contract)
    {
        FileName = $"{endPoint.Name}ConfigurationBuilder{GeneratedFilePostfix}";
    }
    
    protected override void Write(IndentedTextWriter writer)
    {
        //todo: implement writing
    }
}