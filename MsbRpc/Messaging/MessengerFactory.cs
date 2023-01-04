using System;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using MsbRpc.Sockets;

namespace MsbRpc.Messaging;

public static partial class MessengerFactory
{
    public static async ValueTask<Messenger> ConnectAsync(IPEndPoint serverEndPoint, ILogger logger)
    {
        Socket socket = new(serverEndPoint.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
        try
        {
            await socket.ConnectAsync(serverEndPoint);
        }
        catch (Exception exception)
        {
            LogConnectionFailed(logger, serverEndPoint, exception);
            throw;
        }
        return new Messenger(new RpcSocket(socket));
    }
    
    [LoggerMessage
    (
        EventId = (int)LogEventIds.MessengerConnectionFailed,
        Level = LogLevel.Error,
        Message = "Failed to connect messenger to remote endpoint {endpoint}"
    )]
    private static partial void LogConnectionFailed(ILogger logger, IPEndPoint endPoint, Exception exception);
}