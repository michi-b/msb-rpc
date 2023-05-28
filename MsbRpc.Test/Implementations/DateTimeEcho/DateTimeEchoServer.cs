using MsbRpc.Configuration;
using MsbRpc.EndPoints;
using MsbRpc.Messaging;
using MsbRpc.Servers.Generic;
using MsbRpc.Test.Implementations.DateTimeEcho.ToGenerate;

namespace MsbRpc.Test.Implementations.DateTimeEcho;

public class DateTimeEchoServer : EndPointRegisteringServer
{
    private readonly IFactory<IDateTimeEcho> _implementationFactory;

    public DateTimeEchoServer(IFactory<IDateTimeEcho> implementationFactory, ref ServerConfiguration configuration)
        : base(ref configuration)
        => _implementationFactory = implementationFactory;

    protected override IInboundEndPoint CreateEndPoint(Messenger messenger)
        => new DateTimeEchoServerEndPoint(messenger, _implementationFactory.Create(), Configuration.InboundEndPointConfiguration);
}