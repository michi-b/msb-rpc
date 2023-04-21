﻿using MsbRpc.Exceptions;
using MsbRpc.Messaging;
using MsbRpc.Servers;
using MsbRpc.Test.Generator.Incrementer.ToGenerate;

namespace MsbRpc.Test.Generator.Incrementer;

public class IncrementerServer : Server
{
    private readonly IncrementerServerConfiguration _configuration;
    private readonly InboundEndPointRegistry _endPointRegistry;
    private readonly RpcExceptionTransmissionOptions _exceptionTransmissionOptions;

    public InboundEndPointRegistryEntry[] EndPoints => _endPointRegistry.EndPoints;

    public IncrementerServer
    (
        IncrementerServerConfiguration configuration,
        RpcExceptionTransmissionOptions exceptionTransmissionOptions
    ) : base(configuration.ServerConfiguration)
    {
        _configuration = configuration;
        _exceptionTransmissionOptions = exceptionTransmissionOptions;
        _endPointRegistry = new InboundEndPointRegistry(_configuration.EndPointRegistryConfiguration);
    }

    protected override void Accept(Messenger messenger)
    {
        _endPointRegistry.Run(new IncrementerServerEndPoint(messenger, new Incrementer(_exceptionTransmissionOptions), _configuration.EndPointConfiguration));
    }

    protected override void Dispose(bool disposing)
    {
        if (disposing)
        {
            _endPointRegistry.Dispose();
        }

        base.Dispose(disposing);
    }
}