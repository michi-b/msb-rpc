namespace MsbRpc.Generator;

public static class EndPointTypeIdExtensions
{
    public static string GetUpperCaseName(this EndPointTypeId target)
    {
        return target switch
        {
            EndPointTypeId.Client => "Client",
            EndPointTypeId.Server => "Server",
            _ => throw new ArgumentOutOfRangeException(nameof(target), target, null)
        };
    }

    public static string GetLowerCaseName(this EndPointTypeId target)
    {
        return target switch
        {
            EndPointTypeId.Client => "client",
            EndPointTypeId.Server => "server",
            _ => throw new ArgumentOutOfRangeException(nameof(target), target, null)
        };
    }

    public static EndPointTypeId GetOther(this EndPointTypeId target)
    {
        return target switch
        {
            EndPointTypeId.Client => EndPointTypeId.Server,
            EndPointTypeId.Server => EndPointTypeId.Client,
            _ => throw new ArgumentOutOfRangeException(nameof(target), target, null)
        };
    }
}