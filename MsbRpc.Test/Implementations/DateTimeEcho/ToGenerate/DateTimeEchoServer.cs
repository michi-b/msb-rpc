using MsbRpc.Configuration;
using MsbRpc.EndPoints.Interfaces;
using MsbRpc.Messaging;
using MsbRpc.Servers.Generic;

namespace MsbRpc.Test.Implementations.DateTimeEcho.ToGenerate;

public class DateTimeEchoServer : RegistryServer
{
    private readonly IFactory<IDateTimeEcho> _implementationFactory;

    public DateTimeEchoServer(IFactory<IDateTimeEcho> implementationFactory, ref ServerConfiguration configuration)
        : base(ref configuration)
        => _implementationFactory = implementationFactory;

    protected override IInboundEndPoint CreateEndPoint(Messenger messenger, int id)
        => new DateTimeEchoServerEndPoint(messenger, _implementationFactory.Create(), id, Configuration.InboundEndPointConfiguration);
}