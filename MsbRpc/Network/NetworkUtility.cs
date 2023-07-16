#region

using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;

#endregion

namespace MsbRpc.Network;

public static class NetworkUtility
{
    public const int DefaultBufferSize = 1024;

    public static async ValueTask<IPAddress> GetLocalHostAsync()
    {
        IPHostEntry ipHostEntry = await Dns.GetHostEntryAsync(Dns.GetHostName());
        foreach (IPAddress ipAddress in ipHostEntry.AddressList)
        {
            if (ipAddress.AddressFamily == AddressFamily.InterNetwork)
            {
                return ipAddress;
            }
        }

        throw new UnableToRetrieveLocalHostException();
    }

    public static IPAddress GetLocalHost()
    {
        IPHostEntry ipHostEntry = Dns.GetHostEntry(Dns.GetHostName());
        foreach (IPAddress ipAddress in ipHostEntry.AddressList)
        {
            if (ipAddress.AddressFamily == AddressFamily.InterNetwork)
            {
                return ipAddress;
            }
        }

        throw new UnableToRetrieveLocalHostException();
    }
}