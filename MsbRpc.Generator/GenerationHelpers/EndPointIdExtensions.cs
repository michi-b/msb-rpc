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
}