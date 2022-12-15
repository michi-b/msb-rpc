using MsbRpc.Serialization.Primitives;

namespace MsbRpcTest.Serialization.Primitives;

public static class PrimitivesUtility
{
    public static int GetSize(string typeName)
    {
        return typeName switch
        {
            "System.Boolean" => PrimitiveSerializer.BooleanSize,
            "System.Byte" => PrimitiveSerializer.ByteSize,
            "System.Char" => PrimitiveSerializer.CharSize,
            "System.Decimal" => PrimitiveSerializer.DecimalSize,
            "System.Double" => PrimitiveSerializer.DoubleSize,
            "System.Int16" => PrimitiveSerializer.Int16Size,
            "System.Int32" => PrimitiveSerializer.Int32Size,
            "System.Int64" => PrimitiveSerializer.Int64Size,
            "System.SByte" => PrimitiveSerializer.SByteSize,
            "System.Single" => PrimitiveSerializer.SingleSize,
            "System.UInt16" => PrimitiveSerializer.UInt16Size,
            "System.UInt32" => PrimitiveSerializer.UInt32Size,
            "System.UInt64" => PrimitiveSerializer.UInt64Size,
            _ => throw new NotSupportedException()
        };
    }
}