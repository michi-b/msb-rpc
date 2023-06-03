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

    protected readonly string ClassName;

    protected readonly EndPointNode EndPoint;

    protected override string FileName { get; }

    protected EndPointConfigurationBuilderWriter(EndPointNode endPoint) : base(endPoint.Contract)
    {
        EndPoint = endPoint;

        ClassName = $"{endPoint.Name}ConfigurationBuilder";

        FileName = $"{ClassName}{GeneratedFilePostfix}";

        _baseClass = endPoint.Direction switch
        {
            EndPointDirection.Inbound => Types.InboundEndPointConfigurationBuilder,
            EndPointDirection.Outbound => Types.OutboundEndPointConfigurationBuilder,
            _ => throw new ArgumentOutOfRangeException()
        };
    }

    public static EndPointConfigurationBuilderWriter Get(EndPointNode endPoint)
    {
        return endPoint.Direction switch
        {
            EndPointDirection.Inbound => new InboundEndPointConfigurationBuilderWriter(endPoint),
            EndPointDirection.Outbound => new OutboundEndPointConfigurationBuilderWriter(endPoint),
            _ => throw new ArgumentOutOfRangeException()
        };
    }

    protected override void Write(IndentedTextWriter writer)
    {
        writer.WriteLine($"public class {ClassName} : {_baseClass}");
        using (writer.GetBlock(Appendix.None))
        {
            WriteConstructor(writer);
        }
    }

    private void WriteConstructor(IndentedTextWriter writer)
    {
        writer.WriteLine($"public {ClassName}()");
        using (writer.GetBlock())
        {
            WriteConstructorBody(writer);
        }
    }

    protected abstract void WriteConstructorBody(IndentedTextWriter writer);
}