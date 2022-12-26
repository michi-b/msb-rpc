namespace MsbRpc.Generator;

internal static class IndependentNames
{
    public const string InterfacePrefix = "I";
    public const string PrivateFieldPrefix = "_";
    public const string AsyncPostFix = "Async";
    public const string ProcedurePostfix = "Procedure";
    public const string ExtensionsPostFix = "Extensions";
    public const string GeneratedFilePostfix = ".g.cs";

    public static class Types
    {
        public const string LoggerFactoryInterface = "Microsoft.Extensions.Logging.ILoggerFactory";
        public const string Messenger = "MsbRpc.Messaging.Messenger";
        public const string NullLogger = "Microsoft.Extensions.Logging.Abstractions.NullLogger";
        public const string Task = "System.Threading.Tasks.Task";
        public const string VaLueTask = "System.Threading.Tasks.ValueTask";
        public const string CancellationToken = "System.Threading.CancellationToken";
        public const string PrimitiveSerializer = "MsbRpc.Serialization.Primitives.PrimitiveSerializer";
        public const string ArgumentOutOfRangeException = "System.ArgumentOutOfRangeException";
        public const string BufferWriter = "MsbRpc.Serialization.BufferWriter";
        public const string BufferReader = "MsbRpc.Serialization.BufferReader";
        public const string EndPoint = "MsbRpc.EndPoints.RpcEndPoint";
        public const string UndefinedProcedureEnum = "MsbRpc.EndPoints.UndefinedProcedure";
        public const string EndPointDirection = "MsbRpc.EndPoints.EndPointDirection";
    }

    public static class Methods
    {
        public const string CreateLogger = "Microsoft.Extensions.Logging.LoggerFactoryExtensions.CreateLogger";
        public const string EndPointEnterCalling = "EnterCalling";
        public const string EndPointExitCalling = "ExitCalling";
        public const string GetEndPointRequestWriter = "GetRequestWriter";
        public const string GetEndPointResponseWriter = "GetResponseWriter";
        public const string GetNameProcedureEnumExtension = "GetName";
        public const string GetInvertsDirectionProcedureExtension = "GetInvertsDirection";
    }

    public static class Parameters
    {
        public const string LoggerFactory = "loggerFactory";
        public const string Messenger = "messenger";
        public const string CancellationToken = "cancellationToken";
        public const string Procedure = "procedure";
        public const string InitialBufferSize = "initialBufferSize";
        public const string ArgumentsBufferReader = "arguments";
    }
    
    public static class EnumValues
    {
        public const string InboundEndPointDirection = Types.EndPointDirection + ".Inbound";
        public const string OutboundEndPointDirection = Types.EndPointDirection + ".Outbound";
    }

    public static string ToCamelCase(this string target)
    {
        char firstChar = target[0];

        if (!char.IsLower(firstChar))
        {
            char firstCharLower = char.ToLowerInvariant(firstChar);
            return firstCharLower + target.Substring(1);
        }

        return target;
    }

    public static string ToPascalCase(this string target)
    {
        char firstChar = target[0];

        if (!char.IsUpper(firstChar))
        {
            char firstCharUpper = char.ToUpperInvariant(firstChar);
            return firstCharUpper + target.Substring(1);
        }

        return target;
    }
}