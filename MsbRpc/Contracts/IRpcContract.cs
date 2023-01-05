using System;

namespace MsbRpc.Contracts;

public interface IRpcContract : IDisposable
{
    public bool RandToCompletion { get; }
}