using Microsoft.Extensions.Logging;
using MsbRpc.Configuration;
using MsbRpc.EndPoints;
using MsbRpc.Messaging;
using MsbRpc.Servers.Generic;
using MsbRpc.Test.Implementations.DateTimeEcho.ToGenerate;

namespace MsbRpc.Test.Implementations.DateTimeEcho;

public class DateTimeEchoServer : Server<DateTimeEchoServer, DateTimeEchoServerEndPoint>
{
    public DateTimeEchoServer(ILoggerFactory loggerFactory) : base(loggerFactory) { }

    protected override IInboundEndPoint CreateEndPoint(Messenger messenger, InboundEndPointConfiguration endPointConfiguration)
        => new DateTimeEchoServerEndPoint(messenger, new DateTimeEcho(), endPointConfiguration);
}