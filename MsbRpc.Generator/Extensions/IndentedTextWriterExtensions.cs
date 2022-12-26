using System.CodeDom.Compiler;

namespace MsbRpc.Generator.Extensions;

public static class IndentedTextWriterExtensions
{
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
            case BlockAdditions.SemicolonAndNewline:
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
            case BlockAdditions.SemicolonAndNewline:
                await writer.WriteLineAsync(");");
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(additions), additions, null);
        }
    }
}