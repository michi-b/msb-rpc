namespace MsbRpc.Generator.CodeWriters.Utility;

internal static class IndependentNames
{
    public const string InterfacePrefix = "I";
    public const string GeneratedFilePostfix = ".g.cs";
    public const string GeneratedNamespacePostFix = ".Generated";
    public const string EndPointPostfix = "EndPoint";
    public const string ArgumentPostfix = "Argument";
    public const string ExtensionsPostFix = "Extensions";
    public const string ProcedurePostfix = "Procedure";
    public const string SizePostfix = "Size";
    public const string AsyncPostFix = "Async";

    public static class Namespaces
    {
        public const string MsbRpcSerialization = "MsbRpc.Serialization";
    }

    public static class Types
    {
        // system types
        public const string Action = "System.Action";
        public const string Func = "System.Func";
        public const string VaLueTask = "System.Threading.Tasks.ValueTask";
        public const string ArgumentOutOfRangeException = "System.ArgumentOutOfRangeException";
        public const string IPAddress = "System.Net.IPAddress";
        public const string IPEndPoint = "System.Net.IPEndPoint";

        // logging types
        public const string LoggerFactoryInterface = "Microsoft.Extensions.Logging.ILoggerFactory";

        // messaging types
        public const string Messenger = "MsbRpc.Messaging.Messenger";
        public const string MessengerFactory = "MsbRpc.Messaging.MessengerFactory";

        // serialization types
        public const string BufferWriter = "MsbRpc.Serialization.Buffers.BufferWriter";
        public const string BufferReader = "MsbRpc.Serialization.Buffers.BufferReader";
        public const string Response = "MsbRpc.Serialization.Buffers.Response";
        public const string Request = "MsbRpc.Serialization.Buffers.Request";
        public const string PrimitiveSerializer = "MsbRpc.Serialization.Primitives.PrimitiveSerializer";
        public const string StringSerializer = "MsbRpc.Serialization.StringSerializer";
        public const string NullableStringSerializer = "MsbRpc.Serialization.NullableStringSerializer";

        // endpoint types
        public const string InboundEndPoint = "MsbRpc.EndPoints.InboundEndPoint";
        public const string OutboundEndPoint = "MsbRpc.EndPoints.OutboundEndPoint";
        public const string EndPointDirection = "MsbRpc.EndPoints.EndPointDirection";
        public const string Server = "MsbRpc.Server.Server";

        // configuration
        public const string InboundEndPointConfiguration = "MsbRpc.Configuration.InboundEndPointConfiguration";
        public const string OutboundEndPointConfiguration = "MsbRpc.Configuration.OutboundEndPointConfiguration";
        public const string ServerConfiguration = "MsbRpc.Configuration.ServerConfiguration";

        // local types
        public const string LocalConfiguration = "Configuration";
        public const string LocalConfigurationConfigureAction = $"{Action}<{LocalConfiguration}>";
    }

    public static class Methods
    {
        public const string GetNameProcedureExtension = "GetName";
        public const string GetIdProcedureExtension = "GetId";
        public const string FromIdProcedureExtension = "FromId";

        // endpoints methods
        public const string InboundEndPointExecute = "Execute";
        public const string GetProcedure = "GetProcedure";
        public const string GetProcedureName = "GetName";
        public const string GetProcedureId = "GetId";
        public const string ConnectAsync = "ConnectAsync";
        public const string AssertIsOperable = "AssertIsOperable";

        // buffer methods
        public const string BufferWrite = "Write";
        public const string GetReader = "GetReader";
        public const string GetWriter = "GetWriter";
        public const string GetResponse = "GetResponse";
        public const string GetRequest = "GetRequest";
        public const string SendRequestAsync = "SendRequestAsync";
        public const string Invoke = "Invoke";
        public const string Start = "Start";
    }

    public static class Parameters
    {
        public const string LoggerFactory = "loggerFactory";
        public const string Messenger = "messenger";
        public const string Procedure = "procedure";
        public const string ProcedureId = "procedureId";
        public const string ContractImplementation = "implementation";
        public const string Request = "request";
        public const string IPAddress = "address";
        public const string IPEndPoint = "endPoint";
        public const string Port = "port";
        public const string Configuration = "configuration";
        public const string EndPointConfiguration = "endPointConfiguration";
        public const string ConfigureAction = "configure";
        public const string EndPointConfigureAction = "configureEndPoint";
        public const string ContractImplementationFactory = "createImplementation";
    }

    public static class Properties
    {
        public const string RanToCompletion = "RanToCompletion";
    }

    public static class Fields
    {
        public const string EndPointBuffer = "Buffer";
        public const string InboundEndpointImplementation = "Implementation";
        public const string ImplementationFactory = "_createImplementation";
        public const string EndPointConfiguration = "_endPointConfiguration";
        public const string Configuration = "_configuration";
    }

    public static class EnumValues
    {
        public const string InboundEndPointDirection = Types.EndPointDirection + ".Inbound";
        public const string OutboundEndPointDirection = Types.EndPointDirection + ".Outbound";
    }

    public static class Variables
    {
        #region EndPoints

        public const string Result = "result";
        public const string Response = "response";
        public const string Request = "request";
        public const string RequestWriter = "requestWriter";
        public const string RequestSize = "requestSize";
        public const string RequestReader = "requestReader";
        public const string ResponseReader = "responseReader";
        public const string ResultSize = "resultSize";
        public const string ResponseWriter = "responseWriter";
        public const string Messenger = "messenger";
        public const string ArgumentSizeSum = "argumentSizeSum";
        public const string Configuration = "configuration";
        public const string EndPointConfiguration = "endPointConfiguration";
        public const string Server = "server";

        #endregion
    }

    public static string PascalToCamelCase(this string target)
    {
        char firstChar = target[0];

        if (!char.IsLower(firstChar))
        {
            char firstCharLower = char.ToLowerInvariant(firstChar);
            return firstCharLower + target.Substring(1);
        }

        return target;
    }

    public static string CamelToPascalCase(this string target)
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