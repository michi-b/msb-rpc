using System.CodeDom.Compiler;

namespace MsbRpc.Generator.Extensions;

public static class IndentedTextWriterExtensions
{
    public static Block EncloseInBlock(this IndentedTextWriter writer, bool withTrailingNewline = true) => new(writer, withTrailingNewline);

    public readonly ref struct Block
    {
        private readonly IndentedTextWriter _writer;
        private readonly bool _withTrailingNewline;

        public Block(IndentedTextWriter writer, bool withTrailingNewline)
        {
            _writer = writer;
            _withTrailingNewline = withTrailingNewline;
            _writer.WriteBlockScopeStart();
        }

        public void Dispose()
        {
            _writer.WriteBlockScopeEnd(_withTrailingNewline);
        }
    }

    public static string GetResult(this IndentedTextWriter writer) => writer.InnerWriter.ToString();

    private static void WriteBlockScopeStart(this IndentedTextWriter writer)
    {
        writer.WriteLine('{');
        writer.Indent++;
    }

    private static void WriteBlockScopeEnd(this IndentedTextWriter writer, bool withTrailingNewline)
    {
        writer.Indent--;
        if (withTrailingNewline)
        {
            writer.WriteLine('}');
        }
        else
        {
            writer.Write('}');
        }
    }
}