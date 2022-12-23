using MsbRpc.Generator.Info;

namespace MsbRpc.Generator.GenerationHelpers.ReusedNames;

public class ProcedureNames
{
    public ProcedureNames(ProcedureInfo info)
    {
        
    }

    public static class Variables
    {
        public const string RequestWriter = "requestWriter";
        public const string ResponseReader = "responseReader";
        public const string ConstantArgumentsSizeSum = "constantSerializedSize";
        public const string Procedure = "procedure";
        public const string Response = "response";
    }

    public static class Properties
    {
        public const string BufferWriterBufferProperty = "Buffer";
    }

    public static class Types
    {
        public const string BufferWriter = "MsbRpc.Serialization.Buffers.BufferWriter";
        public const string BufferReader = "MsbRpc.Serialization.Buffers.BufferReader";
    }

    public static class Methods
    {
        public const string BufferWriterWrite = "Write";
        public const string SendRequest = "SendRequest";
    }
}