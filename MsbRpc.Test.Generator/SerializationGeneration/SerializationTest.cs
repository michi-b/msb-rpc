using System;
using System.CodeDom.Compiler;
using System.Collections.Immutable;
using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MsbRpc.Generator.Info;
using MsbRpc.Generator.Serialization;

namespace MsbRpc.Test.Generator.SerializationGeneration;

internal readonly ref struct SerializationTest
{
    private readonly SerializationResolver _serializationResolver;
    public string? ExpectedSizeExpression { get; init; }
    public string? ExpectedSerializationStatement { get; init; }
    public string? ExpectedDeserializationExpression { get; init; }
    public string? ExpectedDeclarationSyntax { get; init; }
    public bool ExpectedIsVoid { get; init; }

    public TypeReferenceInfo TargetType { get; }
    public string TargetExpression { get; init; } = "target";

    public string ValueExpression { get; init; } = "value";

    public string BufferWriterExpression { get; init; } = "bufferWriter";

    public string BufferReaderExpression { get; init; } = "bufferReader";

    public SerializationTest(TypeReferenceInfo targetType)
        : this(new SerializationResolver(ImmutableArray<CustomSerializationInfo>.Empty), targetType) { }

    public SerializationTest(SerializationResolver serializationResolver, TypeReferenceInfo targetType)
    {
        _serializationResolver = serializationResolver;
        TargetType = targetType;
        ExpectedSizeExpression = null;
        ExpectedSerializationStatement = null;
        ExpectedDeserializationExpression = null;
        ExpectedDeclarationSyntax = null;
        ExpectedIsVoid = false;
    }

    public void Run()
    {
        ISerialization serialization = _serializationResolver.Resolve(TargetType);
        Assert.IsTrue(serialization.GetIsResolved());
        Assert.AreEqual(ExpectedIsVoid, serialization.GetIsVoid());
        if (ExpectedDeclarationSyntax != null)
        {
            Assert.AreEqual(ExpectedDeclarationSyntax, serialization.GetDeclarationSyntax());
        }

        if (ExpectedSizeExpression != null)
        {
            Assert.AreEqual(ExpectedSizeExpression, GetSizeExpression(serialization, TargetExpression));
        }

        if (ExpectedSerializationStatement != null)
        {
            Assert.AreEqual(ExpectedSerializationStatement, GetSerializationStatement(serialization, BufferWriterExpression, ValueExpression));
        }

        if (ExpectedDeserializationExpression != null)
        {
            Assert.AreEqual(ExpectedDeserializationExpression, GetDeserializationExpression(serialization, BufferReaderExpression));
        }
    }

    public string GetSizeExpression() => GetSizeExpression(_serializationResolver.Resolve(TargetType), TargetExpression);
    public string GetDeserializationExpression() => GetDeserializationExpression(_serializationResolver.Resolve(TargetType), BufferReaderExpression);
    public string GetSerializationStatement() => GetSerializationStatement(_serializationResolver.Resolve(TargetType), BufferWriterExpression, ValueExpression);

    private static string GetSizeExpression(ISerialization serialization, string targetExpression)
    {
        return GetCode(writer => serialization.WriteSizeExpression(writer, targetExpression));
    }

    private static string GetDeserializationExpression(ISerialization serialization, string bufferReaderExpression)
    {
        return GetCode(writer => serialization.WriteDeserializationExpression(writer, bufferReaderExpression));
    }

    private static string GetSerializationStatement(ISerialization serialization, string bufferWriterExpression, string valueExpression)
    {
        return GetCode(writer => serialization.WriteSerializationStatement(writer, bufferWriterExpression, valueExpression));
    }

    private static string GetCode(Action<IndentedTextWriter> write)
    {
        IndentedTextWriter textWriter = CreateTextWriter();
        write(textWriter);
        return GetTextWriterResult(textWriter) ?? string.Empty;
    }

    private static string? GetTextWriterResult(IndentedTextWriter textWriter) => textWriter.InnerWriter.ToString();
    private static IndentedTextWriter CreateTextWriter() => new(new StringWriter());
}