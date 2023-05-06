using System.CodeDom.Compiler;
using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MsbRpc.Generator.Info;
using MsbRpc.Generator.Serialization;
using MsbRpc.Generator.Serialization.Default;

namespace MsbRpc.Test.Generator.SerializationGeneration;

internal readonly ref struct SimpleDefaultSerializationTest
{
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

    public SimpleDefaultSerializationTest(TypeReferenceInfo targetType)
    {
        TargetType = targetType;
        ExpectedSizeExpression = null;
        ExpectedSerializationStatement = null;
        ExpectedDeserializationExpression = null;
        ExpectedDeclarationSyntax = null;
        ExpectedIsVoid = false;
    }

    public SimpleDefaultSerializationTest(SimpleDefaultSerializationKind targetType)
        : this(targetType.GetTargetType()) { }

    public void Run(SerializationResolver resolver)
    {
        ISerialization serialization = resolver.Resolve(TargetType);
        Assert.IsTrue(serialization.GetIsResolved());
        Assert.AreEqual(ExpectedIsVoid, serialization.GetIsVoid());
        if (ExpectedDeclarationSyntax != null)
        {
            Assert.AreEqual(ExpectedDeclarationSyntax, serialization.GetDeclarationSyntax());
        }

        if (ExpectedSizeExpression != null)
        {
            string targetExpression = TargetExpression;
            TestWrittenCode(ExpectedSizeExpression, writer => serialization.WriteSizeExpression(writer, targetExpression));
        }

        if (ExpectedSerializationStatement != null)
        {
            string bufferWriterExpression = BufferWriterExpression;
            string valueExpression = ValueExpression;
            TestWrittenCode(ExpectedSerializationStatement, writer => serialization.WriteSerializationStatement(writer, bufferWriterExpression, valueExpression));
        }

        if (ExpectedDeserializationExpression != null)
        {
            string bufferReaderExpression = BufferReaderExpression;
            TestWrittenCode(ExpectedDeserializationExpression, writer => serialization.WriteDeserializationExpression(writer, bufferReaderExpression));
        }
    }

    private delegate void WriteDelegate(IndentedTextWriter writer);

    private static void TestWrittenCode(string expectedCode, WriteDelegate write)
    {
        IndentedTextWriter textWriter = CreateTextWriter();
        write(textWriter);
        Assert.AreEqual(expectedCode, GetTextWriterResult(textWriter));
    }

    private static string? GetTextWriterResult(IndentedTextWriter textWriter) => textWriter.InnerWriter.ToString();
    private static IndentedTextWriter CreateTextWriter() => new(new StringWriter());
}