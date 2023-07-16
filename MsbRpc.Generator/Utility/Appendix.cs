﻿#region

using System;

#endregion

namespace MsbRpc.Generator.Utility;

[Flags]
public enum Appendix
{
    None = 0,
    Semicolon = 1 << 0,
    NewLine = 1 << 1,
    SemicolonAndNewline = NewLine | Semicolon
}