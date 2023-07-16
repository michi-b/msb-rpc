using MsbRpc.Configuration;
using MsbRpc.EndPoints.Interfaces;
using MsbRpc.Messaging;
using MsbRpc.Servers.Generic;

namespace MsbRpc.Test.Implementations.DateTimeEcho.ToGenerate;

public class DateTimeEchoServer : RegistryServer
{
    private readonly IFactory<IDateTimeEcho> _implementationFactory;

    private DateTimeEchoServer(IFactory<IDateTimeEcho> implementationFactory, ref ServerConfiguration configuration)
        : base(ref configuration)
        => _implementationFactory = implementationFactory;

    public static DateTimeEchoServer Run(IFactory<IDateTimeEcho> implementationFactory, ref ServerConfiguration configuration)
    {
        DateTimeEchoServer server = new(implementationFactory, ref configuration);
        server.Listen();
        return server;
    }

    protected override IInboundEndPoint CreateEndPoint(Messenger messenger)
        => new DateTimeEchoServerEndPoint(messenger, _implementationFactory.Create(), Configuration.EndPointConfiguration);
}