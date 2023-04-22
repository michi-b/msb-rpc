using System;
using System.Diagnostics;
using static MsbRpc.Generator.CodeWriters.Utility.IndependentNames;

namespace MsbRpc.Generator.Info;

internal static class SerializationKindExtensions
{
    private const string PascalCaseNullable = "Nullable";

    public static string? GetKeyword(this SerializationKind serializationKind)
    {
        return serializationKind switch
        {
            SerializationKind.Byte => "byte",
            SerializationKind.Sbyte => "sbyte",
            SerializationKind.Bool => "bool",
            SerializationKind.Char => "char",
            SerializationKind.Int => "int",
            SerializationKind.Long => "long",
            SerializationKind.Short => "short",
            SerializationKind.Uint => "uint",
            SerializationKind.Ulong => "ulong",
            SerializationKind.Ushort => "ushort",
            SerializationKind.Float => "float",
            SerializationKind.Double => "double",
            SerializationKind.Decimal => "decimal",
            SerializationKind.Void => "void",
            SerializationKind.String => "string",
            _ => null
        };
    }

    public static bool GetIsBufferReadAndWritable(this SerializationKind serializationKind)
    {
        return serializationKind switch
        {
            SerializationKind.Byte => true,
            SerializationKind.Sbyte => true,
            SerializationKind.Bool => true,
            SerializationKind.Char => true,
            SerializationKind.Int => true,
            SerializationKind.Long => true,
            SerializationKind.Short => true,
            SerializationKind.Uint => true,
            SerializationKind.Ulong => true,
            SerializationKind.Ushort => true,
            SerializationKind.Float => true,
            SerializationKind.Double => true,
            SerializationKind.Decimal => true,
            SerializationKind.String => true,
            SerializationKind.Void => false,
            _ => false
        };
    }

    public static bool GetIsValidReturnType(this SerializationKind target)
    {
        return target switch
        {
            SerializationKind.Byte => true,
            SerializationKind.Sbyte => true,
            SerializationKind.Bool => true,
            SerializationKind.Char => true,
            SerializationKind.Int => true,
            SerializationKind.Long => true,
            SerializationKind.Short => true,
            SerializationKind.Uint => true,
            SerializationKind.Ulong => true,
            SerializationKind.Ushort => true,
            SerializationKind.Float => true,
            SerializationKind.Double => true,
            SerializationKind.Decimal => true,
            SerializationKind.String => true,
            SerializationKind.Void => true,
            _ => false
        };
    }

    public static bool GetIsConstantSize(this SerializationKind target)
    {
        return target switch
        {
            SerializationKind.Byte => true,
            SerializationKind.Sbyte => true,
            SerializationKind.Bool => true,
            SerializationKind.Char => true,
            SerializationKind.Int => true,
            SerializationKind.Long => true,
            SerializationKind.Short => true,
            SerializationKind.Uint => true,
            SerializationKind.Ulong => true,
            SerializationKind.Ushort => true,
            SerializationKind.Float => true,
            SerializationKind.Double => true,
            SerializationKind.Decimal => true,
            SerializationKind.String => false,
            SerializationKind.Void => true,
            _ => false
        };
    }

    public static string? GetBufferReaderReadMethodName(this SerializationKind target, bool isNullable)
    {
        return target switch
        {
            SerializationKind.Byte => target.GetBufferReaderReadPrimitiveMethodName(isNullable),
            SerializationKind.Sbyte => target.GetBufferReaderReadPrimitiveMethodName(isNullable),
            SerializationKind.Bool => target.GetBufferReaderReadPrimitiveMethodName(isNullable),
            SerializationKind.Char => target.GetBufferReaderReadPrimitiveMethodName(isNullable),
            SerializationKind.Int => target.GetBufferReaderReadPrimitiveMethodName(isNullable),
            SerializationKind.Long => target.GetBufferReaderReadPrimitiveMethodName(isNullable),
            SerializationKind.Short => target.GetBufferReaderReadPrimitiveMethodName(isNullable),
            SerializationKind.Uint => target.GetBufferReaderReadPrimitiveMethodName(isNullable),
            SerializationKind.Ulong => target.GetBufferReaderReadPrimitiveMethodName(isNullable),
            SerializationKind.Ushort => target.GetBufferReaderReadPrimitiveMethodName(isNullable),
            SerializationKind.Float => target.GetBufferReaderReadPrimitiveMethodName(isNullable),
            SerializationKind.Double => target.GetBufferReaderReadPrimitiveMethodName(isNullable),
            SerializationKind.Decimal => target.GetBufferReaderReadPrimitiveMethodName(isNullable),
            SerializationKind.String => isNullable ? "ReadStringNullable" : "ReadString",
            _ => null
        };
    }

    public static string? GetBufferWriterWriteMethodName(this SerializationKind target, bool isNullable)
        => target.GetIsBufferReadAndWritable()
            ? isNullable
                ? "WriteNullable"
                : "Write"
            : null;

    public static string GetName(this SerializationKind target)
    {
        return target switch
        {
            SerializationKind.Unresolved => nameof(SerializationKind.Unresolved),
            SerializationKind.Byte => nameof(SerializationKind.Byte),
            SerializationKind.Sbyte => nameof(SerializationKind.Sbyte),
            SerializationKind.Bool => nameof(SerializationKind.Bool),
            SerializationKind.Char => nameof(SerializationKind.Char),
            SerializationKind.Int => nameof(SerializationKind.Int),
            SerializationKind.Long => nameof(SerializationKind.Long),
            SerializationKind.Short => nameof(SerializationKind.Short),
            SerializationKind.Uint => nameof(SerializationKind.Uint),
            SerializationKind.Ulong => nameof(SerializationKind.Ulong),
            SerializationKind.Ushort => nameof(SerializationKind.Ushort),
            SerializationKind.Float => nameof(SerializationKind.Float),
            SerializationKind.Double => nameof(SerializationKind.Double),
            SerializationKind.Decimal => nameof(SerializationKind.Decimal),
            SerializationKind.Void => nameof(SerializationKind.Void),
            SerializationKind.String => nameof(SerializationKind.String),
            _ => throw new ArgumentOutOfRangeException(nameof(target), target, null)
        };
    }

    public static string GetPrimitiveSizeExpression(this SerializationKind target, bool isNullable)
    {
        string? pascalCaseKeyword = target.GetPascalCaseKeyword();
        Debug.Assert(pascalCaseKeyword != null);
        return Types.PrimitiveSerializer + '.' +
               (isNullable
                   ? PascalCaseNullable + pascalCaseKeyword
                   : pascalCaseKeyword)
               + "Size";
    }

    private static string GetBufferReaderReadPrimitiveMethodName(this SerializationKind target, bool isNullable)
    {
        string? pascalCaseKeyword = target.GetPascalCaseKeyword();
        Debug.Assert(pascalCaseKeyword != null);
        string readNonNullable = "Read" + pascalCaseKeyword;
        return isNullable ? readNonNullable + PascalCaseNullable : readNonNullable;
    }

    private static string? GetPascalCaseKeyword(this SerializationKind target) => target.GetKeyword()?.CamelToPascalCase();
}