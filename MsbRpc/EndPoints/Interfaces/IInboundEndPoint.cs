#region

using System;
using MsbRpc.Messaging;
using MsbRpc.Servers.Listeners.Connections;

#endregion

namespace MsbRpc.EndPoints.Interfaces;

public interface IInboundEndPoint : IEndPoint, IDisposable
{
    public ListenReturnCode Listen();
}