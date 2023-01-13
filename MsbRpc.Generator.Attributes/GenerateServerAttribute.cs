using System;

namespace MsbRpc.Generator.Attributes;

[GeneratorTarget]
public class GenerateServerAttribute : Attribute
{
    [GeneratorTarget] public readonly int DefaultPort;

    public GenerateServerAttribute(int defaultPort = 0) => DefaultPort = defaultPort;
}