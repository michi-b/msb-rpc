#region

using System;

#endregion

namespace MsbRpc.Servers.Listeners.Connections;

[Flags]
public enum ConnectionRequestSizeOptions
{
    None = 0,
    WithId = 1 << 0,
    WithMessageOffset = 1 << 1
}