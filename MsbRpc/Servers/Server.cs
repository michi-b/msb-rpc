﻿using System;
using JetBrains.Annotations;
using MsbRpc.Configuration;
using MsbRpc.Disposable;
using MsbRpc.Messaging;
using MsbRpc.Servers.Listener;

namespace MsbRpc.Servers;

public abstract class Server : ConcurrentDisposable, IUnIdentifiedConnectionReceiver
{
    protected readonly ServerConfiguration Configuration;

    [PublicAPI] public ConnectionListener? ConnectionListener { get; private set; }

    protected Server(ref ServerConfiguration configuration) => Configuration = configuration;

    [PublicAPI]
    public ConnectionListener Listen()
    {
        if (ConnectionListener != null)
        {
            throw new InvalidOperationException($"{ConnectionListener.Thread.Name} is already started.");
        }

        ConnectionListener = ConnectionListener.Run(Configuration.ConnectionListenerConfiguration, this);

        return ConnectionListener;
    }

    public abstract void AcceptUnIdentified(Messenger messenger);

    protected override void DisposeManagedResources()
    {
        if (ConnectionListener != null)
        {
            ConnectionListener.Dispose();
            ConnectionListener = null;
        }

        base.DisposeManagedResources();
    }
}