using Microsoft.Extensions.Logging;
using MsbRpc.Configuration;
using MsbRpc.EndPoints;
using MsbRpc.Exceptions;
using MsbRpc.Messaging;
using MsbRpc.Servers.Generic;
using MsbRpc.Test.Implementations.Incrementer.ToGenerate;

namespace MsbRpc.Test.Implementations.Incrementer;

public class IncrementerServer : Server<IncrementerServer, IncrementerServerEndPoint>
{
    private readonly RpcExceptionTransmissionOptions _exceptionTransmissionOptions;

    public IncrementerServer
    (
        ILoggerFactory loggerFactory,
        RpcExceptionTransmissionOptions exceptionTransmissionOptions = RpcExceptionTransmissionOptions.None
    ) : base(loggerFactory)
        => _exceptionTransmissionOptions = exceptionTransmissionOptions;

    protected override IInboundEndPoint CreateEndPoint(Messenger messenger, InboundEndPointConfiguration endPointConfiguration)
        => new IncrementerServerEndPoint(messenger, new Incrementer(_exceptionTransmissionOptions), endPointConfiguration);
}