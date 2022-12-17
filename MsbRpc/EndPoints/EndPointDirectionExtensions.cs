namespace MsbRpc.EndPoints;

public static class EndPointDirectionExtensions
{
    public static string GetName(this EndPointDirection target)
    {
        return target switch
        {
            EndPointDirection.Inbound => nameof(EndPointDirection.Inbound),
            EndPointDirection.Outbound => nameof(EndPointDirection.Outbound),
            _ => throw new ArgumentOutOfRangeException(nameof(target), target, null)
        };
    }
}