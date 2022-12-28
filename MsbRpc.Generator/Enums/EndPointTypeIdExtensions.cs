namespace MsbRpc.Generator.Enums;

internal static class EndPointTypeIdExtensions
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

    public static EndPointDirection GetInitialDirection(this EndPointTypeId target)
    {
        return target switch
        {
            EndPointTypeId.Client => EndPointDirection.Outbound,
            EndPointTypeId.Server => EndPointDirection.Inbound,
            _ => throw new ArgumentOutOfRangeException(nameof(target), target, null)
        };
    }
}