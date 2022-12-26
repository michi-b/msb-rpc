using MsbRpc.EndPoints;

namespace MsbRpc.Generator.Extensions;

public static class EndPointIdExtensions
{
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