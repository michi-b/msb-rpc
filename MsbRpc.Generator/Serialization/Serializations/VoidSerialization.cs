#region

using System;
using System.CodeDom.Compiler;

#endregion

namespace MsbRpc.Generator.Serialization.Serializations;

public sealed class VoidSerialization : ISerialization
{
    public bool IsVoid => true;

    public bool IsResolved => true;
    public bool IsConstantSize => true;

    public string DeclarationSyntax => throw new InvalidOperationException();
    public bool NeedsSemicolonAfterSerializationStatement => throw new InvalidOperationException();

    public void WriteSizeExpression(IndentedTextWriter writer, string targetExpression) => throw new InvalidOperationException();

    public void WriteSerializationStatement(IndentedTextWriter writer, string bufferWriterExpression, string valueExpression) => throw new InvalidOperationException();

    public void WriteDeserializationExpression(IndentedTextWriter writer, string bufferReaderExpression) => throw new InvalidOperationException();
}