using static MsbRpc.Generator.GenerationHelpers.Names.GeneralNames;

namespace MsbRpc.Generator.GenerationHelpers.Code;

public static class GeneralCode
{
    public const string MessengerParameterLine = $"{MessengerType} {MessengerParameter},";
    public const string LoggerFactoryInterfaceParameter = $"{LoggerFactoryInterfaceType} {LoggerFactoryParameter},";
}