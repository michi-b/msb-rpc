using System.Collections.Immutable;
using Microsoft.CodeAnalysis;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Misbat.CodeAnalysis.Test.CodeTest;
using Misbat.CodeAnalysis.Test.Utility;
using MsbRpc.Generator;

namespace MsbRpcTest;

[TestClass]
public class SerializableGeneratorTest
{
    private static readonly CodeTest CodeTest = new CodeTest
        (
            new CodeTestConfiguration
            (
                ImmutableArray.Create
                (
                    MetadataReferenceUtility.MsCoreLib,
                    MetadataReferenceUtility.SystemRuntime,
                    MetadataReferenceUtility.NetStandard,
                    MetadataReferenceUtility.GetAssemblyReference<MsbRpsObject>()
                )
            ).WithAdditionalGenerators(new MsbRpsSourceGenerator())
        )
        .WithAddedNamespaceImports("MsbRps.GeneratorAttributes");

    // ReSharper disable once MemberCanBePrivate.Global
    // ReSharper disable once AutoPropertyCanBeMadeGetOnly.Global
    // MSTest needs the public setter
    public TestContext TestContext { private get; set; } = null!;

    private CancellationToken CancellationToken => TestContext.CancellationTokenSource.Token;

    [TestMethod]
    public async Task ValidClassGeneratesSerialization()
    {
        const string code = @"
[MsbRpsObject]
public partial struct Test {}";

        CodeTestResult result = (await CodeTest.WithCode(code).Run(CancellationToken)).Result;
        GeneratorDriverRunResult serializationGeneratorResult = result.GeneratorResults[typeof(MsbRpsSourceGenerator)].GetRunResult();
        Assert.AreEqual(1, serializationGeneratorResult.GeneratedTrees.Length);
    }
}