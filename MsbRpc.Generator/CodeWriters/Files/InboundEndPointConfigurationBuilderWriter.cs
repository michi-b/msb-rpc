﻿using System.CodeDom.Compiler;
using MsbRpc.Generator.CodeWriters.Files.Base;
using MsbRpc.Generator.GenerationTree;
using static MsbRpc.Generator.Utility.Names;

namespace MsbRpc.Generator.CodeWriters.Files;

internal class InboundEndPointConfigurationBuilderWriter : EndPointConfigurationBuilderWriter
{
    public InboundEndPointConfigurationBuilderWriter(EndPointNode endPoint) : base(endPoint) { }

    protected override void WriteConstructorBody(IndentedTextWriter writer)
    {
        writer.WriteLine($"{Properties.LoggingName} = \"{ClassName}\";");
        writer.WriteLine($"{Fields.InitialBufferSize} = {EndPoint.Contract.DefaultInitialBufferSize};");
    }
}