using System;

namespace MsbRpc.Generator.Extensions;

[Flags]
public enum BlockAdditions
{
    None = 0,
    Semicolon = 1 << 0,
    NewLine = 1 << 1,
    SemicolonAndNewline = NewLine | Semicolon
}