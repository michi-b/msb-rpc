// ReSharper disable RedundantNameQualifier


using System.Threading;

// ReSharper disable once CheckNamespace
namespace Incrementer.Generated;

public class IncrementerServer : MsbRpc.EndPoints.Server<IncrementerServerEndPoint, IncrementerProcedure, IIncrementer>
{
    public IncrementerServer
    (
        System.Func<IIncrementer> getImplementation,
        Microsoft.Extensions.Logging.ILoggerFactory loggerFactory,
        int port = 0
    )
        : base
        (
            getImplementation,
            loggerFactory,
            MsbRpc.Utility.LoggerFactoryExtensions.TryCreateLogger<IncrementerServer>(loggerFactory),
            port
        ) { }

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
    
    protected override IncrementerServerEndPoint CreateEndPoint
        (MsbRpc.Messaging.Messenger messenger)
        => new IncrementerServerEndPoint(messenger, this.GetImplementation(), this.LoggerFactory);
}