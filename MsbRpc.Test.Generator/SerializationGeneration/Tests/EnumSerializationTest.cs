using Microsoft.VisualStudio.TestTools.UnitTesting;
using MsbRpc.Generator.Info;
using MsbRpc.Generator.Serialization;
using MsbRpc.Generator.Serialization.Default;

namespace MsbRpc.Test.Generator.SerializationGeneration.Tests;

[TestClass]
public class EnumSerializationTest : Base.Test
{
    private static readonly TypeDeclarationInfo EnumDeclaration = new("MyEnum", 0, EnumSerializationKind.Int);

    private static readonly TypeReferenceInfo EnumInfo = new(EnumDeclaration);

    private static ISerialization Serialization => new SerializationResolver().Resolve(EnumInfo);

    [TestMethod]
    public void SerializationIsResolved()
    {
        Assert.IsTrue(Serialization.IsResolved);
    }
}