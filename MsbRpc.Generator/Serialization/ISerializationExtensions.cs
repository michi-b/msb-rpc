using System.CodeDom.Compiler;
using MsbRpc.Generator.Extensions;

namespace MsbRpc.Generator.Serialization;

public static class SerializationExtensions
{
    public static bool GetCanHandleRpcArguments(this ISerialization serialization) => serialization.IsResolved && !serialization.IsVoid;

    public static bool GetCanHandleRpcResults(this ISerialization serialization) => serialization.IsResolved;

    /// <summary>
    ///     same as <see cref="ISerialization.WriteSerializationStatement" />, but with trailing semicolon and new line
    /// </summary>
    public static void WriteFinalizedSerializationStatement
    (
        this ISerialization serialization,
        IndentedTextWriter writer,
        string bufferWriterExpression,
        string valueExpression
    )
    {
        serialization.WriteSerializationStatement(writer, bufferWriterExpression, valueExpression);
        writer.WriteSemicolonLineBreak();
    }

    /// <summary>
    ///     same as <see cref="MsbRpc.Generator.Serialization.ISerialization.WriteDeserializationExpression" />, but with
    ///     trailing semicolon and new line
    /// </summary>
    public static void WriteFinalizedDeserializationExpression
    (
        this ISerialization serialization,
        IndentedTextWriter writer,
        string bufferReaderExpression
    )
    {
        serialization.WriteDeserializationExpression(writer, bufferReaderExpression);
        writer.WriteSemicolonLineBreak();
    }
}