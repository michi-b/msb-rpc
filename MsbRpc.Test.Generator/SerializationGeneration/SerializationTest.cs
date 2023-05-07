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

    public void Run(TestContext? testContext = null)
    {
        ISerialization serialization = _serializationResolver.Resolve(TargetType);
        testContext?.WriteLine($"Serialization has type {serialization.GetType()}");

        bool isResolved = serialization.GetIsResolved();
        testContext?.WriteLine($"Is resolved: {isResolved}");
        Assert.IsTrue(isResolved);

        bool isVoid = serialization.GetIsVoid();
        testContext?.WriteLine($"Is void: {isVoid}");
        Assert.AreEqual(ExpectedIsVoid, isVoid);
        
        string declarationSyntax = serialization.GetDeclarationSyntax();
        testContext?.WriteLine($"Declaration syntax: {declarationSyntax}");
        if (ExpectedDeclarationSyntax != null)
        {
            Assert.AreEqual(ExpectedDeclarationSyntax, declarationSyntax);
        }

        string sizeExpression = GetSizeExpression(serialization, TargetExpression);
        testContext?.WriteLine($"Size expression: {sizeExpression}");
        if (ExpectedSizeExpression != null)
        {
            Assert.AreEqual(ExpectedSizeExpression, sizeExpression);
        }

        string serializationStatement = GetSerializationStatement(serialization, BufferWriterExpression, ValueExpression);
        testContext?.WriteLine($"Serialization statement: {serializationStatement}");
        if (ExpectedSerializationStatement != null)
        {
            Assert.AreEqual(ExpectedSerializationStatement, serializationStatement);
        }

        string deserializationExpression = GetDeserializationExpression(serialization, BufferReaderExpression);
        testContext?.WriteLine($"Deserialization expression: {deserializationExpression}");
        if (ExpectedDeserializationExpression != null)
        {
            Assert.AreEqual(ExpectedDeserializationExpression, deserializationExpression);
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