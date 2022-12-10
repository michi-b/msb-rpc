using System.CodeDom.Compiler;

namespace MsbRpc.Generator.Extensions;

public static class IndentedTextWriterExtensions
{
    public static Block EncloseInBlock(this IndentedTextWriter writer) => new(writer);

    public readonly ref struct Block
    {
        private readonly IndentedTextWriter _writer;

        public Block(IndentedTextWriter writer)
        {
            _writer = writer;
            _writer.WriteBlockScopeStart();
        }

        public void Dispose()
        {
            _writer.WriteBlockScopeEnd();
        }
    }
    
    private static void WriteBlockScopeStart(this IndentedTextWriter writer)
    {
        writer.WriteLine('{');
        writer.Indent++;
    }

    private static void WriteBlockScopeEnd(this IndentedTextWriter writer)
    {
        writer.Indent--;
        writer.WriteLine('}');
    }
}