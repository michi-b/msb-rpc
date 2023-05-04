using System.CodeDom.Compiler;
using System.Collections.Immutable;
using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MsbRpc.Generator.Info;
using MsbRpc.Generator.Serialization;
using MsbRpc.Generator.Utility;

namespace MsbRpc.Test.Generator.SerializationGeneration;

[TestClass]
public class DefaultSerializationsTest : Base.Test
{
    [TestMethod]
    public void TestStringSerialization()
    {
        using IndentedTextWriter textWriter = CreateTextWriter();
        GetSerializationWriter(SimpleDefaultSerializationKind.Bool).WriteSizeExpression(textWriter, "myBool");
        Assert.AreEqual("MsbRpc.Serialization.Primitives.PrimitiveSerializer.BoolSize", GetTextWriterResult(textWriter));
    }

    [TestMethod]
    public void HasAllDefaultSerializations()
    {
        SerializationResolver resolver = CreateResolver();
        foreach (SimpleDefaultSerializationKind serializationKind in SimpleDefaultSerializationKindUtility.All)
        {
            SerializationWriter serializationWriter = resolver.GetSerializationWriter(serializationKind);
            Assert.IsNotNull(serializationWriter);
        }
    }

    private static string? GetTextWriterResult(IndentedTextWriter textWriter) => textWriter.InnerWriter.ToString();

    private static IndentedTextWriter CreateTextWriter() => new(new StringWriter());

    private static SerializationResolver CreateResolver() => new(ImmutableArray<CustomSerializationInfo>.Empty);

    private static SerializationWriter GetSerializationWriter
        (SimpleDefaultSerializationKind serializationKind)
        => CreateResolver().GetSerializationWriter(serializationKind);
}