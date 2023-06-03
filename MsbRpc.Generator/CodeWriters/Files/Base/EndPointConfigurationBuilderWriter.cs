using System;
using System.CodeDom.Compiler;
using MsbRpc.Generator.Enums;
using MsbRpc.Generator.Extensions;
using MsbRpc.Generator.GenerationTree;
using MsbRpc.Generator.Utility;
using static MsbRpc.Generator.Utility.Names;

namespace MsbRpc.Generator.CodeWriters.Files.Base;

internal abstract class EndPointConfigurationBuilderWriter : CodeFileWriter
{
    private readonly string _baseClass;

    private readonly string _class;
    protected override string FileName { get; }

    protected EndPointConfigurationBuilderWriter(EndPointNode endPoint) : base(endPoint.Contract)
    {
        _class = $"{endPoint.Name}ConfigurationBuilder";
        FileName = $"{_class}{GeneratedFilePostfix}";
        _baseClass = endPoint.Direction switch
        {
            EndPointDirection.Inbound => Types.InboundEndPointConfigurationBuilder,
            EndPointDirection.Outbound => Types.OutboundEndPointConfigurationBuilder,
            _ => throw new ArgumentOutOfRangeException()
        };
    }

    protected override void Write(IndentedTextWriter writer)
    {
        writer.WriteLine($"public class {_class} : {_baseClass}");
        using (writer.GetBlock(Appendix.None))
        {
            WriteConstructor(writer);
        }
    }

    private void WriteConstructor(IndentedTextWriter writer)
    {
        writer.WriteLine($"public {_class}()");
        using (writer.GetBlock())
        {
            WriteConstructorBody(writer);
        }
    }

    protected abstract void WriteConstructorBody(IndentedTextWriter writer);
}