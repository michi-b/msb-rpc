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
        public const string GetNameProcedureEnumExtension = "GetName";
        public const string GetInvertsDirectionProcedureExtension = "GetInvertsDirection";
    }

    public static class Parameters
    {
        public const string LoggerFactory = "loggerFactory";
        public const string Messenger = "messenger";
        public const string CancellationToken = "cancellationToken";
        public const string Procedure = "procedure";
    }

    public static string ToCamelCase(this string pascalCase)
    {
        char firstChar = char.ToLowerInvariant(pascalCase[0]);
        return firstChar + pascalCase.Substring(1);
    }
    
    public static string ToPascalCase(this string camelCase)
    {
        char firstChar = char.ToUpperInvariant(camelCase[0]);
        return firstChar + camelCase.Substring(1);
    }
}