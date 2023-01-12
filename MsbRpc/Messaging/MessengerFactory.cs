using System;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using MsbRpc.Sockets;

namespace MsbRpc.Messaging;

public static class MessengerFactory
{
    public static async ValueTask<Messenger> ConnectAsync(IPEndPoint serverEndPoint, ILogger? logger = null)
    {
        Socket socket = new(serverEndPoint.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
        try
        {
            await socket.ConnectAsync(serverEndPoint);
        }
        catch (Exception exception)
        {
            if (logger != null)
            {
                LogConnectionFailed(logger, serverEndPoint, exception);
            }
            throw;
        }

        return new Messenger(new RpcSocket(socket));
    }

    private static void LogConnectionFailed(ILogger logger, IPEndPoint endPoint, Exception exception)
    {
        if (logger.IsEnabled(LogLevel.Error))
        {
            logger.LogError(LogEventIds.MessengerConnectionFailed, exception, "Failed to connect messenger to remote endpoint {Endpoint}", endPoint);
        }
    }
}