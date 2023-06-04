using System;
using MsbRpc.Generator.Enums;
using MsbRpc.Generator.GenerationTree;
using static MsbRpc.Generator.Utility.Names;

namespace MsbRpc.Generator.CodeWriters.Files.Base;

internal abstract class EndPointConfigurationBuilderWriter : ConfigurationBuilderWriter
{
    protected readonly EndPointNode EndPoint;

    protected override string BaseClass { get; }

    protected EndPointConfigurationBuilderWriter(EndPointNode endPoint) : base(endPoint.Contract, endPoint.Name)
    {
        EndPoint = endPoint;

        BaseClass = endPoint.Direction switch
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
}