#region

using System;

#endregion

namespace MsbRpc.Serialization.Buffers;

[Flags]
public enum ResponseFlags : byte
{
    None = 0,
    RanToCompletion = 1 << 0,
    Faulted = 1 << 1
}