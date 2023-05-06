using System.CodeDom.Compiler;
using System.Collections.Immutable;
using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MsbRpc.Generator.Info;
using MsbRpc.Generator.Serialization;
using MsbRpc.Generator.Serialization.Default;

namespace MsbRpc.Test.Generator.SerializationGeneration;

[TestClass]
public class DefaultSerializationsTest : Base.Test
{
    [TestMethod]
    public void TestStringSerialization()
    {
        using IndentedTextWriter textWriter = CreateTextWriter();
        GetSerialization(SimpleDefaultSerializationKind.Bool).WriteSizeExpression(textWriter, "myBool");
        Assert.AreEqual("MsbRpc.Serialization.Primitives.PrimitiveSerializer.BoolSize", GetTextWriterResult(textWriter));
        //todo: fix        
    }

    [TestMethod]
    public void HasAllDefaultSerializations()
    {
        SerializationResolver resolver = CreateResolver();
        foreach (SimpleDefaultSerializationKind serializationKind in SimpleDefaultSerializationKindUtility.All)
        {
            ISerialization serialization = resolver.Resolve(serializationKind.GetTargetType());
            Assert.IsNotNull(serialization);
        }
    }

    private static string? GetTextWriterResult(IndentedTextWriter textWriter) => textWriter.InnerWriter.ToString();

    private static IndentedTextWriter CreateTextWriter() => new(new StringWriter());

    private static SerializationResolver CreateResolver() => new(ImmutableArray<CustomSerializationInfo>.Empty);

    private static ISerialization GetSerialization(SimpleDefaultSerializationKind serializationKind) => CreateResolver().Resolve(serializationKind.GetTargetType());
}