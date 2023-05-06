using System.Collections.Immutable;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MsbRpc.Generator.Info;
using MsbRpc.Generator.Serialization;

namespace MsbRpc.Test.Generator.SerializationGeneration;

[TestClass]
public class NullableByteSerializationTests : Base.Test
{
    private static readonly TypeReferenceInfo NullableByteTypeReferenceInfo = new
    (
        new TypeDeclarationInfo("System.Nullable", 1),
        false,
        new[] { TypeReferenceInfo.CreateSimple("System.Byte") }.ToImmutableList()
    );

    [TestMethod]
    public void NullableByteSerializationIsResolved()
    {
        SerializationResolver resolver = CreateResolver();
        ISerialization serialization = resolver.Resolve(NullableByteTypeReferenceInfo);
        Assert.IsTrue(serialization.GetIsResolved());
    }

    private static SerializationResolver CreateResolver() => new(ImmutableArray<CustomSerializationInfo>.Empty);
}