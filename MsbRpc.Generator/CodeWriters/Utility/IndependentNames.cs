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
        // basic system types
        public const string VaLueTask = "System.Threading.Tasks.ValueTask";
        public const string ArgumentOutOfRangeException = "System.ArgumentOutOfRangeException";
        public const string IPEndPoint = "System.Net.IPEndPoint";
        public const string Void = "System.Void";

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

        // endpoint types
        public const string InboundEndPoint = "MsbRpc.EndPoints.InboundEndPoint";
        public const string OutboundEndPoint = "MsbRpc.EndPoints.OutboundEndPoint";
        public const string EndPointDirection = "MsbRpc.EndPoints.EndPointDirection";

        // configuration
        public const string InboundEndPointConfiguration = "MsbRpc.Configuration.InboundEndPointConfiguration";
        public const string OutboundEndPointConfiguration = "MsbRpc.Configuration.OutboundEndPointConfiguration";

        // exceptions
        public const string Exception = "System.Exception";
        public const string RpcExecutionException = "MsbRpc.Exceptions.RpcExecutionException";

        private const string RpcExecutionStage = "MsbRpc.Contracts.RpcExecutionStage";
        public const string RpcExecutionStageArgumentDeserialization = RpcExecutionStage + ".ArgumentDeserialization";
        public const string RpcExecutionStageImplementationExecution = RpcExecutionStage + ".ImplementationExecution";
        public const string RpcExecutionStageResponseSerialization = RpcExecutionStage + ".ResponseSerialization";
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
        public const string GetReader = "GetReader";
        public const string GetWriter = "GetWriter";
        public const string GetResponse = "GetResponse";
        public const string GetRequest = "GetRequest";
        public const string SendRequestAsync = "SendRequestAsync";
        public const string BufferWriterWrite = "Write";

        //buffer reader read methods
        public const string BufferReaderReadByte = "ReadByte";
        public const string BufferReaderReadSByte = "ReadSByte";
        public const string BufferReaderReadBool = "ReadBool";
        public const string BufferReaderReadChar = "ReadChar";
        public const string BufferReaderReadInt = "ReadInt";
        public const string BufferReaderReadLong = "ReadLong";
        public const string BufferReaderReadShort = "ReadShort";
        public const string BufferReaderReadUInt = "ReadUInt";
        public const string BufferReaderReadULong = "ReadULong";
        public const string BufferReaderReadUShort = "ReadUShort";
        public const string BufferReaderReadFloat = "ReadFloat";
        public const string BufferReaderReadDouble = "ReadDouble";
        public const string BufferReaderReadDecimal = "ReadDecimal";
        public const string BufferReaderReadString = "ReadString";
    }

    public static class Parameters
    {
        public const string Messenger = "messenger";
        public const string Procedure = "procedure";
        public const string ProcedureId = "procedureId";
        public const string ContractImplementation = "implementation";
        public const string Request = "request";
        public const string IPEndPoint = "endPoint";
        public const string Configuration = "configuration";
    }

    public static class Properties
    {
        public const string RanToCompletion = "RanToCompletion";
    }

    public static class Fields
    {
        public const string EndPointBuffer = "Buffer";
        public const string InboundEndpointImplementation = "Implementation";
        public const string PrimitiveSerializerBoolSize = Types.PrimitiveSerializer + ".BoolSize";
    }

    public static class GlobalConstants
    {
        public const string ByteSize = Types.PrimitiveSerializer + ".ByteSize";
        public const string SByteSize = Types.PrimitiveSerializer + ".SByteSize";
        public const string BoolSize = Types.PrimitiveSerializer + ".BoolSize";
        public const string CharSize = Types.PrimitiveSerializer + ".CharSize";
        public const string IntSize = Types.PrimitiveSerializer + ".IntSize";
        public const string LongSize = Types.PrimitiveSerializer + ".LongSize";
        public const string ShortSize = Types.PrimitiveSerializer + ".ShortSize";
        public const string UIntSize = Types.PrimitiveSerializer + ".UIntSize";
        public const string ULongSize = Types.PrimitiveSerializer + ".ULongSize";
        public const string UShortSize = Types.PrimitiveSerializer + ".UShortSize";
        public const string FloatSize = Types.PrimitiveSerializer + ".FloatSize";
        public const string DoubleSize = Types.PrimitiveSerializer + ".DoubleSize";
        public const string DecimalSize = Types.PrimitiveSerializer + ".DecimalSize";
    }

    public static class EnumValues
    {
        public const string InboundEndPointDirection = Types.EndPointDirection + ".Inbound";
        public const string OutboundEndPointDirection = Types.EndPointDirection + ".Outbound";
    }

    public static class Variables
    {
        public const string Result = "result";
        public const string Response = "response";
        public const string Request = "request";
        public const string RequestWriter = "requestWriter";
        public const string RequestReader = "requestReader";
        public const string ResponseReader = "responseReader";
        public const string ResultSize = "resultSize";
        public const string ResponseWriter = "responseWriter";
        public const string Messenger = "messenger";
        public const string ArgumentSizeSum = "argumentSizeSum";
        public const string Exception = "exception";
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