using MsbRpc.Configuration.Interfaces.Generic;
using MsbRpc.EndPoints;
using MsbRpc.Messaging;
using MsbRpc.Servers.Generic;
using MsbRpc.Test.Implementations.DateTimeEcho.ToGenerate;

namespace MsbRpc.Test.Implementations.DateTimeEcho;

public class DateTimeEchoServer : Server<IDateTimeEcho>
{
    private readonly IServerConfiguration<IDateTimeEcho> _configuration;

    public DateTimeEchoServer(DateTimeEchoServerConfiguration configuration) : base(configuration) => _configuration = configuration;

    protected override IInboundEndPoint CreateEndPoint(Messenger messenger)
        => new DateTimeEchoServerEndPoint(messenger, _configuration.ImplementationFactory.Create(), _configuration.InboundEndPointConfiguration);
}