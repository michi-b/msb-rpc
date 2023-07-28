#region

using MsbRpc.EndPoints.Interfaces;

#endregion

namespace MsbRpc.Servers.InboundEndPointRegistry;

public interface IInboundEndPointRegistry
{
    InboundEndPointRegistryEntry[] EndPoints { get; }
    void Run(IInboundEndPoint endPoint);
}