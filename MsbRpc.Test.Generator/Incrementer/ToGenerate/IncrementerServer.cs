using System;
using Incrementer;
using Incrementer.Generated;
using MsbRpc.Configuration;
using MsbRpc.Messaging;
using MsbRpc.Server;

namespace MsbRpc.Test.Generator.Incrementer.ToGenerate;

public class IncrementerServer : Server<IncrementerServer, IncrementerServerEndPoint, IncrementerProcedure, IIncrementer>
{
    // ReSharper disable once NotAccessedField.Local
    private readonly Configuration _configuration;
    private readonly Func<IIncrementer> _createImplementation;
    private readonly IncrementerServerEndPoint.Configuration _endPointConfiguration;

    private IncrementerServer
    (
        Func<IIncrementer> createImplementation,
        Configuration configuration,
        IncrementerServerEndPoint.Configuration endPointConfiguration
    ) : base(configuration)
    {
        _createImplementation = createImplementation;
        _configuration = configuration;
        _endPointConfiguration = endPointConfiguration;
    }

    public class Configuration : ServerConfiguration { }

    public static IncrementerServer Start
    (
        Func<IIncrementer> createImplementation,
        Action<Configuration>? configure = null,
        Action<IncrementerServerEndPoint.Configuration>? configureEndPoint = null
    )
    {
        var configuration = new Configuration();
        configure?.Invoke(configuration);

        var endPointConfiguration = new IncrementerServerEndPoint.Configuration();
        configureEndPoint?.Invoke(endPointConfiguration);

        IncrementerServer server = new(createImplementation, configuration, endPointConfiguration);
        server.Start();
        return server;
    }

    protected override IncrementerServerEndPoint CreateEndPoint(Messenger messenger) => new(messenger, _createImplementation(), _endPointConfiguration);
}