namespace MsbRpc.Generator;

public static class EndPointTypeIdExtensions
{
    public static string GetName(this EndPointTypeId target)
    {
        return target switch
        {
            EndPointTypeId.Client => "Client",
            EndPointTypeId.Server => "Server",
            _ => throw new ArgumentOutOfRangeException(nameof(target), target, null)
        };
    }
}