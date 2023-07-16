#region

using System;
using MsbRpc.Generator.Info;

#endregion

namespace MsbRpc.Generator.Serialization.Default;

public static class SimpleDefaultSerializationKindExtensions
{
    private static readonly TypeReferenceInfo ByteTypeReferenceInfo = TypeReferenceInfo.CreateSimple("System.Byte");
    private static readonly TypeReferenceInfo SByteTypeReferenceInfo = TypeReferenceInfo.CreateSimple("System.SByte");
    private static readonly TypeReferenceInfo BoolType = TypeReferenceInfo.CreateSimple("System.Boolean");
    private static readonly TypeReferenceInfo CharType = TypeReferenceInfo.CreateSimple("System.Char");
    private static readonly TypeReferenceInfo IntType = TypeReferenceInfo.CreateSimple("System.Int32");
    private static readonly TypeReferenceInfo LongType = TypeReferenceInfo.CreateSimple("System.Int64");
    private static readonly TypeReferenceInfo ShortType = TypeReferenceInfo.CreateSimple("System.Int16");
    private static readonly TypeReferenceInfo UintType = TypeReferenceInfo.CreateSimple("System.UInt32");
    private static readonly TypeReferenceInfo UlongType = TypeReferenceInfo.CreateSimple("System.UInt64");
    private static readonly TypeReferenceInfo UshortType = TypeReferenceInfo.CreateSimple("System.UInt16");
    private static readonly TypeReferenceInfo FloatType = TypeReferenceInfo.CreateSimple("System.Single");
    private static readonly TypeReferenceInfo DoubleType = TypeReferenceInfo.CreateSimple("System.Double");
    private static readonly TypeReferenceInfo DecimalType = TypeReferenceInfo.CreateSimple("System.Decimal");
    private static readonly TypeReferenceInfo StringType = TypeReferenceInfo.CreateSimple("System.String");

    public static TypeReferenceInfo GetTargetType(this SimpleDefaultSerializationKind target)
    {
        return target switch
        {
            SimpleDefaultSerializationKind.Byte => ByteTypeReferenceInfo,
            SimpleDefaultSerializationKind.Sbyte => SByteTypeReferenceInfo,
            SimpleDefaultSerializationKind.Bool => BoolType,
            SimpleDefaultSerializationKind.Char => CharType,
            SimpleDefaultSerializationKind.Int => IntType,
            SimpleDefaultSerializationKind.Long => LongType,
            SimpleDefaultSerializationKind.Short => ShortType,
            SimpleDefaultSerializationKind.Uint => UintType,
            SimpleDefaultSerializationKind.Ulong => UlongType,
            SimpleDefaultSerializationKind.Ushort => UshortType,
            SimpleDefaultSerializationKind.Float => FloatType,
            SimpleDefaultSerializationKind.Double => DoubleType,
            SimpleDefaultSerializationKind.Decimal => DecimalType,
            SimpleDefaultSerializationKind.String => StringType,
            _ => throw new ArgumentOutOfRangeException(nameof(target), target, null)
        };
    }
}