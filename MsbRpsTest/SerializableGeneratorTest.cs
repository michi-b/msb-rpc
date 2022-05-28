using Microsoft.CodeAnalysis;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Misbat.CodeAnalysis.Test.CodeTest;
using Misbat.CodeAnalysis.Test.Utility;
using MsbRps.CodeAnalysis;
using MsbRps.Interfaces;
using MsbRpsTest.Utility;

namespace MsbRpsTest;

[TestClass]
public class SerializableGeneratorTest
{
    private static readonly MetadataReference MsbRpsAssembly 
        = MetadataReferenceUtility.GetAssemblyReference<IRpsSerializable>();

    private static readonly CodeTest CodeTest = CodeTestUtility.Default
        .Configure
        (
            configuration => configuration
                .WithAdditionalMetadataReferences(MsbRpsAssembly)
                .WithAdditionalGenerators(new SerializableGenerator())
        )
        .WithAddedNamespaceImports("MsbRps.Interfaces");

    // ReSharper disable once MemberCanBePrivate.Global
    // ReSharper disable once AutoPropertyCanBeMadeGetOnly.Global
    // MSTest needs the public setter
    public TestContext TestContext { private get; set; } = null!;

    private CancellationToken CancellationToken => TestContext.CancellationTokenSource.Token;

    [TestMethod]
    public async Task ValidClassGeneratesSerialization()
    {
        const string code = @"
public partial class Serializable : IRpsSerializable{}";

        CodeTestResult result = (await CodeTest.WithCode(code).Run(CancellationToken)).Result;
        GeneratorDriverRunResult serializationGeneratorResult = result.GeneratorResults[typeof(SerializableGenerator)].GetRunResult();
        Assert.AreEqual(1, serializationGeneratorResult.GeneratedTrees.Length);
    }
}