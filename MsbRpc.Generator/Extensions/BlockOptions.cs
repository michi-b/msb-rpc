namespace MsbRpc.Generator.Extensions;

[Flags]
public enum BlockOptions
{
    None = 0,
    WithTrailingNewline = 1 << 0,
    WithTrailingSemicolon = 1 << 1,
    WithTrailingSemicolonAndNewline = WithTrailingNewline | WithTrailingSemicolon
}