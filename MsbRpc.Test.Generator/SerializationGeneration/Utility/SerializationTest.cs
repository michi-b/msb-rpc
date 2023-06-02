using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MsbRpc.Generator.Extensions;
using MsbRpc.Generator.Info;
using MsbRpc.Generator.Serialization;

namespace MsbRpc.Test.Generator.SerializationGeneration.Utility;

internal readonly ref struct SerializationTest
{
    private readonly SerializationResolver _serializationResolver;
    public string? ExpectedSizeExpression { get; init; }
    public string? ExpectedSerializationStatement { get; init; }
    public string? ExpectedDeserializationExpression { get; init; }
    public string? ExpectedDeclarationSyntax { get; init; }
    private bool ExpectedIsVoid { get; }

    private TypeReferenceInfo TargetType { get; }
    private static string TargetExpression => "target";

    private static string ValueExpression => "value";

    private static string BufferWriterExpression => "bufferWriter";

    private static string BufferReaderExpression => "bufferReader";

    public SerializationTest(TypeReferenceInfo targetType)
        : this(new SerializationResolver(Array.Empty<KeyValuePair<TypeReferenceInfo, CustomSerializationInfo>>()), targetType) { }

    private SerializationTest(SerializationResolver serializationResolver, TypeReferenceInfo targetType)
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

        bool isResolved = serialization.IsResolved;
        testContext?.WriteLine($"Is resolved: {isResolved}");
        Assert.IsTrue(isResolved);

        bool isVoid = serialization.IsVoid;
        testContext?.WriteLine($"Is void: {isVoid}");
        Assert.AreEqual(ExpectedIsVoid, isVoid);

        string declarationSyntax = serialization.DeclarationSyntax;
        testContext?.WriteLine($"Declaration syntax: {declarationSyntax}");
        if (ExpectedDeclarationSyntax != null)
        {
            Assert.AreEqual(ExpectedDeclarationSyntax, declarationSyntax);
        }

        string sizeExpression = GetSizeExpression(serialization, TargetExpression);
        testContext?.WriteLine($"Size expression: {sizeExpression}");
        if (ExpectedSizeExpression != null)
        {
            Assert.AreEqual(ExpectedSizeExpression, sizeExpression, CodeComparer.Instance);
        }

        string serializationStatement = GetFinalizedSerializationStatement(serialization, BufferWriterExpression, ValueExpression);
        testContext?.WriteLine($"Serialization statement: {serializationStatement}");
        if (ExpectedSerializationStatement != null)
        {
            Assert.AreEqual(ExpectedSerializationStatement, serializationStatement, CodeComparer.Instance);
        }

        string deserializationExpression = GetFinalizedDeserializationExpression(serialization, BufferReaderExpression);
        testContext?.WriteLine($"Deserialization expression: {deserializationExpression}");
        if (ExpectedDeserializationExpression != null)
        {
            Assert.AreEqual(ExpectedDeserializationExpression, deserializationExpression, CodeComparer.Instance);
        }
    }

    private class CodeComparer : IEqualityComparer<string>
    {
        public static readonly CodeComparer Instance = new();

        public bool Equals(string? left, string? right)
            => left == null
                ? right == null
                : left.ReplaceLineEndings() == right!.ReplaceLineEndings();

        public int GetHashCode(string target) => target.ReplaceLineEndings().GetHashCode();
    }

    public string GetSizeExpression() => GetSizeExpression(_serializationResolver.Resolve(TargetType), TargetExpression);
    public string GetFinalizedDeserializationExpression() => GetFinalizedDeserializationExpression(_serializationResolver.Resolve(TargetType), BufferReaderExpression);

    public string GetFinalizedSerializationStatement()
        => GetFinalizedSerializationStatement(_serializationResolver.Resolve(TargetType), BufferWriterExpression, ValueExpression);

    private static string GetSizeExpression(ISerialization serialization, string targetExpression)
    {
        return GetCode(writer => serialization.WriteSizeExpression(writer, targetExpression));
    }

    private static string GetFinalizedDeserializationExpression(ISerialization serialization, string bufferReaderExpression)
    {
        return GetCode(writer => writer.WriteFinalizedDeserializationStatement(serialization, bufferReaderExpression));
    }

    private static string GetFinalizedSerializationStatement(ISerialization serialization, string bufferWriterExpression, string valueExpression)
    {
        return GetCode(writer => writer.WriteFinalizedSerializationStatement(serialization, bufferWriterExpression, valueExpression));
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