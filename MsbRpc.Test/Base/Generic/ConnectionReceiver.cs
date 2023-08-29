#region

using Microsoft.Extensions.Logging;
using MsbRpc.Contracts;
using MsbRpc.Disposable;
using MsbRpc.EndPoints;
using MsbRpc.Messaging;
using MsbRpc.Servers.Listeners.Connections;

#endregion

namespace MsbRpc.Test.Base.Generic;

internal class ConnectionReceiver<TServerEndPoint, TProcedure, TContract> : ConcurrentDisposable, IConnectionReceiver
    where TServerEndPoint : InboundEndPoint<TProcedure, TContract>
    where TProcedure : Enum
    where TContract : IRpcContract
{
    private readonly CancellationTokenSource _cancellationTokenSource;
    private readonly List<Task<ListenReturnCode>> _connections = new();
    private readonly Func<Messenger, TServerEndPoint> _createServerEndPoint;
    private readonly ILogger<ConnectionReceiver<TServerEndPoint, TProcedure, TContract>> _logger;

    public ConnectionReceiver(Func<Messenger, TServerEndPoint> createServerEndPoint, ILoggerFactory loggerFactory, CancellationToken cancellationToken)
    {
        _createServerEndPoint = createServerEndPoint;
        _cancellationTokenSource = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);
        _logger = loggerFactory.CreateLogger<ConnectionReceiver<TServerEndPoint, TProcedure, TContract>>();
    }

    public void Accept(Messenger messenger)
    {
        TServerEndPoint endPoint = _createServerEndPoint(messenger);
        _connections.Add(endPoint.ListenAsync(_cancellationTokenSource.Token));
    }

    protected override void DisposeManagedResources()
    {
        _cancellationTokenSource.Cancel();
        foreach (Task<ListenReturnCode> task in _connections)
        {
            ListenReturnCode listenReturnCode = task.Result;
            _logger.Log(LogLevel.Information, "Joining connection task {Task} with listen return code {ListenReturnCode}", task.Id, listenReturnCode);
        }

        _connections.Clear();
        base.DisposeManagedResources();
    }
}