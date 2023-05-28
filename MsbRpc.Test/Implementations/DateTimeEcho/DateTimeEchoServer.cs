using MsbRpc.Configuration.Interfaces;
using MsbRpc.EndPoints;
using MsbRpc.Messaging;
using MsbRpc.Servers.Generic;
using MsbRpc.Test.Implementations.DateTimeEcho.ToGenerate;

namespace MsbRpc.Test.Implementations.DateTimeEcho;

public class DateTimeEchoServer : EndPointRegisteringServer
{
    private readonly IServerConfiguration _configuration;
    private readonly IFactory<IDateTimeEcho> _implementationFactory;

    public DateTimeEchoServer(IFactory<IDateTimeEcho> implementationFactory, IServerConfiguration configuration)
        : base(configuration)
    {
        _implementationFactory = implementationFactory;
        _configuration = configuration;
    }

    protected override IInboundEndPoint CreateEndPoint(Messenger messenger)
        => new DateTimeEchoServerEndPoint(messenger, _implementationFactory.Create(), _configuration.InboundEndPointConfiguration);
}