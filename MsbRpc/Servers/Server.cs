using System;
using JetBrains.Annotations;
using MsbRpc.Configuration;
using MsbRpc.Disposable;
using MsbRpc.Messaging;
using MsbRpc.Servers.Listener;

namespace MsbRpc.Servers;

public abstract class Server : ConcurrentDisposable, IConnectionReceiver
{
    protected readonly ServerConfiguration Configuration;

    [PublicAPI] public MessengerListener? ConnectionListener { get; private set; }

    protected Server(ref ServerConfiguration configuration) => Configuration = configuration;

    [PublicAPI]
    public MessengerListener Listen()
    {
        if (ConnectionListener != null)
        {
            throw new InvalidOperationException($"{ConnectionListener.Thread.Name} is already started.");
        }

        ConnectionListener = MessengerListener.Start(Configuration.MessengerListenerConfiguration, this);

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