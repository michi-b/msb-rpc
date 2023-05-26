using Microsoft.VisualStudio.TestTools.UnitTesting;
using MsbRpc.Generator.Info;
using MsbRpc.Generator.Serialization;

namespace MsbRpc.Test.Generator.SerializationGeneration.Tests;

[TestClass]
public class ArraySerializationTest : Base.Test
{
    private static readonly TypeReferenceInfo IntInfo = new(new NamedTypeDeclarationInfo("System.Int32"));

    private static readonly TypeReferenceInfo IntArrayInfo = new(new ArrayDeclarationInfo(IntInfo, 1));

    private static ISerialization Serialization => new SerializationResolver().Resolve(IntArrayInfo);

    [TestMethod]
    public void SerializationIsResolved()
    {
        Assert.IsTrue(Serialization.IsResolved);
    }

    [TestMethod]
    public void SerializationsAreNotVoid()
    {
        Assert.IsFalse(Serialization.IsVoid);
    }
}