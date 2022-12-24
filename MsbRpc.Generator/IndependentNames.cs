namespace MsbRpc.Generator;

internal static class IndependentNames
{
    public const string GeneratedFileEnding = ".g.cs";
    public const string InterfacePrefix = "I";
    public const string AsyncSuffix = "Async";
    public const string ProcedurePostfix = "Procedure";
    public const string PrivateFieldPrefix = "_";

    public static class Types
    {
        public const string LoggerFactoryInterface = "Microsoft.Extensions.Logging.ILoggerFactory";
        public const string Messenger = "MsbRpc.Messaging.Messenger";
        public const string NullLogger = "Microsoft.Extensions.Logging.Abstractions.NullLogger";
        public const string VaLueTask = "System.Threading.Tasks.ValueTask";
        public const string CancellationToken = "System.Threading.CancellationToken";
        public const string PrimitiveSerializer = "MsbRpc.Serialization.Primitives.PrimitiveSerializer";
        public const string ArgumentOutOfRangeException = "System.ArgumentOutOfRangeException";
        public const string BufferWriter = "MsbRpc.Serialization.BufferWriter";
        public const string BufferReader = "MsbRpc.Serialization.BufferReader";
        public const string EndPointBaseType = "MsbRpc.EndPoints.RpcEndPoint";
        public const string UndefinedProcedureEnum = "MsbRpc.EndPoints.UndefinedProcedure";
    }

    public static class Methods
    {
        public const string CreateLogger = "Microsoft.Extensions.Logging.LoggerFactoryExtensions.CreateLogger";
        public const string EndPointEnterCalling = "EnterCalling";
        public const string EndPointExitCalling = "ExitCalling";
        public const string GetEndPointRequestWriter = "GetRequestWriter";
        public const string GetEndPointResponseWriter = "GetResponseWriter";
    }

    public static class Parameters
    {
        public const string LoggerFactory = "loggerFactory";
        public const string Messenger = "messenger";
        public const string CancellationToken = "cancellationToken";
    }

    public static string ToLowerFirstChar(this string identifier)
    {
        char firstChar = char.ToLowerInvariant(identifier[0]);
        return firstChar + identifier.Substring(1);
    }
    
    public static string ToUpperFirstChar(this string identifier)
    {
        char firstChar = char.ToUpperInvariant(identifier[0]);
        return firstChar + identifier.Substring(1);
    }
}