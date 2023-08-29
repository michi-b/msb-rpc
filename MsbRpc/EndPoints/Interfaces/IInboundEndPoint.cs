#region

using System;
using MsbRpc.Messaging;

#endregion

namespace MsbRpc.EndPoints.Interfaces;

public interface IInboundEndPoint : IEndPoint, IDisposable
{
    public ListenReturnCode Listen();
}