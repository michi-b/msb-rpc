using System.Threading;
using MsbRpc.EndPoints;

namespace MsbRpc.Servers;

public struct InboundEndPointRegistryEntry
{
    public readonly IInboundEndPoint EndPoint;
    public readonly Thread Thread;

    public InboundEndPointRegistryEntry(IInboundEndPoint endPoint, Thread thread)
    {
        Thread = thread;
        EndPoint = endPoint;
    }
}