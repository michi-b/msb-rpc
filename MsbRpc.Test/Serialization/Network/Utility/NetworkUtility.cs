using System.Net;

namespace MsbRpc.Test.Serialization.Network.Utility;

public static class NetworkUtility
{
    public const int DefaultBufferSize = 1024;

    public static async ValueTask<IPAddress> GetLocalHostAsync(CancellationToken cancellationToken)
        => (await Dns.GetHostEntryAsync("localhost", cancellationToken)).AddressList[0];
}