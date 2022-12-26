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

        switch (additions)
        {
            case BlockAdditions.None:
                await writer.WriteAsync("}");
                break;
            case BlockAdditions.Semicolon:
                await writer.WriteAsync("};");
                break;
            case BlockAdditions.NewLine:
                await writer.WriteLineAsync("}");
                break;
            case BlockAdditions.WithTrailingSemicolonAndNewline:
                await writer.WriteLineAsync("};");
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(additions), additions, null);
        }
    }

    public static async ValueTask EnterParenthesesBlockAsync(this IndentedTextWriter writer)
    {
        await writer.WriteLineAsync("(");
        writer.Indent++;
    }

    public static async ValueTask ExitParenthesesBlockAsync(this IndentedTextWriter writer, BlockAdditions additions = BlockAdditions.NewLine)
    {
        writer.Indent--;

        switch (additions)
        {
            case BlockAdditions.None:
                await writer.WriteAsync(")");
                break;
            case BlockAdditions.Semicolon:
                await writer.WriteAsync(");");
                break;
            case BlockAdditions.NewLine:
                await writer.WriteLineAsync(")");
                break;
            case BlockAdditions.WithTrailingSemicolonAndNewline:
                await writer.WriteLineAsync(");");
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(additions), additions, null);
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