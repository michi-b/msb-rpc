#region

using MsbRpc.Configuration;
using MsbRpc.Messaging;
using MsbRpc.Servers.InboundEndPointRegistry;
using MsbRpc.Servers.Listener;

#endregion

namespace MsbRpc.Test.Implementations.DateTimeEcho.ToGenerate;

public class DateTimeEchoServer : IConnectionReceiver
{
    private readonly IFactory<IDateTimeEcho> _implementationFactory;
    private readonly InboundEndPointConfiguration _endPointConfiguration;
    private readonly IInboundEndPointRegistry _endPointRegistry;

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