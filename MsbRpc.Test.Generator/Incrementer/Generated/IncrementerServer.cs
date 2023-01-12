// ReSharper disable RedundantNameQualifier

// ReSharper disable once CheckNamespace

using System;
using Microsoft.Extensions.Logging;
using MsbRpc.EndPoints;
using MsbRpc.Messaging;
using LoggerFactoryExtensions = MsbRpc.Utility.LoggerFactoryExtensions;

namespace Incrementer.Generated;

public class IncrementerServer : Server<IncrementerServerEndPoint, IncrementerProcedure, IIncrementer>
{
    private IncrementerServer
    (
        Func<IIncrementer> createImplementation,
        ILoggerFactory loggerFactory,
        int port = 0
    )
        : base
        (
            loggerFactory,
            LoggerFactoryExtensions.CreateLoggerOptional<IncrementerServer>(loggerFactory),
            port
        ) { }

    public static Generated.IncrementerServer Start
    (
        Func<IIncrementer> getImplementation,
        ILoggerFactory loggerFactory,
        int port = 0
    )
    {
        Generated.IncrementerServer server = new(getImplementation, loggerFactory, port);
        server.Start();
        return server;
    }

    protected override IncrementerServerEndPoint CreateEndPoint(Messenger messenger) => new(messenger, CreateImplementation(), LoggerFactory);
}