using System;
using System.CodeDom.Compiler;

namespace MsbRpc.Generator.Serialization.Serializations;

public sealed class VoidSerialization : ISerialization
{
    public void WriteSizeExpression(IndentedTextWriter writer, string targetExpression) => throw new InvalidOperationException();

    public void WriteSerializationStatement(IndentedTextWriter writer, string bufferWriterExpression, string valueExpression) => throw new InvalidOperationException();

    public void WriteDeserializationExpression(IndentedTextWriter writer, string bufferReaderExpression) => throw new InvalidOperationException();

    public bool GetIsVoid() => true;
    public bool GetIsResolved() => true;

    public string GetDeclarationSyntax() => throw new InvalidOperationException();
}