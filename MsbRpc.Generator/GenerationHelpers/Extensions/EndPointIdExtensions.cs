using MsbRpc.EndPoints;

namespace MsbRpc.Generator.GenerationHelpers.Extensions;

public static class EndPointIdExtensions
{
    public static EndPointDirection GetInitialDirection(this EndPointId target)
    {
        return target switch
        {
            EndPointId.Client => EndPointDirection.Outbound,
            EndPointId.Server => EndPointDirection.Inbound,
            _ => throw new ArgumentOutOfRangeException(nameof(target), target, null)
        };
    }
}