#region

using System;

#endregion

namespace MsbRpc.EndPoints.Interfaces;

public interface IInboundEndPoint : IEndPoint, IDisposable
{
    public void Listen();
}