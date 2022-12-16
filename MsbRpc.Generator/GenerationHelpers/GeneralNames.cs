namespace MsbRpc.Generator.GenerationHelpers;

public static class GeneralNames
{
    public const string LoggerFactoryInterfaceType = "Microsoft.Extensions.Logging.ILoggerFactory";
    public const string LoggerFactoryParameter = "loggerFactory";

    public const string MessengerType = "MsbRpc.Messaging.Messenger";
    public const string MessengerParameter = "messenger";

    public const string CreateLoggerMethod = "Microsoft.Extensions.Logging.LoggerFactoryExtensions.CreateLogger";
    public const string NullLoggerType = "Microsoft.Extensions.Logging.Abstractions.NullLogger";
    public const string GeneratedFileEnding = ".g.cs";
    public const string InterfacePrefix = "I";
}