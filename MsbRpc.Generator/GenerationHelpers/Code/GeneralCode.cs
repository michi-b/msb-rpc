using static MsbRpc.Generator.IndependentNames;

namespace MsbRpc.Generator.GenerationHelpers.Code;

public static class GeneralCode
{
    public const string MessengerParameterLine = $"{Types.Messenger} {Parameters.Messenger},";
    public const string CancellationTokenParameter = $"{Types.CancellationToken} {Parameters.CancellationToken}";
    public const string LoggerFactoryInterfaceParameter = $"{Types.LoggerFactoryInterface} {Parameters.LoggerFactory},";
}