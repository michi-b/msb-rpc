#region

using MsbRpc.Configuration;
using MsbRpc.Messaging;
using MsbRpc.Servers.InboundEndPointRegistry;
using MsbRpc.Servers.Listeners.Connections;

#endregion

namespace MsbRpc.Test.Implementations.DateTimeEcho.ToGenerate;

public class DateTimeEchoServer : IConnectionReceiver
{
    private readonly InboundEndPointConfiguration _endPointConfiguration;
    private readonly IInboundEndPointRegistry _endPointRegistry;
    private readonly IFactory<IDateTimeEcho> _implementationFactory;

    public DateTimeEchoServer
    (
        IFactory<IDateTimeEcho> implementationFactory,
        InboundEndPointConfiguration endPointConfiguration,
        IInboundEndPointRegistry endPointRegistry
    )
    {
        _implementationFactory = implementationFactory;
        _endPointConfiguration = endPointConfiguration;
        _endPointRegistry = endPointRegistry;
    }

    public void Accept(Messenger messenger)
    {
        DateTimeEchoServerEndPoint endPoint = new(messenger, _implementationFactory.Create(), _endPointConfiguration);
        _endPointRegistry.Run(endPoint);
    }
}