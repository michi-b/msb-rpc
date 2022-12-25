using System.CodeDom.Compiler;

namespace MsbRpc.Generator.Extensions;

public readonly ref struct IndentedTextWriterBlock
{
    private readonly IndentedTextWriter _writer;
    private readonly BlockAdditions _additions;

    public IndentedTextWriterBlock(IndentedTextWriter writer, BlockAdditions additions)
    {
        _writer = writer;
        _additions = additions;
        _writer.WriteBlockScopeStart();
        _writer.Indent++;
    }

    public void Dispose()
    {
        _writer.Indent--;
        _writer.WriteBlockScopeEnd(_additions);
    }
}