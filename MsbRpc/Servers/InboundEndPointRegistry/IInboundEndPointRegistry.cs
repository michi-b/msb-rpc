using MsbRpc.EndPoints.Interfaces;

namespace MsbRpc.Servers.InboundEndPointRegistry;

public interface IInboundEndPointRegistry
{
    InboundEndPointRegistryEntry[] EndPoints { get; }
    void Run(IInboundEndPoint endPoint);
}