using System;

namespace MsbRpc.EndPoints;

public interface IInboundEndPoint : IDisposable
{
    public void Listen();
}