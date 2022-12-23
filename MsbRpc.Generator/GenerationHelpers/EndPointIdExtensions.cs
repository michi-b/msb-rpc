namespace MsbRpc.Generator.GenerationHelpers;

public static class EndPointIdExtensions
{
    public static string GetName(this EndPointId target)
    {
        return target switch
        {
            EndPointId.Client => "Client",
            EndPointId.Server => "Server",
            _ => throw new ArgumentOutOfRangeException(nameof(target), target, null)
        };
    }
    
    public static string GetLowerCaseName(this EndPointId target)
    {
        return target switch
        {
            EndPointId.Client => "client",
            EndPointId.Server => "server",
            _ => throw new ArgumentOutOfRangeException(nameof(target), target, null)
        };
    }

    public static EndPointId GetOther(this EndPointId target)
    {
        return target switch
        {
            EndPointId.Client => EndPointId.Server,
            EndPointId.Server => EndPointId.Client,
            _ => throw new ArgumentOutOfRangeException(nameof(target), target, null)
        };
    }
}