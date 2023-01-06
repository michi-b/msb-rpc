namespace MsbRpc.Generator.CodeWriters.Utility;

internal static class IndependentNames
{
    public const string InterfacePrefix = "I";
    public const string PrivateFieldPrefix = "_";
    public const string AsyncPostFix = "Async";
    public const string ProcedurePostfix = "Procedure";
    public const string ExtensionsPostFix = "Extensions";
    public const string GeneratedFilePostfix = ".g.cs";
    public const string ImplementationPostfix = "Implementation";
    public const string GeneratedNamespacePostFix = ".Generated";
    public const string ClientPostfix = "Client";
    public const string ServerPostfix = "Server";
    public const string EndPointPostfix = "EndPoint";

    public static class Types
    {
        public const string InboundEndPoint = "MsbRpc.EndPoints.InboundEndPoint";
        public const string OutboundEndPoint = "MsbRpc.EndPoints.OutboundEndPoint";
        public const string LoggerInterface = "Microsoft.Extensions.Logging.ILogger";
        public const string LoggerFactoryInterface = "Microsoft.Extensions.Logging.ILoggerFactory";
        public const string Messenger = "MsbRpc.Messaging.Messenger";
        public const string MessengerListenReturnCode = "MsbRpc.Messaging.Messenger.ListenReturnCode";
        public const string NullLogger = "Microsoft.Extensions.Logging.Abstractions.NullLogger";
        public const string Task = "System.Threading.Tasks.Task";
        public const string VaLueTask = "System.Threading.Tasks.ValueTask";
        public const string CancellationToken = "System.Threading.CancellationToken";
        public const string PrimitiveSerializer = "MsbRpc.Serialization.Primitives.PrimitiveSerializer";
        public const string ArgumentOutOfRangeException = "System.ArgumentOutOfRangeException";
        public const string BufferWriter = "MsbRpc.Serialization.Buffers.BufferWriter";
        public const string BufferReader = "MsbRpc.Serialization.Buffers.BufferReader";
        public const string UndefinedProcedureEnum = "MsbRpc.EndPoints.UndefinedProcedure";
        public const string EndPointDirection = "MsbRpc.EndPoints.EndPointDirection";
        public const string LocalEndPointResolver = "Resolver";
        public const string RpcResolverInterface = "MsbRpc.EndPoints.IRpcResolver";
        public const string BufferUtility = "MsbRpc.Serialization.Buffers.BufferUtility";
        public const string Response = "MsbRpc.Serialization.Buffers.Response";
        public const string Request = "MsbRpc.Serialization.Buffers.Request";
    }

    public static class Methods
    {
        public const string CreateLogger = "Microsoft.Extensions.Logging.LoggerFactoryExtensions.CreateLogger";
        public const string GetNameProcedureExtension = "GetName";
        public const string GetIdProcedureExtension = "GetId";
        public const string FromIdProcedureExtension = "FromId";
        public const string BufferWrite = "Write";

        //endpoints
        public const string EndPointEnterCalling = "EnterCalling";
        public const string EndPointExitCalling = "ExitCalling";
        public const string EndPointSendRequestAsync = "SendRequestAsync";
        public const string EndPointGetRequestWriter = "GetRequestWriter";
        public const string EndPointGetResponseWriter = "GetResponseWriter";
        public const string EndpointGetProcedureName = "GetName";
        public const string InboundEndPointExecute = "Execute";
        public const string GetReader = "GetReader";
    }

    public static class StaticMethods
    {
        public const string CreateLoggerOptional = "MsbRpc.Utility.LoggerFactoryExtensions.CreateLoggerOptional";
    }

    public static class Constants
    {
        public const string EndPointDefaultInitialBufferSize = "DefaultInitialBufferSize";
    }

    public static class Parameters
    {
        public const string LoggerFactory = "loggerFactory";
        public const string Logger = "logger";
        public const string Messenger = "messenger";
        public const string CancellationToken = "cancellationToken";
        public const string Procedure = "procedure";
        public const string ProcedureId = "procedureId";
        public const string InitialBufferSize = "initialBufferSize";
        public const string ArgumentsBufferReader = "arguments";
        public const string ContractImplementation = "implementation";
        public const string RpcEndPoint = "endPoint";
        public const string Request = "request";
    }

    public static class Fields
    {
        public const string RpcImplementation = "_implementation";
        public const string BufferEmpty = "Empty";
        public const string BufferWriterBuffer = "Buffer";
    }

    public static class PrivateFields
    {
        public const string Logger = "_logger";
    }

    public static class EnumValues
    {
        public const string InboundEndPointDirection = Types.EndPointDirection + ".Inbound";
        public const string OutboundEndPointDirection = Types.EndPointDirection + ".Outbound";
    }

    public static class Variables
    {
        public const string ArgumentPostfix = "Argument";
        public const string SizePostfix = "Size";
        public const string ProcedureResult = "result";
        public const string Procedure = "procedure";
        public const string ConstantSizeRpcParametersSize = "constantSizeParametersSizeSum";
        public const string ParametersSizeSum = "ParametersSizeSum";
        public const string RequestWriter = "requestWriter";
        public const string RequestReader = "requestReader";
        public const string ResponseReader = "responseReader";
        public const string ResponseSize = "responseSize";
        public const string ResponseWriter = "responseWriter";
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