using MsbRpc.Serialization.Primitives;

namespace MsbRpc.Test.Serialization.Primitives;

public static class PrimitivesUtility
{
    public static int GetSize(string typeName)
    {
        return typeName switch
        {
            "System.Boolean" => PrimitiveSerializer.BoolSize,
            "System.Byte" => PrimitiveSerializer.ByteSize,
            "System.Char" => PrimitiveSerializer.CharSize,
            "System.Decimal" => PrimitiveSerializer.DecimalSize,
            "System.Double" => PrimitiveSerializer.DoubleSize,
            "System.Int16" => PrimitiveSerializer.ShortSize,
            "System.Int32" => PrimitiveSerializer.IntSize,
            "System.Int64" => PrimitiveSerializer.LongSize,
            "System.SByte" => PrimitiveSerializer.SbyteSize,
            "System.Single" => PrimitiveSerializer.FloatSize,
            "System.UInt16" => PrimitiveSerializer.UshortSize,
            "System.UInt32" => PrimitiveSerializer.UintSize,
            "System.UInt64" => PrimitiveSerializer.UlongSize,
            _ => throw new NotSupportedException()
        };
    }
}