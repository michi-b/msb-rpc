using System.CodeDom.Compiler;

namespace MsbRpc.Generator.Extensions;

public static class IndentedTextWriterExtensions
{
    public static IndentedTextWriterBlock EncloseInBlockAsync(this IndentedTextWriter writer, BlockAdditions additions = BlockAdditions.NewLine)
        => new(writer, additions);

    public static IndentedTextWriterParenthesesBlock EncloseInParenthesesBlockAsync(this IndentedTextWriter writer, bool withTrailingNewline = true)
        => new(writer, withTrailingNewline);

    public static string GetResult(this IndentedTextWriter writer) => writer.InnerWriter.ToString();

    public static async ValueTask EnterBlockAsync(this IndentedTextWriter writer)
    {
        await writer.WriteLineAsync("{");
        writer.Indent++;
    }

    public static async ValueTask ExitBlockAsync(this IndentedTextWriter writer, BlockAdditions additions = BlockAdditions.NewLine)
    {
        writer.Indent--;

        await writer.WriteAsync("}");

        if (additions.HasFlag(BlockAdditions.Semicolon))
        {
            await writer.WriteLineAsync(';');
        }

        if (additions.HasFlag(BlockAdditions.NewLine))
        {
            await writer.WriteLineAsync();
        }
    }

    public static void WriteBlockScopeStart(this IndentedTextWriter writer)
    {
        writer.WriteLine('{');
    }

    public static void WriteBlockScopeEnd(this IndentedTextWriter writer, BlockAdditions additions)
    {
        writer.Write('}');
        if (additions.HasFlag(BlockAdditions.Semicolon))
        {
            writer.Write(';');
        }

        if (additions.HasFlag(BlockAdditions.NewLine))
        {
            writer.WriteLine();
        }
    }

    public static void WriteParenthesesBlockScopeStart(this IndentedTextWriter writer)
    {
        writer.WriteLine('(');
    }

    public static void WriteParenthesesBlockScopeEnd(this IndentedTextWriter writer, bool withTrailingNewline)
    {
        if (withTrailingNewline)
        {
            writer.WriteLine(')');
        }
        else
        {
            writer.Write(')');
        }
    }
}