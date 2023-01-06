// ReSharper disable RedundantNameQualifier

// ReSharper disable once CheckNamespace
namespace Incrementer.Generated;

public class IncrementerServer : MsbRpc.EndPoints.Server<IncrementerServerEndPoint, IncrementerProcedure, IIncrementer>
{
    public static Incrementer.Generated.IncrementerServer Start
    (
        System.Func<IIncrementer> getImplementation,
        Microsoft.Extensions.Logging.ILoggerFactory loggerFactory,
        int port = 0
    )
    {
        Incrementer.Generated.IncrementerServer server = new(getImplementation, loggerFactory, port);
        server.Start();
        return server;
    }

    private IncrementerServer
    (
        System.Func<IIncrementer> getImplementation,
        Microsoft.Extensions.Logging.ILoggerFactory loggerFactory,
        int port = 0
    )
        : base
        (
            getImplementation,
            loggerFactory,
            MsbRpc.Utility.LoggerFactoryExtensions.CreateLoggerOptional<IncrementerServer>(loggerFactory),
            port
        ) { }

    protected override IncrementerServerEndPoint CreateEndPoint
        (MsbRpc.Messaging.Messenger messenger)
        => new IncrementerServerEndPoint(messenger, GetImplementation(), LoggerFactory);
}