namespace MsbRpc.Generator.GenerationHelpers.Names;

public static class GeneralNames
{
    public static class Types
    {
        public const string LoggerFactoryInterface = "Microsoft.Extensions.Logging.ILoggerFactory";
        public const string Messenger = "MsbRpc.Messaging.Messenger";
        public const string NullLogger = "Microsoft.Extensions.Logging.Abstractions.NullLogger";
        public const string VaLueTask = "System.Threading.Tasks.ValueTask";
        public const string CancellationToken = "System.Threading.CancellationToken";
        public const string PrimitiveSerializer = "MsbRpc.Serialization.Primitives.PrimitiveSerializer";
        public const string BufferWriter = "MsbRpc.Serialization.Buffers.BufferWriter";
        public const string BufferReader = "MsbRpc.Serialization.Buffers.BufferReader";
    }
    
    public static class Methods
    {
        public const string CreateLogger = "Microsoft.Extensions.Logging.LoggerFactoryExtensions.CreateLogger";
    }

    public static class Parameters
    {
        public const string LoggerFactory = "loggerFactory";
        public const string Messenger = "messenger";
        public const string CancellationToken = "cancellationToken";
    }

    public const string GeneratedFileEnding = ".g.cs";
    public const string InterfacePrefix = "I";
    public const string AsyncSuffix = "Async";
}